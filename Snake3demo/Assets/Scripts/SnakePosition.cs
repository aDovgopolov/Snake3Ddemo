using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
[XmlRoot("SnakePosition")]
public class SnakePosition
{
    public int x;

    public int y;

    public int z;
    

    public override string ToString()
    {
        Debug.Log("toString'");
        Debug.Log("second Commit'");
        return $"  x = {this.x}, y = {y}, z = {z}";
    }
}
