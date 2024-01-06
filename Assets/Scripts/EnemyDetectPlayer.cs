using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDetectPlayer : MonoBehaviour
{
    private EnemyAI _enemyAI;

    private void Start()
    {
        _enemyAI = gameObject.GetComponentInParent<EnemyAI>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(_enemyAI._enemyController._target != null)
        {
            return;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyAI enemyAI = other.GetComponent<EnemyAI>();
            if (_enemyAI._myTeam != enemyAI._myTeam)
            {
                _enemyAI._enemyController._target =  other.gameObject;
            }
        }
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if(_enemyAI._myTeam != playerController._myTeam)
            {
                // Uu tien danh enemy
                if(_enemyAI._enemyController._target == null)
                {
                    _enemyAI._enemyController._target = other.gameObject;
                }
               
            }           
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (_enemyAI._enemyController._target == null)
        {
            return;
        }
        if (_enemyAI._enemyController._target == other.gameObject)
        {
            _enemyAI._enemyController._target = null;
        }
        /*
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (_enemyAI._myTeam != playerController._myTeam)
            {
                //Kiem tra xem co dung la player ma enemy dang tan cong 
                               
            }
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyAI enemyAI = other.GetComponent<EnemyAI>();
            if (_enemyAI._myTeam != enemyAI._myTeam)
            {
                //Kiem tra xem co dung la enemy hien ta dang tan cong hay khong
                if(_enemyAI._enemyController._target == other.gameObject)
                {
                    _enemyAI._enemyController._target = null;
                }          
            }
        }
        */
    }
}
