using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    enum GameState {Start, Normal, End };

    [Header("Prefabs")]
    [SerializeField] GameObject player_prefab;
    [SerializeField] GameObject fireGhost_prefab;

    [Header("Components")]
    [SerializeField] UI_Manager uiManager;
    [SerializeField] Transform spawnPointTf;
    [SerializeField] CameraFollow cameraFollow;

    [Header("Read Only")]
    [SerializeField] GameState gameState;
    [SerializeField] bool enterStateOnce = true;
    [SerializeField] Player currentPlayer;
    [SerializeField] GameObject currentFireGhost;
    [SerializeField] Wait currentWait;

    #region singleton
    public static GameManager singleton;
    #endregion

    #region Public Methods
    public Player GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void ChangeControlToFireGhost()
    {
        if (currentFireGhost != null)
        {
            return;
        }

        currentPlayer.enabled = false;

        currentFireGhost = Instantiate(fireGhost_prefab, currentPlayer.transform.position, Quaternion.identity);
    }

    public void ChangeControlToPlayer()
    {
        currentPlayer.enabled = true;

        Destroy(currentFireGhost);
    }

    public void PlayerStartControl()
    {
        ChangeGameState(GameState.Normal);
    }

    public void PlayerFinishScene()
    {
        ChangeGameState(GameState.End);
    }
    #endregion

    #region Life Cycle
    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        uiManager.HideUI();

        ChangeGameState(GameState.Start);
    }

    private void Update()
    {
        if (!enterStateOnce)
        {
            return;
        }

        switch (gameState)
        {
            case GameState.Start:
                enterStateOnce = false;
                EnterSceneStart();
                break;
            case GameState.Normal:
                enterStateOnce = false;
                EnterPlayerControl();
                break;
            case GameState.End:
                enterStateOnce = false;
                EnterSceneEnd();
                break;
        }
    }
    #endregion

    #region Private Methods
    void ChangeGameState(GameState newState)
    {
        enterStateOnce = true;
        gameState = newState;
    }

    void EnterSceneStart()
    {
        InstantiatePlayer(false);

        uiManager.ShowUI();
    }

    void EnterPlayerControl()
    {
        currentPlayer.enabled = true;
    }

    void EnterSceneEnd()
    {
        currentPlayer.enabled = false;
        cameraFollow.PlayCameraZoomAni();
        uiManager.PlaySceneEndAni();

        currentWait = new Wait();
        currentWait.AddCheckFunction(cameraFollow.CheckIsZoomAniEnd);
        currentWait.AddCheckFunction(uiManager.CheckIsSceneEndAniFinished);
        currentWait.SetAfterFinishedCallbackFunc(GotoNextScene);

        StartCoroutine(currentWait.StartWait());
    }

    void GotoNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void InstantiatePlayer(bool canControl)
    {
        currentPlayer = Instantiate(player_prefab, spawnPointTf.position, Quaternion.identity).GetComponent<Player>();
        currentPlayer.enabled = canControl;

        cameraFollow.ChangeTargetTf(currentPlayer.transform);
    }
    #endregion
}
