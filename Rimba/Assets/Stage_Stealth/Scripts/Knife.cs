using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private Hero hero;
    [SerializeField] private float range;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    private Vector2 startPos, currentPos, endPos;

    private void Start()
    {
        startPos = transform.position;
    }
}
