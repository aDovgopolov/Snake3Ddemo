using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = System.Random;
using DG.Tweening;
using UnityEngine.Serialization;

public class Snake : MonoBehaviour
{    
    private SnakeLogic _snakeLogic;
    public static float lastMove = 1f;
    
    [FormerlySerializedAs("_snakePart")] [SerializeField]  
    private GameObject snakePart;
    
    private void Start()
    {    
        local.init("local");
        _snakeLogic = new SnakeLogic(local.general.snake.count);
        _snakeLogic.RegisterHandler(ChangePos, SnakeGrow);
        _snakeLogic.SetSnakeParts(this);
        
        SetGridInfo();
    }

    void Update()
    {
        if (Time.time - lastMove >= 1f )
        {    
            _snakeLogic.Move();
        }
    }
    
    public void ChangePos(Transform objectTransform, Vector3 newPos)
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(objectTransform.DOMove(newPos, 1));
        SetGridInfo();
    }
    
    private void SnakeGrow()
    {
        int index = transform.GetChildCount();
        Vector3 newSnakePartPos = transform.GetChild(index  - 1).transform.position;
        GameObject _snakePart = Instantiate(snakePart, newSnakePartPos, Quaternion.identity);
        _snakePart.transform.parent = transform;
        _snakeLogic.SnakeBody.Add(_snakePart.transform);
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
        _snakeLogic.CheckConnectionWithHead();
    }
}
