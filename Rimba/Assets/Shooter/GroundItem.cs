using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    protected Collider2D m_collider;
    protected bool OnGround;

    protected void OnEnable()
    {
        m_collider = GetComponent<Collider2D>();
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Collector collector = collision.gameObject.GetComponent<Collector>();
        if (collector  != null)
        {
            PickUp(collector);
        }
        
    }
    protected virtual void PickUp(Collector collector)
    {

    }
    public void EnableCollider()
    {
        m_collider.enabled = true;
    }
    public void DisableCollider()
    {
        m_collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
