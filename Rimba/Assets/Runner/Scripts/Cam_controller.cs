using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_controller : MonoBehaviour
{
    public static float up ,down;
    Transform plrPos,_transform;
    public float Slide_on;


    void Awake()
    {
        up = 4.5f;
        down = -4.5f;
        plrPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>() as Transform;
        _transform = gameObject.GetComponent<Transform>();
    }
    
    void FixedUpdate()
    {
            if(plrPos.position.y > up && _transform.position.y < 0){
                _transform.position =new  Vector3(_transform.position.x,_transform.position.y + Slide_on, _transform.position.z);
                up += Slide_on;
                down += Slide_on;
            }

            if(plrPos.position.y < down){
                _transform.position =new  Vector3(_transform.position.x,_transform.position.y - Slide_on , _transform.position.z);
                up -= Slide_on;
                down -= Slide_on;
            }
    }   
}
