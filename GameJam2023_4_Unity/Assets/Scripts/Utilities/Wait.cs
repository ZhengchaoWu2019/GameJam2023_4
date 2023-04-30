using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wait
{
    List<System.Func<bool>> waitCheckFun = new List<System.Func<bool>>();
    System.Action callback;

    public void AddCheckFunction(System.Func<bool> checkFunc)
    {
        waitCheckFun.Add(checkFunc);
    }

    public void SetAfterFinishedCallbackFunc(System.Action afterCallback)
    {
        callback = afterCallback;
    }

    public IEnumerator StartWait()
    {
        yield return new WaitUntil(CheckFinished);

        callback?.Invoke();
    }

    bool CheckFinished()
    {
        foreach(var func in waitCheckFun)
        {
            if (!func())
            {
                return false;
            }
        }

        return true;
    }
}
