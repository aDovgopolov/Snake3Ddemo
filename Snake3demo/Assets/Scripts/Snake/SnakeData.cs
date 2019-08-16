using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class SnakeData
{    
    #region Values
    public int SnakeSize { get; set; }
    public bool AppleFound { get; set; }
    public Vector3 ApplePosition { get; set; }
    [SerializeField ]
    private Vector3 _applePosition;
    [SerializeField ]private int _xHead = 2;
    [SerializeField ]private int _yHead = 2;
    [SerializeField ]private int _zHead = 2;
    
    [XmlIgnore]
    public List<Transform> snakeBody = new List<Transform>();
    public List<Vector3> xmlSnake  = new List<Vector3>();
    [XmlIgnore]
    public List<Vector3> pathToApple = new List<Vector3>();

    private Snake snake;
    private SnakeLoader dataManager;
    #endregion

    public SnakeData()
    {
    }

    public SnakeData(int snakeSize, Snake _snake)
    {
        snake = _snake;
        dataManager = GameObject.Find("DataManager").GetComponent<SnakeLoader>(); 
        dataManager.AddSnakeToList(_snake, this);
        SnakeSize = snakeSize;
        //SetSnakePartsDefault(snake);
       SetSnakeParts(_snake);
    }
    
    public void setSnakeUI(Snake snake)
    {
        SetSnakeParts(snake);
        
        Debug.Log($"{snakeBody.Count} -  {SnakeSize}");
    }

    private void SetSnakePartsDefault(Snake snake)
    {
        Debug.Log("SetSnakePartsDefault(Snake snake)");
        snake.transform.GetChild(0).position = new Vector3(_xHead, _yHead, _zHead);
        
        for (int i = 0; i < SnakeSize; i++) 
        {
            snake.transform.GetChild(i).position = snake.transform.GetChild(0).position + new Vector3(i, 0, 0);
            snakeBody.Add(snake.transform.GetChild(i));
        }
    }
    
    private void SetSnakeParts(Snake snake)
    {
        if (xmlSnake.Count == 0)
        {
            SetSnakePartsDefault(snake);
            return;
        }
        
        Debug.Log("  private void SetSnakeParts(Snake snake)");
        snake.transform.GetChild(0).position = xmlSnake[0];
        
        for (int i = 0; i < SnakeSize; i++)
        {
            Transform transform = snake.transform.GetChild(i);
            transform.position = xmlSnake[i];
            snakeBody.Add(transform);
        }
    }
    
    public void Save()
    {
        xmlSnake.Clear();
        for (int i = 0; i < snakeBody.Count; i++)
        {
            xmlSnake.Add(snakeBody[i].position);
        }
        
        for (int i = 0; i < snakeBody.Count; i++)
        {
            Debug.Log(xmlSnake[i]);
        }
        
        //Serialize(this, "Assets\\Resources\\SnakeData.xml");
    }

    public void SetVectors(List<Vector3> SNAKKE)
    {
        snake.transform.GetChild(0).position = SNAKKE[0];
        
        for (int i = 0; i < SnakeSize; i++)
        {
            Transform transform = snake.transform.GetChild(i);
            transform.position = SNAKKE[i];
            snakeBody.Add(transform);
        }
    }
    
    public static void Serialize(object item, string path)
    {
        XmlSerializer serializer = new XmlSerializer(item.GetType());
        StreamWriter writer = new StreamWriter(path);
        serializer.Serialize(writer.BaseStream, item);
        writer.Close();
    }
    
    
   
}
