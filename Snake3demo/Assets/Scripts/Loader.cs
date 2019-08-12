using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Loader : MonoBehaviour
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
    }
}
