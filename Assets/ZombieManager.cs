using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ZombieManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ARRaycastManager m_arRaycastManager;
    [SerializeField] private GameObject m_ZombiePrefab;

    private List<ARRaycastHit> m_arRaycastHits = new List<ARRaycastHit>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                if (m_arRaycastManager.Raycast(touch.position, m_arRaycastHits))
                {
                    CreateZombie(m_arRaycastHits[0].pose.position);
                    return;
                }

                var ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        Debug.Log("Zombie hit");
                        DeleteZombie(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    private void CreateZombie(Vector3 position)
    {
        Instantiate(m_ZombiePrefab, position, Quaternion.identity);
    }

    private void DeleteZombie(GameObject zombieObj)
    {
        Handheld.Vibrate();
        Destroy(zombieObj);
    }
}
