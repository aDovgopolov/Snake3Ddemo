using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    void Start()
    {
        Grid.grid3D[(int)transform.position.y, (int)transform.position.x, (int)transform.position.z] = transform;
    }

    public void DestrouItselfWhenEated()
    {
        Destroy(gameObject);
    }
}
