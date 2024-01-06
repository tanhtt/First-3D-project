using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    public float        _delayAttack    = 1f;
    private float       _delay          = 0;
    public float        _radius         = 3;
    public LayerMask    _layerMaskEnemy;
    private void Start()
    {
        _delay = 1f;
    }

    private void Update()
    {
        _delay -= Time.deltaTime;    
        if(_delay <= 0)
        {
            _delay = _delayAttack;
            RaycastHit[] listEnemy = Physics.SphereCastAll(transform.position, _radius, transform.forward, _radius, _layerMaskEnemy);
            foreach(RaycastHit hit in listEnemy)
            {
                Debug.Log("Stun enemy:" + hit.transform.name);
                hit.transform.SendMessage("ChangeHealth", 25);
            }

        }
    }   
}
