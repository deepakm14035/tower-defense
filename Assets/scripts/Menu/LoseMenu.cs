using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuManagement
{
    public class LoseMenu : Menu<LoseMenu>
    {
        public void Replay()
        {
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.loadMenu(GameMenu.Instance, 1f, false);
            GameMenu.Instance.ResetGame();
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager.loadGame();
        }

        public void GoHome()
        {
            GameManager gm = FindObjectOfType<GameManager>();
            gm.GoHome();
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.loadMenu(MainMenu.Instance, 1f, false);
        }
    }
}