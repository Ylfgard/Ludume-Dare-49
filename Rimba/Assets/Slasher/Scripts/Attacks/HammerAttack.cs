using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerAttack : MonoBehaviour
{
    private int damage = 100;
    public GameObject attacker;
    private float time;
    private float speed = 3.0f;
    private float timeOfAttack = 0.5f;

    private FMOD.Studio.EventInstance instance;
    private Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        attacker = gameObject.transform.parent.gameObject;
        transform.position = attacker.transform.position;

        Debug.Log(attacker.name);
        
        if (attacker.CompareTag("Player"))
        {
            target = attacker.GetComponent<PlayerController>().mouseRel;
        }

        if (attacker.CompareTag("Enemy") || attacker.CompareTag("Boss"))
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
        instance = FMODUnity.RuntimeManager.CreateInstance("event:/whoosh_rock_throw");
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(attacker.transform.position));
        instance.start();
        instance.release();
        yield return new WaitForSeconds(timeOfAttack);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(attacker != null)
        {
            if ((attacker.CompareTag("Enemy") || attacker.CompareTag("Boss")) && other.gameObject.CompareTag("Player")  )
            {
                other.gameObject.GetComponent<PlayerController>().UpdateHP(-damage);
            }
            
            else if ( attacker.CompareTag("Player") && other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<EnemyController>().UpdateHP(-damage);
            }

            else if(attacker.CompareTag("Player") && other.gameObject.CompareTag("Boss"))
            {
                other.gameObject.GetComponent<DaVinchiMechController>().UpdateHP(-damage);
            }
        }
    }
}
