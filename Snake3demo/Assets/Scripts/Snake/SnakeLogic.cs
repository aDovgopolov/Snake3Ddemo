using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SnakeLogic
{
    #region Values

    private Snake _snake;
    private SnakeData _snakeData;
    
    private const int ValueX = 1;
    private const int ValueY = 2;
    private const int ValueZ = 3;
    private Vector3 _applePosition;
    
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
    public delegate void OnSnakeCreated();
    public delegate void OnAppleEated();

    public event OnPosChanged Del = delegate { }; 
    public event OnAppleEated Add = delegate { };
    public event OnSnakeCreated Draw = delegate { };
    #endregion
    
    #region Methods
    
    public void RegisterHandler(OnPosChanged del, OnAppleEated add, OnSnakeCreated draw)
    {
        Del = del;
        Add = add;
        Draw = draw;
    }
    
    public SnakeLogic(int snakeSize, Snake snake)
    {
        _snake = snake;
        _snakeData = new SnakeData(snakeSize, snake);
    }
    
    public void Move()
    {    
        if(!_snakeData.AppleFound)SearchApple();
        
        Vector3 direction = _snakeData.AppleFound ? MoveToApple() : CheckFreeSpaceExeptItself();
        
        Vector3 oldPosition = SnakeBody[0].position;
        
        Vector3 newPos = SnakeBody[0].position + direction;
        
        Del(SnakeBody[0], newPos);
        
        for (int i = 1; i < SnakeBody.Count; i++)
        {
            oldPosition = SnakeBody[i - 1].position;
            Del.Invoke(SnakeBody[i] , oldPosition);
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
            _snakeData.AppleFound = true;
        }
    }
    
    private void GetMoveDirectionToApple()
    {
        Vector3 checkingPosition = SnakeBody[0].position;
        Vector3 value = _applePosition  - checkingPosition; 

        CreatePathBy(value.x, ValueX);
        CreatePathBy(value.y, ValueY);
        CreatePathBy(value.z, ValueZ);
    }

    private void CreatePathBy(float value, int valueChecker)
    {   
        float distance = Mathf.Abs((int)value);
        float target = getPoint(value);
        
        while (distance != 0)
        {
            distance--;
            if(valueChecker == ValueX)
                pathToApple.Add(new Vector3(target,0, 0));
            else if(valueChecker == ValueY)
                pathToApple.Add(new Vector3(0,target, 0));
            else
                pathToApple.Add(new Vector3(0,0, target));
        }
    }
    
    private float getPoint(float value)
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
        
        int count = 0;
        while (Grid.GetTransformOnPoint(direction))
        {    
            if (count == 6)
            {
                Debug.LogError("CheckFreeSpaceExeptItself: no free space");
                Debug.Break();
                break;
            }
            
            direction = SnakeBody[0].position;
            randomDirectionToMove = directions[count];
            direction += randomDirectionToMove;
            Debug.Log($"count = {count } + direction = {direction}  + _appleFound {_snakeData.AppleFound}");
            count++;
            Debug.Break();
            
        }
        return randomDirectionToMove;
    }
    
    public void CheckConnectionWithHead()
    {
        Transform apple = Grid.GetAppleByVector3(_applePosition);
        apple.GetComponent<Apple>().DestrouItselfWhenEated();
        _snakeData.AppleFound = false;
        Add.Invoke();
        _snakeData.SnakeSize++;
    }
    
    public int GetSnakeSize()
    {
        return _snakeData.SnakeSize;
    }
    #endregion
}
