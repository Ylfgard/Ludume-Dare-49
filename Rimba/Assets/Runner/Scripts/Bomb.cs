using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bomb : MonoBehaviour
{
    public float Speed , radius = 1;    
    Transform Bomb_tr;
    bool IsBooming = false;
    public float wait_for_boom, Dead_time;
    public float spawnValuesX = 3, spawnValuesY = 3;
    bool Coroutine_is_running = false;

    void Awake(){

        Bomb_tr = gameObject.GetComponent<Transform>();

    }

    void Update()
    {
        StartCoroutine(Stop_moving());

        if(!IsBooming){
            Bomb_tr.position = new Vector2 ( Bomb_tr.position.x + Random.Range (-radius, radius) * Time.deltaTime , Bomb_tr.position.y + Random.Range (-radius, radius) * Time.deltaTime);
        } else{
            if (!Coroutine_is_running)
            StartCoroutine(Boom());
        }
        
    }

     IEnumerator Stop_moving(){
         yield return new WaitForSeconds(wait_for_boom);
         IsBooming = true;
    }

     IEnumerator Boom(){
        Coroutine_is_running = true;
        yield return new WaitForSeconds(wait_for_boom);
        gameObject.AddComponent<BoxCollider2D>();
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        Destroy(gameObject,Dead_time);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            SceneManager.LoadScene(Go_to_the_next.lvl);
    }
}

