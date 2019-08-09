using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
[XmlRoot("Knight")]
public class Hero
{
    //[XmlElement("n")]
    public string name;

    public bool isBoss;

    //[XmlIgnore]
    public int hitPoints;

    public float baseDamage;
    //[XmlArray("rewards"), XmlArrayItem("reward")]
    public int[] comboRewards;

    public override string ToString()
    {
        return $"name: {name}, isBoss: {isBoss}, hitPoints: {hitPoints}, baseDamage: {baseDamage}, comboRewards: {comboRewards}"; ;
    }
}