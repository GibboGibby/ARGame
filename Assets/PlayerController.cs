using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject spawnPoint;

    [SerializeField] private Camera mainCam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy()
    {
        Instantiate(enemy, spawnPoint.transform);
    }

    public void Shoot()
    {
        RaycastHit hit;
        Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, Mathf.Infinity);
        if (hit.collider.CompareTag("Enemy"))
        {
            Destroy(hit.collider.gameObject);
        }
    }
}
