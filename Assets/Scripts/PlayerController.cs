using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    public Team _myTeam = Team.Blue;
    [SerializeField] private Rigidbody  _rigidbody;
    [SerializeField] private Joystick   _joystick;
    [SerializeField] private Animator   _animator;
    [SerializeField] private ParticleSystem _efRun;

    [SerializeField] private GameObject _efAttackRock;
    [SerializeField] private GameObject _efAttackWall;
    [SerializeField] private GameObject _efAttackEnemy;


    [SerializeField] private GameObject _bulletSkill1;
    [SerializeField] private int        _bulletSpeedSkill1 =200;

    [SerializeField] private float      _ultiScanningRadius = 10f;
    [SerializeField] private GameObject _efUltiOnEnemy;

    [SerializeField] private GameObject _efHealing;

    [SerializeField] private LayerMask  _layerMaskEnemy;

    //HealthBar
    [SerializeField] private int        _maxHealth = 50;
    private int                         _currentHealth;
    [SerializeField] private HealthBar  _healthBar;
    //----

   
    [SerializeField] private GameObject _efFlash;
    //----

    //Audio Player
    [SerializeField] private AudioSource _audioSword;
    [SerializeField] private AudioSource _audioHit;
    [SerializeField] private AudioSource _audioSkill1;
    //[SerializeField] private AudioSource _audioSkill2;
    //[SerializeField] private AudioSource _audioSkill3;
    [SerializeField] private AudioSource _audioFlash;
    //----

    [SerializeField] private float speed;
    public bool _canAttack = true;
    private bool _canJump = true;
    private enum PlayerState
    {
        Idle,
        Run,
        Jump,
        Attack,
    }
    [SerializeField] public GameObject[] listSwords;
    private GameObject currentSword;
    public Transform swordSocket;

    private void Start()
    {
        CreateSword(0);
        _currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
    }
    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ChangeHealth(10);
        }
    }

    void FixedUpdate()
    {
        
        _rigidbody.velocity = new Vector3(_joystick.Horizontal * speed, 0, _joystick.Vertical * speed);
        
        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
            _animator.SetBool("isRunning", true);
            if(!_efRun.isPlaying)
            {
                _efRun.Play();
            }
        }
        else
        {
            _animator.SetBool("isRunning", false);
            if (_efRun.isPlaying)
            {
                _efRun.Stop();
            }
        }
    }


    public void OnJumpButtonPressed()
    {
        if (_canJump)
        {
            StartCoroutine("WaitForJump");
        }
    }

    public void OnAttackButtonPressed()
    {
        if (_canAttack)
        {
            StartCoroutine("WaitForAttack");
        }           

    }
    public IEnumerator WaitForAttack()
    {
        _canAttack = false;
        _audioSword.Play();
        _animator.SetTrigger("AttackTrigger");
        gameObject.BroadcastMessage("TurnOnOffTrail", true);
        yield return new WaitForSeconds(0.533f);
        _canAttack = true;
        gameObject.BroadcastMessage("TurnOnOffTrail", false);
    }

    public IEnumerator WaitForJump()
    {
        _canJump = false;
        _animator.SetTrigger("JumpTrigger");
        yield return new WaitForSeconds(0.8f);
        _canJump = true;
    }

    

    public void CreateSword(int index)
    {
        if(currentSword != null)
        {
            Destroy(currentSword);
        }
        currentSword = GameObject.Instantiate(listSwords[index], swordSocket);
    }

    public void AttackEffect(Transform target, Transform pointEf)
    {
        
        GameObject ef;
        switch (target.tag)
        {
            case "Rock":
                {
                    ef = GameObject.Instantiate(_efAttackRock, pointEf.position, Quaternion.identity);
                    Destroy(ef, 2f);
                    target.SendMessage("ChangeHealth", 10);
                    _audioHit.Play();
                }
                break;
            case "Wall":
                {
                    ef = GameObject.Instantiate(_efAttackWall, pointEf.position, Quaternion.identity);
                    Destroy(ef, 2f);
                    target.SendMessage("ChangeHealth", 10);
                    _audioHit.Play();
                }
                break;
            case "Enemy":
                {
                    ef = GameObject.Instantiate(_efAttackEnemy, pointEf.position, Quaternion.identity);
                    target.SendMessage("ChangeHealth", 50);
                    _audioHit.Play();
                }
                break;
            default:
                break;
        }        
    }
    
    //-------------------------------------------------------------------------//
    /// Skill 1: action
    //-------------------------------------------------------------------------//    
    public void AttackSkill_1(Vector3 direction)
    {
        GameObject bullet = GameObject.Instantiate(_bulletSkill1, currentSword.transform.position + Vector3.up, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(direction * _bulletSpeedSkill1);
        Destroy(bullet, 2f);
        _audioSkill1.Play();
    }    

    //-------------------------------------------------------------------------//
    //<summary>
    /// Skill 2: action
    /// </summary>
    public void Healing()
    {
        GameObject ef = GameObject.Instantiate(_efHealing, transform.position, Quaternion.identity);
        Destroy(ef, 1f);
        ChangeHealth(-20);
    }
    //-------------------------------------------------------------------------//
    // Skill Ultimate: Action
    //------------------------------------------------------------------------------------------//
    public void AttackSkillUlti()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _ultiScanningRadius);

        foreach (Collider collider in colliders)
        {
            EnemyController enemy = collider.GetComponent<EnemyController>();
            if (enemy != null && enemy.IsInScanningRange(transform, _ultiScanningRadius))
            {
                _animator.SetTrigger("UltiTrigger");
                GameObject meteorShower = GameObject.Instantiate(_efUltiOnEnemy, enemy.transform.position + new Vector3(0, 12, 0), Quaternion.identity);
                Destroy(meteorShower, 5f);
                //Debug.Log("Target selected: "+ enemy.name);
                return;
            }
        }

        Debug.Log("No target in range!");
    }

    //------------------------------------------------------------------------------------------//

    // Skill Flash
    //------------------------------------------------------------------------------------------//
    public void SkillFlash(Vector3 direction)
    {
        
        Vector3 currentPosition = transform.position;
        GameObject efStart = GameObject.Instantiate(_efFlash, currentPosition + new Vector3(0,1,0), Quaternion.Euler(-90,0,0));
        Destroy(efStart, 1f);
        
        //Debug.Log("Curent" + currentPosition);
        Vector3 newPosition = currentPosition + new Vector3(direction.x * 3, 0f, direction.z * 3);
        //Debug.Log("New" + newPosition);

        Vector3 offset = newPosition - currentPosition;
        Vector3 tele = currentPosition + Vector3.ClampMagnitude(offset, 4f);
        _rigidbody.MovePosition(tele);
        GameObject efEnd = GameObject.Instantiate(_efFlash, tele + new Vector3(0, 1, 0), Quaternion.Euler(-90, 0, 0));
        Destroy(efEnd, 1f);
        _audioFlash.Play();
    }    

    //------------------------------------------------------------------------------------------//

    // Player Health
    //------------------------------------------------------------------------------------------//
    private void ChangeHealth(int damage)
    {
        _currentHealth -= damage;
        if(_currentHealth <= 0)
        {
            _currentHealth = 0;
        }
        if(_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        _healthBar.SetValue(_currentHealth);
    }
    //------------------------------------------------------------------------------------------//
}

