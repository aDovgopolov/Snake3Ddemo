using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SnakeLoader: MonoBehaviour
{
    AllData alldata = new AllData();
    private List<Snake> snakeList = new List<Snake>();
    private List<SnakeData> snakePosList = new List<SnakeData>();

    public void AddSnakeToList(Snake snake, SnakeData snakeData)
    {
        snakeList.Add(snake);
        snakePosList.Add(snakeData);
    }

    public void RemoveSnakeFromList(Snake snake)
    {
        snakeList.Remove(snake);
    }
    
    private void Start()
    {    
        snakeList.Clear();
        snakePosList.Clear();
        
        Load();
    }

    public void Save()
    {
        string writer = "Assets\\Resources\\snakes.xml";
        
        FileStream fileStream = File.Open(writer, FileMode.Open);
        fileStream.SetLength(0);
        fileStream.Close();
        
        XmlSerializer serializer = new XmlSerializer(typeof(AllData));
        FileStream fs = new FileStream(writer, FileMode.OpenOrCreate);
        
       //snakePosList.Clear();
        
        /*foreach (var snake in snakeList)
        {
            snakePosList.Add(snake.);
        }*/
        
        
        alldata.snakeData = snakePosList;
        
        //---/////
        var class1 = alldata;
        var outputString = JsonUtility.ToJson(class1);
        File.WriteAllText("Assets\\Resources\\peoplesJSON.json", outputString);
        //---/////
        
        serializer.Serialize(fs, alldata);
        Debug.Log("Save");
    }
    
    public void Load()
    {
        XmlSerializer formatter = new XmlSerializer(typeof(AllData));
        
        using (FileStream fs = new FileStream("Assets\\Resources\\snakes.xml", FileMode.OpenOrCreate))
        {
            alldata = (AllData)formatter.Deserialize(fs);
            
            /*foreach (SnakeData p in allSnakes.snakeData)
            {
                snakePosList.Add(p);
                Debug.Log(p.ToString());
            }*/
        }
       // SetSnakes();
        StartCoroutine(WaitFor1Second());
    }

    public void SetSnakes()
    {
        
        snakePosList[0].xmlSnake = alldata.snakeData[0].xmlSnake;
        snakeList[0].dataLoaded = true;
        /*for (int i = 0; i < snakePosList.Count; i++)
        {
            snakePosList[i] = alldata.snakeData[i];
            for (int j = 0; j < snakePosList[i].snakeBody.Count; j++)
            {
                /*
                Vector3 vector3 = new Vector3(snakePosList[i].xmlSnake[i].x, snakePosList[i].xmlSnake[i].y, snakePosList[i].xmlSnake[i].z);
                snakeList[i].transform.position = vector3;#1#
            }
            snakeList[i].dataLoaded = true;
        }*/
    }
    
    IEnumerator WaitFor1Second()
    {
        yield return  new WaitForSeconds(0.1f);
        
        snakePosList[0].xmlSnake = alldata.snakeData[0].xmlSnake;
        for (int i = 0; i < alldata.snakeData.Count; i++)
        {
            for (int j = 0; j < alldata.snakeData[i].xmlSnake.Count; j++)
            {
                Vector3 vec = alldata.snakeData[i].xmlSnake[j];
                Debug.Log($" {vec.x}+ {vec.y} + {vec.x}");
            }
        }
        snakeList[0].dataLoaded = true;
        
        snakePosList[0].SetVectors(alldata.snakeData[0].xmlSnake);
       //GameObject.Find("TrueSnake").GetComponent<SnakeData>().SetVectors();
        /*for (int i = 0; i < snakePosList.Count; i++)
        {
            for (int j = 0; j < snakePosList[i].snakeBody.Count; j++)
            {
                /*
                Vector3 vector3 = new Vector3(snakePosList[i].xmlSnake[i].x, snakePosList[i].xmlSnake[i].y, snakePosList[i].xmlSnake[i].z);
                snakeList[i].transform.position = vector3;#1#
            }
            snakeList[i].dataLoaded = true;
        }*/
    }
    
    void OnApplicationQuit()
    {
        snakePosList[0].Save();
        Save();
    }


    #region Old

    /*void Method()
    {
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
    }*/

    #endregion
}