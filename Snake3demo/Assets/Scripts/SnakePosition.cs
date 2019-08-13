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

    public SnakePosition()
    {
    }

    public SnakePosition(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override string ToString()
    {
        return $"  x = {this.x}, y = {y}, z = {z}";
    }
}
