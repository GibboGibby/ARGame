using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform playerTransform;

    [SerializeField] private WeaponManager weaponManager;

    [SerializeField] private int pointsPerHit;
    [SerializeField] private int pointsPerKill;

    public PlayerStatsScriptableObject playerStats;

    [SerializeField] private GameObject GameStuff;
    [SerializeField] private GameObject UpgradeStuff;
    [SerializeField] private GameObject MenuStuff;

    [SerializeField] private WeaponScriptableObject currentWeapon;

    [SerializeField] private ARPlaneManager planeManager;

    [SerializeField] private ZombieSpawner spawner;

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
        
    }

    public WeaponScriptableObject GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponManager GetWeaponManager()
    {
        return weaponManager;
    }


    public void PlayerDied()
    {
        playerStats.Points += pointsPerRound;
        pointsPerRound = 0;
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
    }

}
