using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI timeText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        pointsText.text = GameManager.Instance.pointsPerRound.ToString();
        int min = (int)(GameManager.Instance.TimeSpentGaming() / 60);
        string minStr = (min < 10) ? $"0{min}" : $"{min}";
        int sec = (int)(GameManager.Instance.TimeSpentGaming() % 60);
        string secStr = (sec < 10) ? $"0{sec}" : $"{sec}";
        timeText.text = $"{minStr}:{secStr}";
    }

    public void BackToMenu()
    {
        GameManager.Instance.ResetTimeSpentGaming();
        GameManager.Instance.pointsPerRound = 0;
        GameManager.Instance.GetMenuManager().ChangeState("menu");
    }
}
