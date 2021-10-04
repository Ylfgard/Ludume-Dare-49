using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lazer_Rotation : MonoBehaviour
{
    public GameObject Lazer;

    public float RForRandomAngle;
    public float wait_for_shoot;

    bool Coroutine_is_running = false, shooting = false;
    public float Dead_time;
 
    Coroutine coroutine;
    Transform _tr;
    
    void Awake(){
        _tr = Lazer.GetComponent<Transform>();
    }
    void Update()
    {
        StartCoroutine(Stop_moving());

        if(!shooting){
            if (coroutine == null)
            {
                float Angle = Random.Range(-RForRandomAngle,RForRandomAngle);
                coroutine = StartCoroutine(c_Rotate( Random.Range(-Angle, Angle), wait_for_shoot));
            }
        }
        else{
            if(!Coroutine_is_running)
                StartCoroutine(Shoot());
        }
    }
 

    IEnumerator Stop_moving(){
         yield return new WaitForSeconds(wait_for_shoot);
         shooting = true;
    }
    IEnumerator c_Rotate(float angle, float intensity)
    {
        var me = Lazer.transform;
        var to = me.rotation * Quaternion.Euler(0.0f, 0.0f, angle);
 
        while (true)
        {
            me.rotation = Quaternion.Lerp(me.rotation, to, intensity * Time.deltaTime);
 
            if (Quaternion.Angle(me.rotation, to) < 0.01f)
            {
                coroutine = null;
                me.rotation = to;
                yield break;
            }
 
            yield return null;
        }
    }

    IEnumerator Shoot(){
        Coroutine_is_running = true;
        yield return new WaitForSeconds(wait_for_shoot);
        gameObject.AddComponent<BoxCollider2D>();
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        _tr.localScale = new Vector3(_tr.localScale.x , _tr.localScale.y * 15, _tr.localScale.z);
        Destroy(gameObject,Dead_time);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            SceneManager.LoadScene("First");
    }
}

