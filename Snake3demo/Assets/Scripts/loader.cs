using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class loader : MonoBehaviour
{
    public SnakePosition snakePosition;

    private void Awake()
    {
        snakePosition = Deserialize<SnakePosition>("Assets\\Resources\\riddle2.xml");
    }
    
    public static T Deserialize<T>(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StreamReader reader = new StreamReader(path);
        T deserialized = (T)serializer.Deserialize(reader.BaseStream);
        reader.Close();
        return deserialized;
        /*try
        {
           
        }
        catch (Exception e) {
        UnityEngine.Debug.LogError("Exception loading question data: " + e);
        return null;
        }*/
        
        /*TextAsset xmlData = Resources.Load("riddle") as TextAsset;
        Debug.Log(xmlData);
        using (XmlReader reader = XmlReader.Create(new StringReader(xmlData.text)))
        {
            while (reader.Read())
            {
                Debug.Log(reader.Name);
            }
        }*/
    }
}
