using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefend : MonoBehaviour
{
    //public enum Team { Blue, Red};
    public Team _myTeam = Team.Blue;

    public int _health = 1000;
    public int _damage = 10;
    public int _currentHealth = 0;

    public float _distanceAttack = 5f;
    public float _totalTimeToAttack = 1f;
    private float _countTimeToAttack = 0;

    private HealthBar _healthBar;

    private LineRenderer _lineRendererLaser;
    private Transform _pointLaser;
    //private int _damageLaser = 20;
    //private bool _isAttacking = false;

    private GameObject _target;


    private Material material;
    // Start is called before the first frame update
    void Start()
    {
        _healthBar = gameObject.GetComponentInChildren<HealthBar>();
        _pointLaser = transform.Find("Sphere");
        _lineRendererLaser = _pointLaser.GetComponent<LineRenderer>();
        _lineRendererLaser.enabled = false;
        material = gameObject.GetComponent<Renderer>().material;
        if(_myTeam == Team.Blue)
        {
            material.color = Color.blue;
        }
        else
        {
            material.color = Color.red;
        }
        _currentHealth = _health;
        _healthBar.SetMaxHealth(_health);
    }

    private void Update()
    {
       
        if(_target == null)
        {
            ScanObject();
        }
        else
        {
            AttackTarget();
        }

    }

    public void ChangeHealth(int health)
    {       
        _currentHealth -= health;
        _healthBar.SetValue(_currentHealth);
        if(_currentHealth < 0)
        {
            Debug.Log("Destroy Tower:" + gameObject.name);
        }
        //Debug.Log("Tower:" + gameObject.name+" tru mau:"+health);
    }

    private void AttackTarget()
    {

        
        _lineRendererLaser.SetPosition(0, _pointLaser.position);
        _lineRendererLaser.SetPosition(1, _target.transform.position);

        _countTimeToAttack -= Time.deltaTime;
        if(_countTimeToAttack <= 0)
        {
            _lineRendererLaser.enabled = true;
            _target.SendMessage("ChangeHealth", 20);
            _countTimeToAttack = _totalTimeToAttack;
            
        }
        if(_countTimeToAttack < _totalTimeToAttack - 0.2f)
        {
            _lineRendererLaser.enabled = false;
        }
        
        float dist = Vector3.Distance(transform.position, _target.transform.position);
        if(dist > _distanceAttack)
        {
            _target = null;
        }
    }

    private void ScanObject()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, _distanceAttack);
        foreach (Collider col in cols)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                //On off laser
                EnemyAI ea = col.gameObject.GetComponent<EnemyAI>();
                if (ea._myTeam != _myTeam)
                {
                    _target = col.gameObject;
                }  
            }
            if (col.gameObject.CompareTag("Player"))
            {
                //On off laser
                PlayerController ea = col.gameObject.GetComponent<PlayerController>();
                if (ea._myTeam != _myTeam)
                {
                    _target = col.gameObject;
                }
            }
        }
    }
}
