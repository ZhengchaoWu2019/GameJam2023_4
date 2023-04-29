using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject fireGhost;

    [Header("Components")]
    [SerializeField] CameraFollow cameraFollow;

    [Header("Read Only")]
    [SerializeField] Player currentPlayer;
    [SerializeField] Wood firingWood;
    [SerializeField] GameObject currentFireGhost;

    #region singleton
    public static GameManager singleton;
    private void Awake()
    {
        singleton = this;
    }
    #endregion

    public void SetFiringWood(Wood currentFiringWood)
    {
        firingWood = currentFiringWood;
    }

    public bool CheckIsCurrentFiringWood(GameObject wood)
    {
        if (wood.Equals(firingWood.gameObject))
        {
            return true;
        }

        return false;
    }

    public void ChangeControlToFireGhost()
    {
        if(firingWood != null)
        {
            if(currentFireGhost != null)
            {
                return;
            }

            currentPlayer.enabled = false;

            currentFireGhost = Instantiate(fireGhost, firingWood.transform.position, Quaternion.identity);
            firingWood.Extinct();

            cameraFollow.ChangeTargetTf(currentFireGhost.transform);
        }
    }

    public void ChangeControlToPlayer()
    {
        currentPlayer.enabled = true;

        Destroy(currentFireGhost);

        cameraFollow.ChangeTargetTf(currentPlayer.transform);

        firingWood = null;
    }
}
