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
        plrPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>() as Transform;
        _transform = gameObject.GetComponent<Transform>();
        up = _transform.position.y + Slide_on;
        down = _transform.position.y - Slide_on;
    }
    
    void FixedUpdate()
    {
        if(plrPos.position.y < _transform.position.y)
        {
            Vector3 newPos = _transform.position;
            newPos.y = plrPos.position.y - Slide_on;
            up-=Slide_on;
            down -= Slide_on;
            _transform.position = newPos;
        }
        
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
