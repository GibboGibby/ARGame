using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    public enum MenuState
    {
        Menu,
        Upgrades,
        DefinePlayArea,
        Game,
        GameOver
    }

    public bool inGame;

    public static bool GoingToGame;

    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Upgrades;
    [SerializeField] private GameObject DefinePlayArea;
    [SerializeField] private GameObject Game;
    [SerializeField] private GameObject GameOver;

   

    void Start()
    {
        GameManager.Instance.SetDetection(false);
        ToMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void ChangeState(MenuState state)
    {
        switch (state)
        {
            case MenuState.Menu:
                ToMenu();
                break;
            case MenuState.Upgrades:
                ToUpgrades();
                break;
            case MenuState.DefinePlayArea:
                ToDefinePlayArea();
                break;
            case MenuState.Game:
                ToGame();
                break;
            case MenuState.GameOver:
                ToGameOver();
                break;
            default:
                break;
        }
    }

    public void HideAll()
    {
        inGame = false;
        Menu.SetActive(false);
        Upgrades.SetActive(false);
        DefinePlayArea.SetActive(false);
        Game.SetActive(false);
        GameOver.SetActive(false);
    }

    public void ToMenu()
    {
        HideAll();
        Menu.SetActive(true);
    }


    public void ToGame()
    {
        HideAll();
        inGame = true;
        if (!GameManager.planesFound && !MenuManager.GoingToGame)
        {
            MenuManager.GoingToGame = true;
            ToDefinePlayArea();
            return;
        }
        MenuManager.GoingToGame = false;
        Game.SetActive(true);
        GameManager.Instance.StartGame();
    }

    public void ToDefinePlayArea()
    {
        HideAll();
        GameManager.Instance.SetDetection(true);
        DefinePlayArea.SetActive(true);
    }

    public void ToUpgrades()
    {
        HideAll();
        Upgrades.SetActive(true);

    }

    public void ToGameOver()
    {
        HideAll();
        GameOver.SetActive(true);
    }

    public void ChangeState(string state)
    {
        switch (state.ToLower())
        {
            case "menu":
                ChangeState(MenuState.Menu);
                break;
            case "upgrades":
                ChangeState(MenuState.Upgrades);
                break;
            case "defineplayarea":
                ChangeState(MenuState.DefinePlayArea);
                break;
            case "game":
                ChangeState(MenuState.Game);
                break;
            case "gameover":
                ChangeState(MenuState.GameOver);
                break;
            default:
                Debug.Log("No state with that name");
                break;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DoneDefiningPlayArea()
    {
        if (MenuManager.GoingToGame)
            ChangeState(MenuState.Game);
        else
            ChangeState(MenuState.Menu);
    }
}
