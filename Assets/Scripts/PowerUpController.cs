using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    public GameObject[] listSwords;
    public int selectedIndex;
    private void Start()
    {
        CreateSword(selectedIndex);
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * speed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SendMessage("CreateSword", selectedIndex);
            Destroy(gameObject);
        }
    }

    private void CreateSword(int index)
    {
        GameObject.Instantiate(listSwords[index], transform);
    }
    


}
