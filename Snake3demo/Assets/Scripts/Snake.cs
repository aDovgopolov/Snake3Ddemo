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
    private Vector3 oldPosition;
    
    [FormerlySerializedAs("_snakePart")] [SerializeField] 
    private GameObject snakePart;
    
    private void Start()
    {    
        local.init("local");
        snakeLogic = new SnakeLogic(local.general.snake.count);
        snakeLogic.RegisterHandler(ChangePos, SnakeGrow);
            
        GetComponent<Transform>().position = new Vector3(_x, _y, _z);
        
        // TODO can be out of range on Start
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
            snakeLogic.Move();
        }
    }
    
   
    public void ChangePos(Transform transform, Vector3 pos)
    {
        Debug.Log("ChangePos");
        Sequence mySequence = DOTween.Sequence();
        transform.DOScale(new Vector3(0.5f, 0.5f, 1), 0.5f).OnComplete(() => OnScaleRevert(transform));
        mySequence.Append(transform.DOMove(pos, 1));
    }
    
    private void OnScaleRevert(Transform transform)
    {
        transform.DOScale(new Vector3(1f, 1f, 1), 0.5f);
    }
    
    private void SnakeGrow()
    {
        int index = transform.GetChildCount();
        Vector3 newSnakePartPos = transform.GetChild(index  - 1).transform.position;
        GameObject _snakePart = Instantiate(snakePart, newSnakePartPos, Quaternion.identity) as GameObject;
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
    
    
    
    
    
    
    
    
    
    
    
    #region OldType 
    
    
    public void ChangePos(float oldX, float oldY, float oldZ, 
        float newX, float newY, float newZ)
    {
        Debug.Log("ChangePos");
        
    }

    
    
    private void move(Transform transform, Vector3 pos)
    {
        Sequence mySequence = DOTween.Sequence();
        transform.DOScale(new Vector3(0.5f, 0.5f, 1), 0.5f).OnComplete(() => OnScaleRevert(transform));
        mySequence.Append(transform.DOMove(pos, 1));
    }
    
    /*public void MovementToApple()
    {
        Debug.Log("MovementToApple");
        
        SetGridInfo();
        // neto
        Vector3 direction = _applePosotion;
        
        Vector3 oldPosition  = transform.GetChild(0).transform.position;
        
        
        Vector3 pos = transform.GetChild(0).transform.position += direction;
        move(transform.GetChild(0).transform , pos);
        
        LookForward(transform.GetChild(0).transform.position, direction);
        
        for (int i = 1; i < transform.GetChildCount(); i++)
        {
            move(transform.GetChild(i).transform, oldPosition);
            oldPosition = transform.GetChild(i).transform.position;
        }
        
        _lastMove = Time.time;
    }*/
    
    /*private void LookForward(Vector3 headPosition, Vector3 direction)
    {   
        int layerMask = 1 << 9;

        RaycastHit hit;
        Vector3 directionLine = headPosition + (direction * 15);
        
        if (Physics.Raycast(headPosition, direction, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(headPosition, direction , Color.yellow, 1f);
            Debug.DrawLine(headPosition, directionLine, Color.red, 1f);
            Debug.Log($"Did Hit {hit} and {hit.transform.position} and {hit.transform.gameObject.name}");
            _appleFound = true;
            _applePosotion = hit.transform.position;
            //Debug.Break();
        }
        else
        {
            Debug.DrawLine(headPosition, direction, Color.white, 1f);
            Debug.Log("Did not Hit");
        }
    }*/
    
    /*public void Movement()
    {
        SetGridInfo();
        
        
        Vector3 direction = _appleFound ? GetMoveDirectionToApple() :  CheckFreeSpaceExeptItself();
        
        Vector3 oldPosition  = transform.GetChild(0).transform.position;
        
        
        Vector3 pos = transform.GetChild(0).transform.position += direction;
        move(transform.GetChild(0).transform , pos);
        
        LookForward(transform.GetChild(0).transform.position, direction);
        
        for (int i = 1; i < transform.GetChildCount(); i++)
        {
            move(transform.GetChild(i).transform, oldPosition);
            oldPosition = transform.GetChild(i).transform.position;
        }
        
        lastMove = Time.time;
    }
    
    private Vector3 GetMoveDirectionToApple()
    {
        Debug.Log("GetMoveDirectionToApple");
        
        Vector3 value = _applePosotion - gameObject.transform.GetChild(0).position;
        int tmp;
//        Debug.Log("value = " + value);
        
        if (value.x != 0.0f)
        {
            tmp = (int)Mathf.Abs(value.x / value.x);
            if (value.x < 0)
                tmp = tmp * (-1);
            Debug.Log(tmp);
            directionToApple = new Vector3(tmp, value.y, value.z);
        }
        else if (value.y != 0.0f)
        {
            tmp = (int)Mathf.Abs(value.y / value.y);
            if (value.y < 0)
                tmp = tmp * (-1);
            Debug.Log(tmp);
            directionToApple = new Vector3(value.x , tmp, value.z);
        }
        else if(value.z != 0.0f)
        {
            tmp = (int)Mathf.Abs(value.z / value.z);
            if (value.z < 0)
                tmp = tmp * (-1);
            Debug.Log(tmp);
            directionToApple = new Vector3(value.x, value.y, tmp);
            
        }
//        Debug.Log("direction = " +  directionToApple);
        //Debug.Break();
        
        return directionToApple;
    }
    
    private Vector3 CheckFreeSpaceExeptItself()
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
    }*/

    #endregion
}
