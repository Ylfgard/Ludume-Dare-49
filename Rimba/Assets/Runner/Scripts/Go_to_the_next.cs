using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Go_to_the_next : MonoBehaviour
{
    public static int lvl = 4;
    public KeyCode key = KeyCode.LeftControl;


     void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            if(Input.GetKey(key))
                {
                    lvl++;
                    SceneManager.LoadScene(Go_to_the_next.lvl);
                }
    }
} 
