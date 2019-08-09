using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = System.Random;
using DG.Tweening;

public class Snake : MonoBehaviour
{    
    private float lastMove = 1;
    private int x = 0;
    private int y = 0;
    private int z = 0;
    
    private Vector3[] directions = new Vector3[]
          {
              new Vector3(0, -1, 0), 
              new Vector3(0, 1, 0), 
              new Vector3(-1, 0, 0), 
              new Vector3(1, 0, 0), 
              new Vector3(0, 0, 1)
          };

    private void Awake()
    {
        
        Random random = new Random();
        x = random.Next(0, 15);
        y = random.Next(0, 15);
        z = random.Next(0, 15);
    }

    private void Start()
    {
        loader loader = gameObject.AddComponent<loader>();
        Debug.Log($"{loader.snakePosition.x} - {loader.snakePosition.y} - {loader.snakePosition.z}");
        local.init("local");
        GetComponent<Transform>().position = new Vector3(x, y, z);
    }

    void Update()
    {
        if (Time.time - lastMove >= 1)
        {
            Movement();
        }
    }

    public void Movement()
    {
        SetGridInfo();
        
        Vector3 direction = CheckFreeSpaceExeptItself();
        
        Vector3 oldPosition  = transform.GetChild(0).transform.position;
        
        //transform.GetChild(0).transform.position += direction;
        Vector3 pos = transform.GetChild(0).transform.position += direction;
        //transform.GetChild(0).transform.DOMove(pos, 1).SetEase(Ease.InBounce);
        move(transform.GetChild(0).transform , pos);
        
        for (int i = 1; i < transform.GetChildCount(); i++)
        {
            //Transform childPos = transform.GetChild(i).transform;
            Vector3 childOldPos = transform.GetChild(i).transform.position;
            //childPos.position = oldPosition;
            move(transform.GetChild(i).transform, oldPosition);
            //childPos.DOMove(oldPosition, 1);
            oldPosition = childOldPos;
        }
        
        lastMove = Time.time;
    }

    private void move(Transform transform, Vector3 pos)
    {
        transform.DOMove(pos, 1).SetEase(Ease.InBounce);
    }
    
    public Vector3 CheckFreeSpaceExeptItself()
    {
        Vector3 direction = gameObject.transform.GetChild(0).position;
        
        Random random = new Random();
        Vector3 randomDirectionToMove = directions[random.Next(0, directions.Length) ];
        
        bool isFreeSpace = false;
        direction += randomDirectionToMove;
        
        while (Grid.GetTransformOnPoint(direction))
        {
            direction = gameObject.transform.GetChild(0).position;
            randomDirectionToMove = directions[random.Next(0, directions.Length)];
            direction += randomDirectionToMove;
        }

        return randomDirectionToMove;
    }

    public void SetGridInfo()
    {
        if (Grid.IsValidGridPos3D(this))
        {
            Grid.UpdateGrid3D(this);
        }
    }
    
}
