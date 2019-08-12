using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTrigger : MonoBehaviour
{
    private Snake snake;

    private void Start()
    {
        snake = GameObject.Find("TrueSnake").GetComponent<Snake>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Food"))
        {
            Debug.Log("OnTriggerEnter = "  +other.name);
            if (snake != null)
            {
                Destroy(other.gameObject);
                snake.CheckConnectionWithHead();
            }
            else
            {
                Debug.LogError("snake == null");
            }
        }
    }
}
