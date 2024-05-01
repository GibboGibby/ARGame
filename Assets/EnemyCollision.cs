using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float multiplier = 1f;
    [SerializeField] private ZombieController controller;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageEnemy(float damage)
    {
        controller.EnemyHit(damage * multiplier);
        GameManager.Instance.AddHitPoints((multiplier == 2.0f) ? 5f : 1f);
    }
}
