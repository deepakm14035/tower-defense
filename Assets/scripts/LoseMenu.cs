using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuManagement
{
    public class LoseMenu : Menu<LoseMenu>
    {
        public void GoHome()
        {

        }
        public void Replay()
        {
            GameManager gm = FindObjectOfType<GameManager>();
            gm.loadGame();
        }
    }
}