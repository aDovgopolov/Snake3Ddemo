﻿using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = System.Random;
using DG.Tweening;

public class Snake : MonoBehaviour
{    
    private float lastMove = 1f;
    private int _x = 5;
    private int _y = 5;
    private int _z = 8;
    private Vector3 oldPosition;
    private Vector3 newPos;
    private bool _appleFound = false;
    private Vector3 _applePosotion;
    private Vector3 directionToApple;
    
    private Vector3[] directions = {
              new Vector3(0, -1, 0), 
              new Vector3(0, 1, 0), 
              new Vector3(-1, 0, 0), 
              new Vector3(1, 0, 0), 
              new Vector3(0, 0, 1), 
              new Vector3(0, 0, -1)
          };

    private void Awake()
    {
        Random random = new Random();
        /*_x = random.Next(0, 15);
        _y = random.Next(0, 15);
        _z = random.Next(0, 15);*/
    }

    private void Start()
    {
        //local.init("local");
        loader loader = gameObject.AddComponent<loader>();
        GetComponent<Transform>().position = new Vector3(_x, _y, _z);
        
        // TODO can be out of range on Start
        Grid.UpdateGrid3D(this);
    }

    void Update()
    {
        if (Time.time - lastMove >= 1f )
        {
            Movement();
            /*if (!_appleFound)
                
            else
                MovementToApple();*/
        }
    }

    public void MovementToApple()
    {
        Debug.Log("MovementToApple");
        
        SetGridInfo();
        // neto
        Vector3 direction = _applePosotion;
        
        Vector3 oldPosition  = transform.GetChild(0).transform.position;
        
        
        Vector3 pos = transform.GetChild(0).transform.position += direction;
        move(transform.GetChild(0).transform , pos);
        
        LookForward(transform.GetChild(0).transform.position, direction);
        
        for (int i = 1; i < transform.GetChildCount(); i++)
        {
            move(transform.GetChild(i).transform, oldPosition);
            oldPosition = transform.GetChild(i).transform.position;
        }
        
        lastMove = Time.time;
    }
    
    public void Movement()
    {
        SetGridInfo();
        
        
        Vector3 direction = _appleFound ? GetMoveDirectionToApple() :  CheckFreeSpaceExeptItself();
        
        Vector3 oldPosition  = transform.GetChild(0).transform.position;
        
        
        Vector3 pos = transform.GetChild(0).transform.position += direction;
        move(transform.GetChild(0).transform , pos);
        
        LookForward(transform.GetChild(0).transform.position, direction);
        
        for (int i = 1; i < transform.GetChildCount(); i++)
        {
            move(transform.GetChild(i).transform, oldPosition);
            oldPosition = transform.GetChild(i).transform.position;
        }
        
        lastMove = Time.time;
    }

    private void LookForward(Vector3 headPosition, Vector3 direction)
    {   
        int layerMask = 1 << 9;

        
        RaycastHit hit;
        Vector3 directionLine = headPosition + (direction * 15);
        
        if (Physics.Raycast(headPosition, direction, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(headPosition, direction , Color.yellow, 1f);
            Debug.DrawLine(headPosition, directionLine, Color.red, 1f);
            Debug.Log($"Did Hit {hit} and {hit.transform.position} and {hit.transform.gameObject.name}");
            _appleFound = true;
            _applePosotion = hit.transform.position;
            Debug.Break();
        }
        else
        {
            Debug.DrawLine(headPosition, direction, Color.white, 1f);
            Debug.Log("Did not Hit");
        }
    }
    
    private void move(Transform transform, Vector3 pos)
    {
        Sequence mySequence = DOTween.Sequence();
        transform.DOScale(new Vector3(0.5f, 0.5f, 1), 0.5f).OnComplete(() => OnScaleRevert(transform));
        mySequence.Append(transform.DOMove(pos, 1));
    }
   
    private void OnScaleRevert(Transform transform)
    {
        transform.DOScale(new Vector3(1f, 1f, 1), 0.5f);
    }

    private Vector3 GetMoveDirectionToApple()
    {
        Debug.Log("GetMoveDirectionToApple");
        
        Vector3 value = _applePosotion - gameObject.transform.GetChild(0).position;
        
        Debug.Log("value = " + value);
        
        if (value.x != 0.0f)
        {
            directionToApple = new Vector3(Math.Abs(value.x / value.x), value.y, value.z);
        }
        else if (value.y != 0.0f)
        {
            directionToApple = new Vector3(value.x , Math.Abs(value.y / value.y), value.z);
        }
        else if(value.z != 0.0f)
        {
            directionToApple = new Vector3(value.x, value.y, Math.Abs(value.z / value.z));
            
        }
        Debug.Log("direction = " +  directionToApple);
        Debug.Break();
        
        return directionToApple;
    }
    
    private Vector3 CheckFreeSpaceExeptItself()
    {
        Vector3 direction = gameObject.transform.GetChild(0).position;
        
        Random random = new Random();
        Vector3 randomDirectionToMove = directions[random.Next(0, directions.Length) ];
        
        direction += randomDirectionToMove;
        
        int count = 10;
        while (Grid.GetTransformOnPoint(direction))
        {    
            direction = gameObject.transform.GetChild(0).position;
            randomDirectionToMove = directions[random.Next(0, directions.Length)];
            direction += randomDirectionToMove;
            count--;
            
            if (count < 0)
            {
                Debug.LogError("CheckFreeSpaceExeptItself: no free space");
                break;
            }
        }
        
        return randomDirectionToMove;
    }
    
    public void SetGridInfo()
    {
        if (Grid.IsValidGridPos3D(this))
        {
            Grid.UpdateGrid3D(this);
        }
    }
    
}
