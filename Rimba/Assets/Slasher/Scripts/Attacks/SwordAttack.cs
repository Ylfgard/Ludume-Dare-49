using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    private int damage = 20;
    public float cooldownTime;
    private bool isCooldown;
    private float time;
    
    public GameObject attacker;
    public GameObject slashEffect;

    // Start is called before the first frame update
    void Start()
    {
        var parent = transform.parent;
        attacker = parent.gameObject;
        transform.position = attacker.transform.position;
        slashEffect = Instantiate(slashEffect, parent.transform.position, parent.transform.rotation);
        slashEffect.transform.parent = null;
        slashEffect.transform.Rotate(0, 0, 90);
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
        Destroy(slashEffect.gameObject);
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
