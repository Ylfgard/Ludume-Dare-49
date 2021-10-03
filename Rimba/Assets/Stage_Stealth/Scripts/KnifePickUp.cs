using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElusiveRimba
{

    public class KnifePickUp : MonoBehaviour
    {


        public bool canPickUp { get; set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                // Pick up knife
                Debug.LogWarning("Knife should be added to inventory");
                Destroy(gameObject);
            }
        }
    }
}
