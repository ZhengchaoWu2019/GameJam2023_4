using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    enum GameState {Start, Normal, End };

    static bool isReload = false;

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

        cameraFollow.ChangeTargetTf(currentFireGhost.transform);
    }

    public void ChangeControlToPlayer()
    {
        currentPlayer.enabled = true;

        cameraFollow.ChangeTargetTf(currentPlayer.transform);

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

    public bool CheckIsSceneStart()
    {
        if(gameState == GameState.Start)
        {
            return true;
        }

        return false;
    }

    public void PlayerDead()
    {
        Destroy(currentPlayer.gameObject);

        cameraFollow.PlayCameraZoomAni();
        uiManager.PlaySceneEndAni();

        currentWait = new Wait();
        currentWait.AddCheckFunction(cameraFollow.CheckIsZoomAniEnd);
        currentWait.AddCheckFunction(uiManager.CheckIsSceneEndAniFinished);
        currentWait.SetAfterFinishedCallbackFunc(RespawnPlayer);

        StartCoroutine(currentWait.StartWait());
    }

    public void EnterSceneEnd()
    {
        cameraFollow.PlayCameraZoomAni();
        uiManager.PlaySceneEndAni();

        currentWait = new Wait();
        currentWait.AddCheckFunction(cameraFollow.CheckIsZoomAniEnd);
        currentWait.AddCheckFunction(uiManager.CheckIsSceneEndAniFinished);
        currentWait.SetAfterFinishedCallbackFunc(GotoNextScene);

        StartCoroutine(currentWait.StartWait());

        isReload = false;
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

        if (!isReload)
        {
            ChangeGameState(GameState.Start);
        }
        else
        {
            InstantiatePlayer(true);
            ChangeGameState(GameState.Normal);
        }
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
                EnterSceneEndDialogue();
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

        uiManager.ShowStartSceneUI();
    }

    void EnterPlayerControl()
    {
        currentPlayer.enabled = true;
    }

    void EnterSceneEndDialogue()
    {
        currentPlayer.enabled = false;
        currentPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        uiManager.ShowEndSceneUI();
    }
    void GotoNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void RespawnPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isReload = true;
    }

    void InstantiatePlayer(bool canControl)
    {
        currentPlayer = Instantiate(player_prefab, spawnPointTf.position, Quaternion.identity).GetComponent<Player>();
        currentPlayer.enabled = canControl;

        cameraFollow.ChangeTargetTf(currentPlayer.transform);
    }
    #endregion
}
