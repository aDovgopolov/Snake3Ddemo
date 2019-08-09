using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = System.Random;
using DG.Tweening;

public class Snake : MonoBehaviour
{    
    private float lastMove = 1f;
    private int x = 0;
    private int y = 0;
    private int z = 0;
    private Vector3 oldPosition;
    private Vector3 newPos;
    
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
        //local.init("local");
        loader loader = gameObject.AddComponent<loader>();
        GetComponent<Transform>().position = new Vector3(x, y, z);
        
        // TODO can be out of range on Start
        Grid.UpdateGrid3D(this);
    }

    void Update()
    {
        if (Time.time - lastMove >= 1f)
        {
            Movement();
        }
    }
    
    public void Movement()
    {
        SetGridInfo();
        
        Vector3 direction = CheckFreeSpaceExeptItself();
        
        Vector3 oldPosition  = transform.GetChild(0).transform.position;
        
        Vector3 pos = transform.GetChild(0).transform.position += direction;
        move(transform.GetChild(0).transform , pos);
        
        for (int i = 1; i < transform.GetChildCount(); i++)
        {
           // Vector3 childOldPos  = transform.GetChild(i).transform.position;
            move(transform.GetChild(i).transform, oldPosition);
            oldPosition = transform.GetChild(i).transform.position;
           // oldPosition = childOldPos;
        }
        
        lastMove = Time.time;
    }

    private void move(Transform transform, Vector3 pos)
    {
        Sequence mySequence = DOTween.Sequence();
        transform.DOScale(new Vector3(0.5f, 0.5f, 1), 0.5f).OnComplete(() => OnScaleRevert(transform));
        mySequence.Append(transform.DOMove(pos, 1));
    }
    
    public void Movementv1()
    {
        SetGridInfo();
        
        Vector3 direction = CheckFreeSpaceExeptItself();
        
        Vector3 oldPosition  = transform.GetChild(0).transform.position;
        
        Vector3 pos = transform.GetChild(0).transform.position + direction;
        Move(transform.GetChild(0).transform , pos);
        
        for (int i = 1; i < transform.GetChildCount(); i++)
        {
            Vector3 childOldPos = transform.GetChild(i).transform.position;
            Move(transform.GetChild(i).transform, oldPosition);
            oldPosition = childOldPos;
        }
        
        lastMove = Time.time;
    }
    
    public void MovementV2()
    {
        SetGridInfo();
        
        Vector3 direction = CheckFreeSpaceExeptItself();
        
        //oldPosition  = transform.GetChild(0).transform.position;
            
        
        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            newPos = transform.GetChild(i).transform.position + direction;
            Move(transform.GetChild(i).transform, newPos);
        }
        
        lastMove = Time.time;
    }
    
    private void Move(Transform transform, Vector3 pos)
    {
        Sequence mySequence = DOTween.Sequence();
        
        Vector3 childOldPos = transform.position;
        transform.DOScale(new Vector3(0.5f, 0.5f, 1), 0.5f).SetDelay(0.25f).OnComplete(() => OnScaleRevert(transform));
        mySequence.Append(transform.DOMove(pos, lastMove).SetDelay(0.25f));
        //transform.DOMove(pos, lastMove);
        
        newPos = childOldPos;
    }

    private void OnScaleRevert(Transform transform)
    {
        transform.DOScale(new Vector3(1f, 1f, 1), 0.5f);
    }
    
    public Vector3 CheckFreeSpaceExeptItself()
    {
        Vector3 direction = gameObject.transform.GetChild(0).position;
        
        Random random = new Random();
        Vector3 randomDirectionToMove = directions[random.Next(0, directions.Length) ];
        
        direction += randomDirectionToMove;
        
        int count = 10;
        while (Grid.GetTransformOnPoint(direction))
        {    
            direction = gameObject.transform.GetChild(0).position;
            randomDirectionToMove = directions[random.Next(0, directions.Length)];
            direction += randomDirectionToMove;
            count--;
            
            if (count < 0)
            {
                Debug.LogError("CheckFreeSpaceExeptItself: no free space");
                break;
            }
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
