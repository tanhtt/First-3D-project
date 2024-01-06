using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum Team { Blue, Red };

public class EnemyAI : MonoBehaviour
{
    
    public Team _myTeam = Team.Blue;

    [SerializeField] public Transform[] _wayPoints;
    [SerializeField] private float _speed = 20;
    private int _currentWayPoint = 0;
    private Rigidbody _rigidbody;
    public bool _detectedPlayer = false;
    public bool _detectedEnemy = false;
    public float _distanceWaypoint = 6;
    public bool _isStun = false;


    // Start is called before the first frame update
    public EnemyController _enemyController;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _enemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyController._target != null)
        {
            return;
        }
        if(_isStun)
        {
            return;
        }
        MoveToWayPoint();
    }

    private void MoveToWayPoint()
    {
        if(_currentWayPoint >= _wayPoints.Length)
        {
            return;
        }


        Transform target = _wayPoints[_currentWayPoint];        

        //tính khoảng cách tới điểm waypoint hiện tại
        float distance = Vector3.Distance(transform.position, target.position);
        //Debug.Log("distance:" + distance);
        if (distance < _distanceWaypoint)
        {
            TowerDefend td = _wayPoints[_currentWayPoint].GetComponent<TowerDefend>();
            if (_myTeam == td._myTeam)
            {
                _currentWayPoint++;
            }
            else
            {
                //Attack to tower
                _enemyController.EnemyAttack(_wayPoints[_currentWayPoint].gameObject);
                
            }
            return;           
        }
        // di chuyen Enemy
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        direction.Normalize();
        _rigidbody.velocity = direction * _speed * Time.deltaTime;

        Vector3 smoothLook = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(Vector3.Lerp(transform.position, smoothLook, 2f));

    }
    
}
