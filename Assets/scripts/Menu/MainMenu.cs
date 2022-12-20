using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MenuManagement
{
    public class MainMenu : Menu<MainMenu>
    {
        public Text coinsText;
        public Text topScore;
        private void Start()
        {
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        }

        private void OnEnable()
        {
            StartCoroutine(loadStats());
        }

        IEnumerator loadStats()
        {
            yield return new WaitForEndOfFrame();
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        }

        /*public void playLevel(int level)
        {
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            menuManager.loadMenu(GameMenu.Instance, 1f, false);
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            gameManager.loadGame(level);
        }*/

        public void loadLevelSelector()
        {
            MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
            LevelSelectionMenu.Instance.setup();
            menuManager.loadMenu(LevelSelectionMenu.Instance, 1f, false);
        }

    }
}