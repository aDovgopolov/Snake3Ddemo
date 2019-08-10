using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRay : MonoBehaviour
{
    RaycastHit hit;
    Ray ray;
    bool wRay;
    
    
    void Update()
    {
        //wRay = Physics.Raycast(transform.position, Vector3.left, out hit);
        
        //Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position,
            Vector3.forward, Mathf.Infinity))
        {
            Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z - 10000000), Color.yellow, 5f);
//            print("There is something in front of the object!");
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.forward, Color.red, 5f);
            Debug.DrawLine(transform.position, new Vector3(transform.position.x , transform.position.y, transform.position.z  - 10000000), Color.yellow, 5f);
            print("There is NOTHING in front of the object!");
        }
        
    }

    public void method2()
    {
        
        ray.origin = transform.position;
        ray.direction = Vector3.forward;
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
    }
}
