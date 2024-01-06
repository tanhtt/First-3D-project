using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    public GameObject _trail;

    private GameObject _player;

    private bool _canAttack = true;

    void Start()
    {
        _trail.SetActive(false);
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    public void TurnOnOffTrail(bool isTrail)
    {
        _trail.SetActive(isTrail);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if(!_player.GetComponent<PlayerController>()._canAttack) // Player is attacking
            {
                if (_canAttack) // sword can attack
                {
                    _player.GetComponent<PlayerController>().AttackEffect(other.transform, _trail.transform);
                    StartCoroutine(SwordWaitForAttack());
                    Debug.Log("Sword Attacking");
                }
            }     
        }
        if (other.gameObject.layer == 7)
        {
            if (!_player.GetComponent<PlayerController>()._canAttack)
            {
                if (_canAttack)
                {
                    _player.GetComponent<PlayerController>().AttackEffect(other.transform, _trail.transform);
                    StartCoroutine(SwordWaitForAttack());
                    Debug.Log("Sword Attacking");
                }
            }
        }
    }

    private IEnumerator SwordWaitForAttack()
    {
        _canAttack = false;
        yield return new WaitForSeconds(0.5f);
        _canAttack = true;

    }

}
