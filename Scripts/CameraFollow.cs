using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator animator;
    [SerializeField] List<Transform> layerTfs;
    [SerializeField] float parallaxFar;

    [Header("Properties")]
    [SerializeField] Vector3 offset;

    [Header("Read Only")]
    [SerializeField] Transform targetTf;
    [SerializeField] bool isZoomAniEnd;
    [SerializeField] Vector2 prevPos;

    public void ChangeTargetTf(Transform newTargetTf)
    {
        targetTf = newTargetTf;
    }

    public void PlayCameraZoomAni()
    {
        animator.SetTrigger("toZoomIn");
        isZoomAniEnd = false;
    }

    public void BackToFollow()
    {
        animator.SetTrigger("toNormal");
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

            foreach(var layerTf in layerTfs)
            {
                float t = Mathf.InverseLerp(0, Mathf.Abs(parallaxFar), Mathf.Abs(layerTf.position.z));

                Vector2 delta = (Vector2)transform.position - prevPos;
                delta = Vector2.Lerp(Vector2.zero, delta, t);
                layerTf.position += new Vector3(delta.x, delta.y, 0);
            }
                prevPos = transform.position;
        }
    }

    void Animation_OnZoomInAniEnd()
    {
        isZoomAniEnd = true;
    }
}
