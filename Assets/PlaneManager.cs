using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ZombieSpawner spawner;
    [SerializeField] private GameObject playingCanvas;

    [SerializeField] private ARPlaneManager planeManager;
    void Start()
    {
        spawner.enabled = false;
        playingCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        spawner.enabled = true;
        playingCanvas.SetActive(true);
        //planeManager.detectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
        planeManager.requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
    }
}
