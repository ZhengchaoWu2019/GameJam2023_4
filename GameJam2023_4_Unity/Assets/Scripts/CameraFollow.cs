using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator animator;

    [Header("Properties")]
    [SerializeField] Vector3 offset;

    [Header("Read Only")]
    [SerializeField] Transform targetTf;
    [SerializeField] bool isZoomAniEnd;

    public void ChangeTargetTf(Transform newTargetTf)
    {
        targetTf = newTargetTf;
    }

    public void PlayCameraZoomAni()
    {
        animator.SetTrigger("toZoomIn");
        isZoomAniEnd = false;
    }

    public bool CheckIsZoomAniEnd()
    {
        return isZoomAniEnd;
    }

    private void Update()
    {
        if(targetTf != null)
        {
            transform.position = targetTf.position + offset;
        }
    }

    void Animation_OnZoomInAniEnd()
    {
        isZoomAniEnd = true;
    }
}
