using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElusiveRimba
{

    public class KnifePickUp : MonoBehaviour
    {
        private Weapon weaponScript;

        public bool canPickUp { get; set; }

        private void Awake()
        {
            weaponScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Weapon>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                // Pick up knife
                weaponScript.PlusOneKnife();
                Destroy(gameObject);
            }
        }
    }
}
