using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public float _delayTime     = 1f;
    public GameObject _enemy;
    public int _totalEnemy = 5;
    public Team _enemyTeam;
    
    private float _delay         = 0f;
    private int _countEnemy = 0;

    [SerializeField] private Transform[] _wayPointsMid;

    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(_countEnemy >= _totalEnemy)
        {
            return;
        }
        _delay -= Time.deltaTime;
        if (_delay <= 0)
        {
            _delay = _delayTime;
            GameObject em = GameObject.Instantiate(_enemy, transform.position, Quaternion.identity);
            em.GetComponent<EnemyAI>()._wayPoints = _wayPointsMid;
            em.GetComponent<EnemyAI>()._myTeam = _enemyTeam;
            
            em.name = "Enemy " + _enemyTeam.ToString();
            _countEnemy++;
        }
    }
}
