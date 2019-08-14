using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class SnakeData : IDataManager
{    
    #region Values
    public int SnakeSize { get; set; }
    public bool AppleFound { get; set; }
    public Vector3 ApplePosition { get; set; }
    [SerializeField ]
    private Vector3 _applePosition;
    [SerializeField ]private int _xHead = 14;
    [SerializeField ]private int _yHead = 14;
    [SerializeField ]private int _zHead = 14;
    
    [XmlIgnore]
    public List<Transform> snakeBody = new List<Transform>();
    public List<Vector3> xmlSnake  = new List<Vector3>();
    public List<Vector3> pathToApple = new List<Vector3>();
     
    #endregion

    public SnakeData()
    {
    }

    public SnakeData(int snakeSize, Snake snake)
    {
        SnakeSize = snakeSize;
        SetSnakeParts(snake);
    }
    
    public void setSnakeUI(Snake snake)
    {
        SetSnakeParts(snake);
        
        Debug.Log($"{snakeBody.Count} -  {SnakeSize}");
    }

    /*private void SetSnakeParts(Snake snake)
    {
        snake.transform.GetChild(0).position = new Vector3(_xHead, _yHead, _zHead);
        
        for (int i = 0; i < SnakeSize; i++) 
        {
            snake.transform.GetChild(i).position = snake.transform.GetChild(0).position + new Vector3(i, 0, 0);
            snakeBody.Add(snake.transform.GetChild(i));
        }
    }*/
    
    private void SetSnakeParts(Snake snake)
    {
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
        for (int i = 0; i < snakeBody.Count; i++)
        {
            xmlSnake.Add(snakeBody[i].position);
        }
        
        for (int i = 0; i < snakeBody.Count; i++)
        {
            Debug.Log(xmlSnake[i]);
        }
        
        Serialize(this, "Assets\\Resources\\SnakeData.xml");
    }
    
    public static void Serialize(object item, string path)
    {
        XmlSerializer serializer = new XmlSerializer(item.GetType());
        StreamWriter writer = new StreamWriter(path);
        serializer.Serialize(writer.BaseStream, item);
        writer.Close();
    }
    
    public void Load()
    {
         
    }
    
   
}
