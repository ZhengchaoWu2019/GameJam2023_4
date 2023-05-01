using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAni : MonoBehaviour
{
    public System.Action OnDieAniEnd;
    void Animation_OnDieAniEnd()
    {
        OnDieAniEnd?.Invoke();
    }
}
