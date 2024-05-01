using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField] private PlayerRaycastController controller;
    private void OnCollisionEnter(Collision collision)
    {
        controller.EnemyHitPlayer(collision);
    }
}
