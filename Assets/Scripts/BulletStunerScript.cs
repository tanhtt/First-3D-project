using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStunerScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SendMessage("SetStunner", 3);
            Destroy(gameObject);
        }
    }
}
