using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHUD : HUD
{
    [SerializeField] private Transform target;
    private Vector3 offset;
    void Awake()
    {
        transform.parent = null;
        offset = new Vector3(0, 1.5f, 0);
    }
    void Update()
    {
        if (target != null)
        transform.position = target.position + offset;
    }
}
 