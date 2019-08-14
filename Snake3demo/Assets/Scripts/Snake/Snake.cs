using UnityEngine;
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
        _snakeLogic = new SnakeLogic(local.general.snake.count, this);
        _snakeLogic.RegisterHandler(ChangePos, SnakeGrow, DrawSnake);
        
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

    private void DrawSnake()
    {
        
    }
    
    private void SnakeGrow()
    {
        Vector3 newSnakePartPos = transform.GetChild(_snakeLogic.GetSnakeSize()  - 1).transform.position;
        GameObject _snakePart = Instantiate(snakePart, newSnakePartPos, Quaternion.identity);
        _snakePart.transform.parent = transform;
        _snakeLogic.SnakeBody.Add(_snakePart.transform);
    }
    
    private void SetGridInfo()
    {
        if (Grid.IsValidGridPos3D(this))
        {
            Grid.UpdateGrid3D(this);
        }
        else
        {
            Debug.Log("Debug SetGridInfo");
            Debug.Break();
        }
    }
    
    public void CheckConnectionWithHead()
    {
        _snakeLogic.CheckConnectionWithHead();
    }
}
