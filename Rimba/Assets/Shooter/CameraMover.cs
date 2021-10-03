using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] Transform target;

    Vector3 offset;
    void Start()
    {
        offset = new Vector3(0, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            transform.position = target.position + offset;
    }
}
