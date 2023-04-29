using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] Vector3 offset;

    [Header("Read Only")]
    [SerializeField] Transform targetTf;

    public void ChangeTargetTf(Transform newTargetTf)
    {
        targetTf = newTargetTf;
    }

    private void Update()
    {
        if(targetTf != null)
        {
            transform.position = targetTf.position + offset;
        }
    }
}
