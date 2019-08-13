using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SnakeLogic
{
    #region Values

    private int _snakeSize;
    private bool _appleFound;
    private Vector3 _applePosition;
    private Vector3 _directionToApple;
    private Vector3 _headPosition;
    private Vector3 _oldPosition;

    private float _x = 0f;
    private float _y = 0f;
    private float _z = 0f;

    private Vector3[] directions =
    {
        new Vector3(0, -1, 0),
        new Vector3(0, 1, 0),
        new Vector3(-1, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1)
    };

    public List<Transform> SnakeBody = new List<Transform>();

    public delegate void OnPosChanged(Transform transform, Vector3 pos);

    public delegate void OnAppleEated();

    public event OnPosChanged Del = delegate { }; 
    public event OnAppleEated Add;
    #endregion

    
    #region Methods
    
    public void RegisterHandler(OnPosChanged del, OnAppleEated add)
    {
        Del = del;
        Add = add;
    }
    
    public SnakeLogic(int snakeSize)
    {
        _snakeSize = snakeSize;
        _headPosition = new Vector3(_x, _y, _z);
    }

    public void Move()
    {    
        // SetGridInfo();
        Vector3 direction = _appleFound ? GetMoveDirectionToApple() :  CheckFreeSpaceExeptItself();
        
        Debug.Log("_headPosition = "  + _headPosition  + $" - {direction}");
        Vector3 newPOs = _headPosition += direction;
        
        _oldPosition = _headPosition; 
        
        Debug.Log("_headPosition = "  + _headPosition  + $" - {direction} + {newPOs}" );
        
        Del(SnakeBody[0] , newPOs);
        //_headPosition = newPOs;
        LookForward(_headPosition, direction);
        
        for (int i = 1; i < SnakeBody.Count; i++)
        {
            _oldPosition = SnakeBody[i - 1].position;
            Del?.Invoke(SnakeBody[i] , _oldPosition);
        }
        
        Snake._lastMove = Time.time;
    }
    
    private void LookForward(Vector3 headPosition, Vector3 direction)
    {   
        int layerMask = 1 << 9;

        if (Physics.Raycast(headPosition, direction, out var hit, Mathf.Infinity, layerMask))
        {
            _appleFound = true;
            _applePosition = hit.transform.position;
        }
        else
        {
            Debug.DrawLine(headPosition, direction, Color.white, 1f);
        }
    }
    
    private Vector3 GetMoveDirectionToApple()
    {
        Debug.Log("GetMoveDirectionToApple");
        
        Vector3 value = _applePosition - _headPosition;  // gameObject.transform.GetChild(0).position;
        int tmp = 0;
        
        if (value.x != 0.0f)
        {
            tmp = (int)Mathf.Abs(value.x / value.x);
            if (value.x < 0)
                tmp = tmp * (-1);
            _directionToApple = new Vector3(tmp, value.y, value.z);
        }
        else if (value.y != 0.0f)
        {
            tmp = (int)Mathf.Abs(value.y / value.y);
            if (value.y < 0)
                tmp = tmp * (-1);
            _directionToApple = new Vector3(value.x , tmp, value.z);
        }
        else if(value.z != 0.0f)
        {
            tmp = (int)Mathf.Abs(value.z / value.z);
            if (value.z < 0)
                tmp = tmp * (-1);
            _directionToApple = new Vector3(value.x, value.y, tmp);
            
        }
        
        Debug.Log(tmp);
        return _directionToApple;
        
    }
    
    private Vector3 CheckFreeSpaceExeptItself()
    {
        Vector3 direction = _headPosition; //gameObject.transform.GetChild(0).position;
        
        Random random = new Random();
        Vector3 randomDirectionToMove = directions[random.Next(0, directions.Length) ];
        
        direction += randomDirectionToMove;
        
        int count = 10;
        while (Grid.GetTransformOnPoint(direction))
        {    
            direction = _headPosition;
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
    
    public void CheckConnectionWithHead()
    {
        Add?.Invoke();
        _appleFound = false;
        Debug.Log("Success");
    }
    
    
    
    #endregion
}
