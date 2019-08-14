using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using Random = System.Random;

public class SnakeLogic
{
    #region Values

    private readonly Snake _snake;
    private readonly SnakeData _snakeData;
    
    private const int ValueX = 1;
    private const int ValueY = 2;
    private const int ValueZ = 3;
    
    private readonly Vector3[] _directions =
    {
        new Vector3(0, -1, 0),
        new Vector3(0, 1, 0),
        new Vector3(-1, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1)
    };
    
    #region Delegates
    
    public delegate void OnPosChanged(Transform transform, Vector3 pos);
    public delegate void OnSnakeCreated();
    public delegate void OnAppleEated();

    public event OnPosChanged Del = delegate { }; 
    public event OnAppleEated Add = delegate { };
    public event OnSnakeCreated Draw = delegate { };
    
    public void RegisterHandler(OnPosChanged del, OnAppleEated add, OnSnakeCreated draw)
    {
        Del = del;
        Add = add;
        Draw = draw;
    }
    
    #endregion
    
    #endregion
    
    #region Methods
    
    public SnakeLogic(int snakeSize, Snake snake)
    {
        _snake = snake;
        _snakeData = new SnakeData(snakeSize, snake);
        //_snakeData = Deserialize<SnakeData>("Assets\\Resources\\SnakeData.xml");
        //_snakeData.setSnakeUI(snake);
        
        string str = Application.dataPath + $"\\Resources\\SnakeData.xml";
        
        if (File.Exists(str))
        {
            _snakeData = Deserialize<SnakeData>($"Assets\\Resources\\SnakeData.xml");
            _snakeData.setSnakeUI(snake);
            Debug.Log(str);
        }
        else
        {
            _snakeData = new SnakeData(snakeSize, snake);
        }
        
        SetGridInfo();
    }
    
    public static SnakeData Deserialize<SnakeData>(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SnakeData));
        StreamReader reader = new StreamReader(path);
        SnakeData deserialized = (SnakeData) serializer.Deserialize(reader.BaseStream);
        reader.Close();
        return deserialized;
    }
    
    public void Move()
    {    
        if(!_snakeData.AppleFound) SearchApple();
        
        Vector3 direction = _snakeData.AppleFound ? MoveToApple() : CheckFreeSpaceExeptItself();
        
        Vector3 oldPosition = _snakeData.snakeBody[0].position;
        
        Vector3 newPos = _snakeData.snakeBody[0].position + direction;
        
        Del(_snakeData.snakeBody[0], newPos);
        
        for (int i = 1; i < _snakeData.snakeBody.Count; i++)
        {
            oldPosition = _snakeData.snakeBody[i - 1].position;
            Del(_snakeData.snakeBody[i] , oldPosition);
        }
        
        Snake.lastMove = Time.time;
    }

    public Vector3 MoveToApple()
    {
        if (_snakeData.pathToApple.Count == 0)
        {
            CheckConnectionWithHead();
            return CheckFreeSpaceExeptItself();
        }
        
        Vector3 direction = _snakeData.pathToApple[0];
        _snakeData.pathToApple.RemoveAt(0);
        
        return direction;
    }
    
    public void SearchApple()
    {
        Transform transform = Grid.SearchForApple();
        
        if ( transform != null)
        {
            _snakeData.ApplePosition = transform.position;
            GetMoveDirectionToApple();
            _snakeData.AppleFound = true;
        }
    }
    
    private void GetMoveDirectionToApple()
    {
        Vector3 checkingPosition = _snakeData.snakeBody[0].position;
        Vector3 value = _snakeData.ApplePosition  - checkingPosition; 

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
                _snakeData.pathToApple.Add(new Vector3(target,0, 0));
            else if(valueChecker == ValueY)
                _snakeData.pathToApple.Add(new Vector3(0,target, 0));
            else
                _snakeData.pathToApple.Add(new Vector3(0,0, target));
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
        Vector3 direction = _snakeData.snakeBody[0].position;
        
        Random random = new Random();
        Vector3 randomDirectionToMove = _directions[random.Next(0, _directions.Length) ];
        
        direction += randomDirectionToMove;
        
        int count = 0;
        while (Grid.GetTransformOnPoint(direction))
        {    
            if (count == 6)
            {
                Debug.LogError("CheckFreeSpaceExeptItself: no free space");
                //Debug.Break();
                break;
            }
            
            direction = _snakeData.snakeBody[0].position;
            randomDirectionToMove = _directions[count];
            direction += randomDirectionToMove;
            count++;
            //Debug.Break();
        }
        
        return randomDirectionToMove;
    }
    
    public void CheckConnectionWithHead()
    {
        Transform apple = Grid.GetAppleByVector3(_snakeData.ApplePosition);
        apple.GetComponent<Apple>().DestrouItselfWhenEated();
        _snakeData.AppleFound = false;
        Add.Invoke();
        _snakeData.SnakeSize++;
    }
    
    public int GetSnakeSize()
    {
        return _snakeData.SnakeSize;
    }
    
    public void SetGridInfo()
    {
        if (Grid.IsValidGridPos3D(_snake))
        {
            Grid.UpdateGrid3D(_snake);
        }
        else
        {
            Debug.Log("Debug SetGridInfo");
            //Debug.Break();
        }
    }

    public void AddBoneToSnakeBody(Transform _snakePart)
    {
        _snakeData.snakeBody.Add(_snakePart.transform);
    }

    public void Save()
    {
        _snakeData.Save();
    }

    #endregion
}
