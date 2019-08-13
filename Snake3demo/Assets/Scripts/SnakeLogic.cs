using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SnakeLogic
{
    #region Values

    private static int _snakeSize;
    private Snake _snake;
    private bool _appleFound;
    private Vector3 _applePosition;
    private Vector3 _directionToApple;
    private Vector3 _oldPosition;
    
    private int _xHead = 5;
    private int _yHead = 5;
    private int _zHead = 8;
    
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
    private List<Vector3> pathToApple = new List<Vector3>();
    
    public delegate void OnPosChanged(Transform transform, Vector3 pos);

    public delegate void OnAppleEated();

    public event OnPosChanged Del = delegate { }; 
    public event OnAppleEated Add = delegate { };
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
    }

    public void SetSnakeParts(Snake snake)
    {
        _snake = snake;
        snake.transform.GetChild(0).position = new Vector3(_xHead, _yHead, _zHead);
        
        for (int i = 0; i < local.general.snake.count; i++) 
        {
            snake.transform.GetChild(i).position = snake.transform.GetChild(0).position + new Vector3(i, 0, 0);
            SnakeBody.Add(snake.transform.GetChild(i));
        }
    }
    
    public void Move()
    {    
        if(!_appleFound)SearchApple();
        
        Vector3 direction = _appleFound ? MoveToApple() : CheckFreeSpaceExeptItself();
        
        _oldPosition = SnakeBody[0].position;
        
        Vector3 newPos = SnakeBody[0].position + direction;
        
        Del(SnakeBody[0], newPos);
        
        for (int i = 1; i < SnakeBody.Count; i++)
        {
            _oldPosition = SnakeBody[i - 1].position;
            Del.Invoke(SnakeBody[i] , _oldPosition);
        }
        
        Snake.lastMove = Time.time;
    }

    public Vector3 MoveToApple()
    {
        if (pathToApple.Count == 0)
        {
            CheckConnectionWithHead();
            return CheckFreeSpaceExeptItself();
        }
        
        Vector3 direction = pathToApple[0];
        pathToApple.RemoveAt(0);
        
        return direction;
    }
    
    public void SearchApple()
    {
        Transform transform = Grid.SearchForApple();
        
        if ( transform != null)
        {
            _applePosition = transform.position;
            GetMoveDirectionToApple();
            _appleFound = true;
        }
    }
    
    private void GetMoveDirectionToApple()
    {
        Vector3 checkingPosition = SnakeBody[0].position;
        Vector3 value = _applePosition  - checkingPosition; 
        
        float xDistance = Mathf.Abs((int)value.x);
        float yDistance = Mathf.Abs((int)value.y);
        float zDistance = Mathf.Abs((int)value.z);

        float xx = getPoint(value.x);
        while (xDistance != 0)
        {
            xDistance--;
            pathToApple.Add(new Vector3(xx, 0, 0));
        }
        
        float yy = getPoint(value.y);
        while (yDistance != 0)
        {
            yDistance--;
            pathToApple.Add(new Vector3(0, yy, 0));
        }
        
        float zz = getPoint(value.z);
        while (zDistance != 0)
        {
            zDistance--;
            pathToApple.Add(new Vector3(0, 0, zz));
        }
    }

    public float getPoint(float value)
    {
        float dir  = 0f; 
        
        if (value < 0)
        {
            dir = -1f;
        }
        else if(value > 0)
        {
            dir = 1f;
        }

        return dir;
    }
    
    private Vector3 CheckFreeSpaceExeptItself()
    {
        Vector3 direction = SnakeBody[0].position;
        
        Random random = new Random();
        Vector3 randomDirectionToMove = directions[random.Next(0, directions.Length) ];
        
        direction += randomDirectionToMove;
        
        int count = 10;
        while (Grid.GetTransformOnPoint(direction))
        {    
            direction = SnakeBody[0].position;
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
        Transform apple = Grid.GetAppleByVector3(_applePosition);
        apple.GetComponent<Apple>().DestrouItselfWhenEated();
        _appleFound = false;
        Add.Invoke();
    }
    
    #endregion
}
