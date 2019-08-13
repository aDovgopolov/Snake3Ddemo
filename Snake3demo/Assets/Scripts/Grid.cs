using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
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
    
    public static void UpdateGrid3D(Snake snake)
    {
        for (int y = 0; y < Grid.y; ++y)
            for (int x = 0; x < Grid.x; ++x)
                for (int z = 0; z < Grid.x; ++z)
                    if (grid3D[x, y, z] != null)
                        if (grid3D[x, y, z].parent == snake.gameObject.transform)
                            grid3D[x, y, z] = null;

        foreach (Transform child in snake.gameObject.transform)
        {
            if (child.gameObject.active)
            {
                Vector3 v = RoundVec3(child.position);
                grid3D[(int)v.x, (int)v.y, (int)v.z] = child;
                //Debug.Log(child.gameObject.name);
            }
        }
    }

    public static Transform SearchForApple()
    {
        for (int y = 0; y < Grid.y; ++y)
        {
            for (int x = 0; x < Grid.x; ++x)
            {
                for (int z = 0; z < Grid.x; ++z)
                {
                    if (grid3D[y, x, z] != null && grid3D[y, x, z].gameObject.tag.Equals("Food"))
                    {
                        Debug.Log($"grid3D[{y}][{x}][{z}] = {grid3D[y, x, z]} + {grid3D[y, x, z].gameObject.name}" );
                        return grid3D[y, x, z];
                    }
                }
            }
        }

        return null;
    }
    
    public static bool IsValidGridPos3D(Snake snake)
    {
        foreach (Transform child in snake.gameObject.transform)
        { 
            if(!child.gameObject.active) continue;
            
            Vector3 v = RoundVec3(child.position);

            if (!InsideBorder3D(v))
                return false;

            if (grid3D[(int)v.x, (int)v.y, (int)v.y] != null &&
                grid3D[(int)v.x, (int)v.y, (int)v.y].parent != snake.gameObject.transform)
                return false;
        }
        return true;
    }
    
    public static bool GetTransformOnPoint(Vector3 vec)
    {
        if (!InsideBorder3D(vec))
            return false;
        
        Transform transform = grid3D[(int)vec.x, (int)vec.y, (int)vec.z];
        bool isSomething = transform != null;
        return isSomething;
    }

    public static Transform GetAppleByVector3(Vector3 position)
    {
        Transform transform = grid3D[(int)position.y, (int)position.x, (int)position.z];
        Debug.Log($"position = {position} - transform = { transform}");
        return transform;
    }
}
