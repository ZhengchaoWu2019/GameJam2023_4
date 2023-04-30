using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SceneEndAni : MonoBehaviour
{
    [Header("Read Only")]
    [SerializeField] bool isAniEnd;

    public bool CheckIsAniEnd()
    {
        return isAniEnd;
    }

    void Animation_OnSceneEndAniFinished()
    {
        isAniEnd = true;
    }
}
