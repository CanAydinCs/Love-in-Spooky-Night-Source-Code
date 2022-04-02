using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int startHealth = 100;

    Slider healthBar;

    float health;

    private void Start()
    {
        healthBar = GetComponentInChildren<Slider>();

        health = startHealth;
        healthBar.value = health / startHealth;

        if (CompareTag("Player"))
        {
            healthBar.gameObject.GetComponent<Image>().color = Color.blue;
        }
        else if (CompareTag("enemy"))
        {
            healthBar.gameObject.GetComponent<Image>().color = Color.red;
        }
        else if (CompareTag("pasif"))
        {
            healthBar.gameObject.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            healthBar.gameObject.GetComponent<Image>().color = Color.green;
        }
    }

    private void Update()
    {
        GetComponentInChildren<Canvas>().transform.LookAt(Camera.main.transform);
    }

    public void TakeDamage(float value)
    {
        health -= value;
        if(health <= 0)
        {
            tag = "dead";
            Invoke("Death", 3f);
        }
        healthBar.value = health / startHealth;
    }

    void Death()
    {
        Destroy(gameObject);
    }

    public void Heal(float value)
    {
        if (CompareTag("pasif") && Input.GetKey(KeyCode.F))
        {
            tag = "dead";
            GameObject.FindGameObjectWithTag("Player").GetComponent<SpawnAllay>().allayNumber += 1;
            Invoke("Death", 3f);
            return;
        }

        health += value;
        if(health >= startHealth)
        {
            health = startHealth;
        }
        healthBar.value = health / startHealth;
    }
}
