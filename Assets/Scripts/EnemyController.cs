using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject _player;
    public int _health = 100;
    private Rigidbody _rigidbody;
    public float _speed = 5;
    public float _distanceAttack = 1f;
    public GameObject _efStuner;

    public HealthBar _healthBar;
    private int _currentHealth;

    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _efExplosion;
    private EnemyAI _enemyAI;

    public float _isStuner = 0;
    private bool _isAttacking = false;

    public GameObject _target;//doi tuong de enemy tan cong: Enemy, Player, Tower
    public PlayerController _playerController;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody>();
        _currentHealth = _health;
        _healthBar.SetMaxHealth(_health);

        _enemyAI = GetComponent<EnemyAI>();
        if (_enemyAI._myTeam== Team.Blue)
        {
            _healthBar.SetColorTeam(Color.green);
        }
        else
        {
            _healthBar.SetColorTeam(Color.yellow);
        }       
        
        _anim.SetBool("isRunning", true);
        Physics.IgnoreLayerCollision(7, 7);
    }

    private void Update()
    {
        if(_isStuner == 0)
        {
            return;
        }
        _isStuner -= Time.deltaTime;
        if(_isStuner < 0)
        {
            _isStuner = 0;
            _enemyAI._isStun = false;
            _anim.SetBool("isRunning", true);
            //Debug.Log("Running again");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
               
        if(_isStuner > 0)
        {
            return;
        }
        if (_target == null)
        {
            return; //Enemy t? ??ng ?i ??n ?iêm waypoint
        }
        //Move to target to attack
        MoveToTarget();
    }

    public void MoveToTarget()
    {
        Vector3 targetPos = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z);
        float distance = Vector3.Distance(targetPos, transform.position);

        if (distance > _distanceAttack)
        {
            _anim.SetBool("isRunning", true);
            transform.LookAt(targetPos);
            Vector3 direction = targetPos - transform.position;
            direction.y = 0;
            direction.Normalize();
            _rigidbody.velocity = direction * _speed * Time.fixedDeltaTime;
            // transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
        }
        else
        {
            _anim.SetBool("isRunning", false);
            //tan cong Player
            EnemyAttack(_target);
            //Debug.Log("Target" + target.name);
        }
    }

    public void EnemyAttack(GameObject target)
    {
        _target = target;
        if (!_isAttacking)
        {
            StartCoroutine(WaitForAttacking());
        }
    }

    private IEnumerator WaitForAttacking()
    {
        _isAttacking = true;
        _anim.SetTrigger("AttackTrigger");
        StartCoroutine(WaitToDamage());
        //Debug.Log("Attack1");
        yield return new WaitForSeconds(2);
        _isAttacking = false;
    }

    private IEnumerator WaitToDamage()
    {
        yield return new WaitForSeconds(0.5f);
        if(_target != null)
        {
            _target.SendMessage("ChangeHealth", 5);
        }        
    }

    public void SetStunner(int stuner)
    {
        _isStuner = stuner;
        GameObject ef = GameObject.Instantiate(_efStuner, transform.position+ Vector3.up, Quaternion.identity, transform );
        Destroy(ef, stuner);
        ChangeHealth(-10);

        _enemyAI._isStun = true;
        _anim.SetBool("isRunning", false);

    }
    private void ChangeHealth(int heath)
    {
        _currentHealth -= heath;
        _healthBar.SetValue(_currentHealth);
        if(_currentHealth <= 0)
        {
            GameObject obj = GameObject.Instantiate(_efExplosion, transform.position, Quaternion.identity);
            Destroy(obj, 1f);
            Destroy(gameObject);
        }
    }

    public bool IsInScanningRange(Transform player, float scanningRadius)
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= scanningRadius;
    }    
}
