using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Creating : MonoBehaviour
{

    float time;
    public float Time_to_Bomb;

    bool spowned = false;

    public GameObject Bomb_circle;

    public float spawnValuesX = 3, spawnValuesDownY = Cam_controller.down ,spawnValuesUpY = Cam_controller.up;

    void Update()
    {

        if (!spowned)
            StartCoroutine(Creat());
    }


    IEnumerator Creat(){
        spowned = true;
        yield return new WaitForSeconds(Time_to_Bomb);
        spawnValuesDownY = Cam_controller.down;
        spawnValuesUpY = Cam_controller.up;
        Instantiate(Bomb_circle, new Vector3(Random.Range (-spawnValuesX, spawnValuesX),Random.Range (spawnValuesUpY , spawnValuesDownY)), Quaternion.identity);
        spowned = false;
    }
}
