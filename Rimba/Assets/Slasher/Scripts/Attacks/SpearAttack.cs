using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAttack : MonoBehaviour
{
    private int damage = 50;
    public GameObject attacker;
    private float time;
    private float speed = 15.0f;
    private float timeOfAttack = 0.2f;

    private Vector2 target;
    
    // Start is called before the first frame update
    void Start()
    {
        attacker = gameObject.transform.parent.gameObject;
        transform.position = attacker.transform.position;

        if (attacker.CompareTag("Player"))
        {
            target = attacker.GetComponent<PlayerController>().mouseRel;
        }

        if (attacker.CompareTag("Enemy"))
        {
            target = GameObject.Find("Player").transform.position - gameObject.transform.position;
        }

        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.parent = null;
        
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * (speed * Time.deltaTime));
    }
    
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(timeOfAttack);
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
        
        else if ( attacker.CompareTag("Player") && other.gameObject.CompareTag("Boss") )
        {
            other.gameObject.GetComponent<DaVinchiMechController>().UpdateHP(-damage);
        }
    }
}
