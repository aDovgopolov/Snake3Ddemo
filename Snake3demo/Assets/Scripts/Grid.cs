using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static int x = 15;
    public static int y = 15;
    public static int z = 15;
    public static Transform[,,] grid3D = new Transform[x, y, z];
    
    public static Vector3 RoundVec3(Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x),
            Mathf.Round(v.y),
            Mathf.Round(v.z));
    }
    
    public static bool InsideBorder3D(Vector3 pos)
    {
        bool inX = (int) pos.x >= 0 && (int) pos.x < x;
        bool inY = (int) pos.y >= 0 && (int) pos.y < y;
        bool inZ = (int) pos.z >= 0 && (int) pos.z < z;
        return ( inX && inY && inZ );
    }
    
    public static void UpdateGrid3D(Snake group)
    {
        for (int y = 0; y < Grid.y; ++y)
            for (int x = 0; x < Grid.x; ++x)
                for (int z = 0; z < Grid.x; ++z)
                    if (grid3D[x, y, z] != null)
                        if (grid3D[x, y, z].parent == group.gameObject.transform)
                            grid3D[x, y, z] = null;

        foreach (Transform child in group.gameObject.transform)
        {
            Vector3 v = RoundVec3(child.position);
            grid3D[(int)v.x, (int)v.y, (int)v.z] = child;
        }
    }
    
    public static bool IsValidGridPos3D(Snake sname)
    {
        foreach (Transform child in sname.gameObject.transform)
        {
            Vector3 v = RoundVec3(child.position);

            if (!InsideBorder3D(v))
                return false;

            if (grid3D[(int)v.x, (int)v.y, (int)v.y] != null &&
                grid3D[(int)v.x, (int)v.y, (int)v.y].parent != sname.gameObject.transform)
                return false;
        }
        return true;
    }
    
    public static bool GetTransformOnPoint(Vector3 vec)
    {
        //Debug.Log($"x = {(int)vec.x} - y = {(int)vec.y} - z = {(int)vec.z}");
        
        // out of range
        bool xOutOfRange = (int) vec.x >= 15 || (int) vec.x < 0;
        bool yOutOfRange = (int) vec.y >= 15 || (int) vec.y < 0;
        bool zOutOfRange = (int) vec.z >= 15 || (int) vec.z < 0;
        
        if (xOutOfRange || yOutOfRange || zOutOfRange)
        {
            return true;
        }
        
        Transform transform = grid3D[(int)vec.x, (int)vec.y, (int)vec.z];
        Debug.Log($"transform = {transform}");
        bool isSomething = transform != null;
        return isSomething;
    }
    
}
