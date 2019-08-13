using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = System.Random;
using DG.Tweening;
using UnityEngine.Serialization;

public class Snake : MonoBehaviour
{    
    private SnakeLogic snakeLogic;
    
    public static float _lastMove = 1f;
    private int _x = 5;
    private int _y = 5;
    private int _z = 8;
    
    [FormerlySerializedAs("_snakePart")] [SerializeField] 
    private GameObject snakePart;
    
    private void Start()
    {    
        local.init("local");
        snakeLogic = new SnakeLogic(local.general.snake.count);
        snakeLogic.RegisterHandler(ChangePos, SnakeGrow);
            
        GetComponent<Transform>().position = new Vector3(_x, _y, _z);
        
        for (int i = 0; i < local.general.snake.count; i++)
        {
            snakeLogic.SnakeBody.Add(transform.GetChild(i));
        }
        
        Grid.UpdateGrid3D(this);
    }

    void Update()
    {
        if (Time.time - _lastMove >= 1f )
        {    
            SetGridInfo();
            Debug.Log("snakeLogic.Move()");
            snakeLogic.Move();
        }
    }
    
    public void ChangePos(Transform objectTransform, Vector3 pos)
    {
        Sequence mySequence = DOTween.Sequence();
        transform.DOScale(new Vector3(0.5f, 0.5f, 1), 0.5f).OnComplete(() => OnScaleRevert(transform));
        
        Debug.Log($"ChangePos {objectTransform.position} + {pos}");
        mySequence.Append(objectTransform.DOMove(pos, 1));
        //Debug.Break();
    }
    
    private void OnScaleRevert(Transform transform)
    {
        transform.DOScale(new Vector3(1f, 1f, 1), 0.5f);
    }
    
    private void SnakeGrow()
    {
        int index = transform.GetChildCount();
        Vector3 newSnakePartPos = transform.GetChild(index  - 1).transform.position;
        GameObject _snakePart = Instantiate(snakePart, newSnakePartPos, Quaternion.identity);
        _snakePart.transform.parent = transform;
        snakeLogic.SnakeBody.Add(_snakePart.transform);
    }
    
    public void SetGridInfo()
    {
        if (Grid.IsValidGridPos3D(this))
        {
            Grid.UpdateGrid3D(this);
        }
    }


    public void CheckConnectionWithHead()
    {
        snakeLogic.CheckConnectionWithHead();
    }
    
}
