using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer_Creator : MonoBehaviour
{

    float time;
    public float Time_to_Lazer;

    bool spowned = false;

    public GameObject Lazer, /*Lazer_container*/ Right, Left;

    public float spawn_right_X, spawn_left_X, spawnValuesDownY,spawnValuesUpY;

    void Awake(){
        spawn_right_X = Right.GetComponent<Transform>().position.x ;

        spawn_left_X = Left.GetComponent<Transform>().position.x ;
    }
    void Update()
    {

        if (!spowned)
            StartCoroutine(Creat());
    }


    IEnumerator Creat(){
        spowned = true;
        yield return new WaitForSeconds(Time_to_Lazer);
        spawnValuesDownY = Cam_controller.down;
        spawnValuesUpY = Cam_controller.up;
        float X = Random.Range(0,10);

        if (X < 5.0f){
            X = spawn_right_X;
        } else{
            X = spawn_left_X;
        }
        Instantiate(Lazer, new Vector3(X,Random.Range (spawnValuesUpY , spawnValuesDownY)), Quaternion.identity);
        spowned = false;
    }
}
