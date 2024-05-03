using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform playerTransform;

    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private PlayerRaycastController playerRaycastController;

    [SerializeField] private int pointsPerHit;
    [SerializeField] private int pointsPerKill;

    public PlayerStatsScriptableObject playerStats;

    [SerializeField] private GameObject GameStuff;
    [SerializeField] private GameObject UpgradeStuff;
    [SerializeField] private GameObject MenuStuff;

    //[SerializeField] private WeaponScriptableObject currentWeapon;

    [SerializeField] private ARPlaneManager planeManager;

    [SerializeField] private ZombieSpawner spawner;
    [SerializeField] private MenuManager menuManager;

    [SerializeField] private AudioSource playerAudioSource;

    private float timeSpentGaming = 0f;

    public int pointsPerRound = 0;

    public Transform PlayerTransform {
        get { return playerTransform; }
        private set { playerTransform = value; }
    }

    public static bool planesFound = false;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        spawner.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (menuManager.inGame)
        {
            timeSpentGaming += Time.deltaTime;
        }
    }

    public float TimeSpentGaming()
    {
        return timeSpentGaming;
    }

    public void ResetTimeSpentGaming()
    {
        timeSpentGaming = 0f;
    }

    private int currentAmmo;

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public void RemoveAmmo(int amt = 1)
    {
        currentAmmo -= amt;
        if (currentAmmo < 0) currentAmmo = 0;
    }

    public void ReloadWeapon()
    {
        currentAmmo = playerStats.currentWeapon.ammoPerMagazine.GetValue();
    }

    public WeaponScriptableObject GetCurrentWeapon()
    {
        return playerStats.currentWeapon;
    }

    public WeaponManager GetWeaponManager()
    {
        return weaponManager;
    }

    public MenuManager GetMenuManager()
    {
        return menuManager;
    }


    public void PlayerDied()
    {
        playerStats.Points += pointsPerRound;
        //pointsPerRound = 0;
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in zombies)
        {
            Destroy(obj);
        }
        spawner.enabled = false;
        playerRaycastController.SetHealth(playerStats.MaxHealth.GetValue());
        menuManager.ChangeState("gameover");
        
    }

    public void AddHitPoints(float multiplier)
    {
        pointsPerRound += (int)(pointsPerHit * multiplier);
    }

    public void AddKilledPoints()
    {
        pointsPerRound += pointsPerKill;
    }
    
    public void RemoveAllPlanes()
    {
        foreach (var plane in planeManager.trackables)
        {
            Destroy(plane.gameObject);
        }
    }

    public void SetDetection(bool val)
    {
        planeManager.requestedDetectionMode = (val) ? UnityEngine.XR.ARSubsystems.PlaneDetectionMode.Horizontal : UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
    }

    public void StartGame()
    {
        spawner.enabled = true;
        currentAmmo = playerStats.currentWeapon.ammoPerMagazine.GetValue();
        playerRaycastController.StartGameHealth();
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("FirstTimeBoot");

        playerStats.ResetProgress();
        weaponManager.ResetProgress();
    }

    public void ChangeWeapon(WeaponsEnum weapon)
    {
        playerStats.currentWeapon = weaponManager.GetWeapon(weapon);
        currentAmmo = weaponManager.GetWeapon(weapon).ammoPerMagazine.GetValue();
    }

    public AudioSource GetAudioSource()
    {
        return playerAudioSource;
    }
}
