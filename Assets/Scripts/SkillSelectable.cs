using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSelectable : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler// These are the interfaces the OnPointerUp method requires.
{
    private GameObject              _player;
    private float                   _touchTime = 0;
    private bool                    _pressed = false;
    private const float             ONE_CLICK_TIME = 0.1f;

    public float                    _totalDelay = 3;
    public TextMeshProUGUI          _texhMesh;
    public Image                    _image;
    private float                   _countDelay = 0;


    [SerializeField] public GameObject      _ObjDirectionSkill;
    private Joystick                        _joystickSkill;
    private Vector3                         _directionSkill;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _ObjDirectionSkill.SetActive(false);
        _joystickSkill = gameObject.GetComponent<Joystick>();
        _texhMesh.text = _countDelay.ToString();
    }
    private void Update()
    {
        if(_countDelay > 0)
        {
            _countDelay -= Time.deltaTime;
            _texhMesh.text = Mathf.Round(_countDelay).ToString();
            _image.color = Color.gray;

            return; // thoat Update vi dang count down
        }
        else
        {
            if(_texhMesh.text.Length > 0)
            {
                _image.color = Color.white;
                _countDelay = 0;
                _texhMesh.text = "";
            }            
        }        

        if (_pressed)
        {
            _touchTime += Time.deltaTime;
            if(_touchTime >= ONE_CLICK_TIME) // Draging button
            {
                if(_ObjDirectionSkill.activeSelf == false)
                {
                    _ObjDirectionSkill.SetActive(true) ;                    
                }
                Vector3 dir = new Vector3(_joystickSkill.Horizontal, 0, _joystickSkill.Vertical);
                if (dir != Vector3.zero)
                {
                    _directionSkill = dir;
                    _directionSkill.Normalize();
                    _ObjDirectionSkill.transform.rotation = Quaternion.LookRotation(_directionSkill);
                }
            }

        } 
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
        _touchTime = 0; 

    }  
    public void OnPointerUp(PointerEventData eventData)
    {
        if(_countDelay > 0) {  return; }

        _pressed = false;
        _ObjDirectionSkill.SetActive(false);
        if (_touchTime < ONE_CLICK_TIME)
        {
            _directionSkill = _player.transform.forward;
        }

        switch (gameObject.name)
        {
            case "Skill1 Joy":
                {
                    _player.SendMessage("AttackSkill_1", _directionSkill);
                }
                break;
            case "Flash Joy":
                {
                   _player.SendMessage("SkillFlash", _directionSkill);
                }
                break;
            default: break;
        }

        _countDelay = _totalDelay;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

}
