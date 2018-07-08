using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsRemoveBullet : MonoBehaviour {

    // Bullet Spark Effect
    public GameObject sparkEffect;

    private void OnCollisionEnter(Collision collision)
    {
        // find tag
        if (collision.collider.tag == "BULLET")
        {
            ShowEffect(collision);
            Destroy(collision.gameObject);    
        }
    }

    void ShowEffect(Collision coll)
    {
        // GET CONTACT POINT
        ContactPoint contact = coll.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        // show spark effect
        GameObject spark = Instantiate(sparkEffect, contact.point, rot);
        spark.transform.SetParent(this.transform);
    }
}
