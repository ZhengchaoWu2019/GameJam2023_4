using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    [SerializeField] GameObject resetGO;

    void Animation_OnPlayEnd()
    {
        resetGO.SetActive(true);
    }
}
