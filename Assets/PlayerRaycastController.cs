using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerRaycastController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera camera;
    [SerializeField] private LayerMask rayMask;

    [SerializeField] private int maxHealth = 3;
    private int health;

    [SerializeField] private TextMeshProUGUI healthText;
    
    void Start()
    {
        health = maxHealth;
        healthText.text = $"Health - {health}/{maxHealth}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.gameObject.GetComponent<ZombieController>().EnemyDied();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision Detected");
        if (collision.collider.CompareTag("Enemy"))
        {
            health--;
            healthText.text = $"Health - {health}/{maxHealth}";
            Destroy(collision.gameObject);
            if (health <= 0)
            {
                Debug.Log("Game Over");
                Application.Quit();
            }
        }
    }
}
