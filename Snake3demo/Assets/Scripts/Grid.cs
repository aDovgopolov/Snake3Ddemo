using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static int x = 15;
    public static int y = 15;
    public static int z = 15;
    public static Transform[,] grid = new Transform[x, y];
    public static Transform[,,] grid3D = new Transform[x, y, z];

    public static Vector2 RoundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x),
            Mathf.Round(v.y));
    }
    
    public static bool InsideBorder(Vector2 pos)
    {
        return ( (int)pos.x >= 0 && (int)pos.x < x && (int)pos.y >= 0 );
    }
    
    public static void UpdateGrid(Snake group)
    {
        for (int y = 0; y < Grid.y; ++y)
        for (int x = 0; x < Grid.x; ++x)
            if (Grid.grid[x, y] != null)
                if (Grid.grid[x, y].parent == group.gameObject.transform)
                    Grid.grid[x, y] = null;

        foreach (Transform child in group.gameObject.transform)
        {
            Vector2 v = Grid.RoundVec2(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child;
        }
    }
    
    public static bool IsValidGridPos(Snake sname)
    {
        foreach (Transform child in sname.gameObject.transform)
        {
            Vector2 v = Grid.RoundVec2(child.position);

            if (!Grid.InsideBorder(v))
                return false;

            if (Grid.grid[(int)v.x, (int)v.y] != null &&
                Grid.grid[(int)v.x, (int)v.y].parent != sname.gameObject.transform)
                return false;
        }
        return true;
    }
    
    //----------------------------------------------------------------//
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
                    if (Grid.grid3D[x, y, z] != null)
                        if (Grid.grid3D[x, y, z].parent == group.gameObject.transform)
                            Grid.grid3D[x, y, z] = null;

        foreach (Transform child in group.gameObject.transform)
        {
            Vector3 v = Grid.RoundVec3(child.position);
            Grid.grid3D[(int)v.x, (int)v.y, (int)v.z] = child;
        }
    }
    
    public static bool IsValidGridPos3D(Snake sname)
    {
        foreach (Transform child in sname.gameObject.transform)
        {
            Vector3 v = Grid.RoundVec3(child.position);

            if (!Grid.InsideBorder3D(v))
                return false;

            if (Grid.grid3D[(int)v.x, (int)v.y, (int)v.y] != null &&
                Grid.grid3D[(int)v.x, (int)v.y, (int)v.y].parent != sname.gameObject.transform)
                return false;
        }
        return true;
    }
    
    
    //public static 
}
