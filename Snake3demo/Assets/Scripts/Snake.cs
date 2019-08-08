using UnityEngine;
using Random = System.Random;

public class Snake : MonoBehaviour
{
    private float lastMove = 1;

    private Vector3[] directions = new Vector3[]
          {
              new Vector3(0, -1, 0), 
              new Vector3(0, 1, 0), 
              new Vector3(-1, 0, 0), 
              new Vector3(1, 0, 0), 
              new Vector3(0, 0, 1)
              
          };

    void Update()
    {
        if (Time.time - lastMove >= 1)
        {
            Movement();
        }
    }

    public void Movement()
    {
        if (Grid.IsValidGridPos3D(this))
        {
            Grid.UpdateGrid3D(this);
            //Debug.Log("IsValidGridPos3D");
        }
        
        Vector3 direction = CheckFreeSpaceExeptItself();
        
        Vector3 oldPosition  = transform.GetChild(0).transform.position;
        transform.GetChild(0).transform.position += direction;
        
        
        for (int i = 1; i < transform.GetChildCount(); i++)
        {
            Transform childPos = transform.GetChild(i).transform;
            Vector3 childOldPos = childPos.position;
            childPos.position = oldPosition;
            oldPosition = childOldPos;
        }
        
        
        
        /*if (Grid.IsValidGridPos3D(this))
        {
            Grid.UpdateGrid3D(this);
            Debug.Log("IsValidGridPos3D");
        }
        else
        {
            transform.position = oldPosition;
            Debug.Log("NO IsValidGridPos3D");
            // transform.position += new Vector3(1, 0, 0);
        }*/
        
        lastMove = Time.time;
    }


    public Vector3 CheckFreeSpaceExeptItself()
    {
        Vector3 direction = gameObject.transform.GetChild(0).position;
        
        Random random = new Random();
        Vector3 randomDirectionToMove = directions[random.Next(0, directions.Length) ];
        
        bool isFreeSpace = false;
        direction += randomDirectionToMove;
        
        //Grid.GetTransformOnPoint(direction);
        
        while (Grid.GetTransformOnPoint(direction))
        {
            Debug.Log(direction);
            direction = gameObject.transform.GetChild(0).position;
            randomDirectionToMove = directions[random.Next(0, directions.Length)];
            direction += randomDirectionToMove;
        }
        

        /*while (!isFreeSpace)
        {
            direction += randomDirectionToMove;

            //костыль какой-то
            foreach (Transform child in gameObject.transform)
            {
                Debug.Log($"direction = {direction}  - child.position = {child.position}");
                if (direction != child.position)
                {

                    Debug.Log($"before break");
                    isFreeSpace = true;
                    break;
                }

                Debug.Log($"after break");
                direction = gameObject.transform.GetChild(0).position;
                randomDirectionToMove = directions[random.Next(0, directions.Length)];
                //direction += randomDirectionToMove;

            }
        }*/

        return randomDirectionToMove;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        MeshRenderer meshRend = GetComponent<MeshRenderer>();
        meshRend.material.color = Color.green;
        Debug.Log(other.name);
    }

    public void Brak()
    { 
        /*foreach (Transform child in transform)
        {    
            child.position += direction;
            oldPosition = child.position;
            
        }*/
        
        
        /*if (oldPosition != transform.position)
        {
            MoveTail(oldPosition);
        }*/
        
        
        
        /*while (!isFreeSpace)
        {
            direction += randomDirectionToMove;
            
            //костыль какой-то
            foreach (Transform child in gameObject.transform)
            {
                Debug.Log($"direction = {direction}  - child.position = {child.position}");
                if (direction != child.position)
                {
                    isFreeSpace = true;
                    
                    Debug.Log($"before break");
                    break;
                }
                
                Debug.Log($"after break");
                direction = gameObject.transform.GetChild(0).position;
                randomDirectionToMove = directions[random.Next(0, directions.Length) ];
                direction += randomDirectionToMove;
            }
        }*/
    }
}
