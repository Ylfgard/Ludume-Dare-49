using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    private int damage = 20;
    public GameObject attacker;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        attacker = gameObject.transform.parent.gameObject;
        transform.position = attacker.transform.position;
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = attacker.transform.position;
        transform.Rotate(Vector3.back * (Time.deltaTime * 720));
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( attacker.CompareTag("Enemy") && other.gameObject.CompareTag("Player")  )
        {
            other.gameObject.GetComponent<PlayerController>().UpdateHP(-damage);
        }
        
        else if ( attacker.CompareTag("Player") && other.gameObject.CompareTag("Enemy")  )
        {
            other.gameObject.GetComponent<EnemyController>().UpdateHP(-damage);
        }
    }
}
