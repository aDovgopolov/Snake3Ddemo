using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeData : IDataManager
{    
    #region Values

    public int SnakeSize { get; set; }
    public bool AppleFound { get; set; } = false;

    private Vector3 _applePosition;
    
    private int _xHead = 5;
    private int _yHead = 5;
    private int _zHead = 8;
    
    public List<Transform> SnakeBody = new List<Transform>();
    private List<Vector3> pathToApple = new List<Vector3>();
    
    #endregion
    
    public SnakeData()
    {
    }

    public SnakeData(int snakeSize, Snake snake)
    {
        SnakeSize = snakeSize;
        SetSnakeParts(snake);
    }
    
    private void SetSnakeParts(Snake snake)
    {
        snake.transform.GetChild(0).position = new Vector3(_xHead, _yHead, _zHead);
        
        for (int i = 0; i < local.general.snake.count; i++) 
        {
            snake.transform.GetChild(i).position = snake.transform.GetChild(0).position + new Vector3(i, 0, 0);
            SnakeBody.Add(snake.transform.GetChild(i));
        }
        
    }
    
    public void Save()
    {
        throw new System.NotImplementedException();
    }

    public void Load()
    {
        throw new System.NotImplementedException();
    }
}
