using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSkill : MonoBehaviour
{
    [SerializeField] private int _totalCountdown;
    [SerializeField] private string _skillName;
    private float _count;
    public Text _txtCount;
    private Button _button;
    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _count = 0;
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _txtCount.text = _skillName;
    }

    // Update is called once per frame
    void Update()
    {
        if (_button.enabled) return;

        _count -= Time.deltaTime;
        int count = (int) Mathf.Round(_count);

        if(count <= 0)
        {
            count = 0;
            _button.enabled = true;
            _image.color = Color.white;
            _txtCount.text = _skillName;
        }
        else
        {
            _txtCount.text = count.ToString();
        }
        
    }

    public void OnButtonSkillPressed()
    {
        _count = _totalCountdown;
        _button.enabled = false;
        _image.color = Color.gray;

    }
   
}
