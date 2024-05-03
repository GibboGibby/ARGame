using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

class RaycastHitComprarer : IComparer
{
    public int Compare(object x, object y)
    {
        Vector3 playerPos = GameManager.Instance.PlayerTransform.position;
        RaycastHit hit1 = (RaycastHit)x;
        RaycastHit hit2 = (RaycastHit)y;
        float distPlayerHit1 = Vector3.Distance(playerPos, hit1.point);
        float distPlayerHit2 = Vector3.Distance(playerPos, hit2.point);
        if (distPlayerHit1 < distPlayerHit2) return -1;
        else if (distPlayerHit1 > distPlayerHit2) return 1;
        else return 0;
    }
}

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
        
    }

    public void StartGameHealth()
    {
        health = GameManager.Instance.playerStats.MaxHealth.GetValue();
        healthText.text = $"Health - {health}/{GameManager.Instance.playerStats.MaxHealth.GetValue()}";
    }

    // Update is called once per frame

    private float shootTimer = 0f;
    private bool canShoot = true;

    private void ShootTimer()
    {
        if (canShoot) return;

        shootTimer += Time.deltaTime;
        if (shootTimer >= GameManager.Instance.playerStats.currentWeapon.fireRate.GetValue())
        {
            canShoot = true;
            shootTimer = 0f;
        }

    }

    public void Reload()
    {
        if (reloading) return;
        if (GameManager.Instance.GetCurrentAmmo() == GameManager.Instance.GetCurrentWeapon().ammoPerMagazine.GetValue()) return;
        GameManager.Instance.GetAudioSource().pitch = GameManager.Instance.GetCurrentWeapon().reloadClip.length / GameManager.Instance.GetCurrentWeapon().reloadSpeed.GetValue();
        GameManager.Instance.GetAudioSource().PlayOneShot(GameManager.Instance.GetCurrentWeapon().reloadClip, GameManager.Instance.GetCurrentWeapon().volumeReload);
        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        reloading = true;
        yield return new WaitForSeconds(GameManager.Instance.GetCurrentWeapon().reloadSpeed.GetValue());
        GameManager.Instance.ReloadWeapon();
        reloading = false;
    }

    private bool reloading = false;
    void Update()
    {
        if (reloading) return;  
        ShootTimer();

        if (shootingStarted && canShoot && GameManager.Instance.GetCurrentAmmo() > 0)
        {
            Shoot();
            if (GameManager.Instance.playerStats.currentWeapon.singleShot)
            {
                shootingStarted = false;
            }
        }
    }

    public void SetHealth(int val)
    {
        health = val;
        healthText.text = $"Health - {health}/{GameManager.Instance.playerStats.MaxHealth.GetValue()}";
    }


    bool shootingStarted = false;
    public void Shoot()
    {
        Debug.Log("Shooting");
        canShoot = false;
        GameManager.Instance.RemoveAmmo();
        GameManager.Instance.GetAudioSource().pitch = 1f;
        GameManager.Instance.GetAudioSource().PlayOneShot(GameManager.Instance.GetCurrentWeapon().shotClip, GameManager.Instance.GetCurrentWeapon().volumeShot);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(camera.transform.position, camera.transform.forward, Mathf.Infinity);
        Array.Sort(hits, new RaycastHitComprarer());
        for (int i = 0; i < hits.Length; i++)
        {
            if (i > GameManager.Instance.GetCurrentWeapon().penetration.GetValue()) break;
            if (hits[i].collider.CompareTag("Enemy") || hits[i].collider.CompareTag("EnemyHead"))
            {
                Debug.Log("Collider found");
                hits[i].collider.gameObject.GetComponent<EnemyCollision>().DamageEnemy(GameManager.Instance.GetCurrentWeapon().damage.GetValue());
            }
        }
            

    }

    public void StartShoot()
    {
        shootingStarted = true;
    }

    public void StopShoot()
    {
        shootingStarted = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    public void EnemyHitPlayer(Collision collision)
    {
        //Debug.Log("Collision Detected");
        if (collision.collider.CompareTag("Enemy"))
        {
            health--;
            healthText.text = $"Health - {health}/{GameManager.Instance.playerStats.MaxHealth.GetValue()}";
            Destroy(collision.gameObject);
            Handheld.Vibrate();
            if (health <= 0)
            {
                //Debug.Log("Game Over");
                //Application.Quit();
                GameManager.Instance.PlayerDied();


            }
        }
    }
}
