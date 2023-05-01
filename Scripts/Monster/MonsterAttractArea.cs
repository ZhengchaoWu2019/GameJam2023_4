using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttractArea : MonoBehaviour
{
    public System.Action<Vector2> OnAttractWoodFiring;

    [Header("Components")]
    [SerializeField] Wood attractWood;

    [Header("Read Only")]
    [SerializeField] bool attractOnce = true;

    private void Update()
    {
        if(attractWood != null)
        {
            if (attractWood.CheckIsFiring())
            {
                if (attractOnce)
                {
                    attractOnce = false;
                    OnAttractWoodFiring?.Invoke(attractWood.transform.position);
                }
            }
        }
    }
}
