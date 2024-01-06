using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealth : MonoBehaviour
{
    [SerializeField] public float health = 50f;
    public GameObject _efExplosion;
   
   public void ChangeHealth(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            GameObject ef = GameObject.Instantiate(_efExplosion, transform.position, Quaternion.identity);
            Destroy(ef, 2f);
            Destroy(gameObject);
        }
    }
}
