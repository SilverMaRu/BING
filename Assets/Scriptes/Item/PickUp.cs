using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public ItemBaseInfo info;

    private Rigidbody rgBody;
    private Collider coll;

    private void Awake()
    {
        rgBody = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        ChangeToRigidbodyCollision();
    }

    private void OnEnable()
    {
        ChangeToRigidbodyCollision();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 9)
        {
            ChangeToKinematicTrigger();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            gameObject.SetActive(false);
        }
    }

    // 切换为刚体碰撞体
    private void ChangeToRigidbodyCollision()
    {
        rgBody.isKinematic = false;
        coll.isTrigger = false;
    }

    // 切换为运动学触发碰撞体
    private void ChangeToKinematicTrigger()
    {
        rgBody.isKinematic = true;
        coll.isTrigger = true;
    }
}
