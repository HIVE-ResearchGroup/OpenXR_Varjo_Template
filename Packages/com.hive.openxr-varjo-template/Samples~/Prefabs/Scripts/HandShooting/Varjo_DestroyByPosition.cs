﻿using UnityEngine;

public class Varjo_DestroyByPosition : MonoBehaviour
{
    public float destroyPositionY = -10;
    void Update()
    {
        if(transform.position.y < destroyPositionY)
        {
            Destroy(this.gameObject);
        }
    }
}
