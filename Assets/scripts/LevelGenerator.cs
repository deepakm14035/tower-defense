using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/*
 * 
Story:
zombie
random towers every time a user plays

 enemies-
 med speed, med health
 low speed, high health
 med speed, self healing
 after getting destroyed, spawns smaller faster enemies
     
tower-
-basic
-inferno
-laser
-docking multiple
-small range, high damage
-reduce speed of all enemies in range
-teleport to another position within range
-weighing scale - on button click, increasing speed and life of enemy for 
 10 seconds, on another button click, increase attack speed and damage
-unlimited range, takes long time to completely destroy enemy in one shot
-box/line - if any projectile passes through that, their damage and speed gets doubled (zora from black clover)
-a bad tower that generates more money if it destroys enemy

special abilities-
slow down everything on track
poison
move any tower to another location

     */


public class LevelGenerator : MonoBehaviour
{
    public bool isGameStarted = false;
    int currentRound = 0;
    int roundSpawnIndex = 0;
    int indexInPath = 0;
    public GameObject[] enemies;
    public Level currentLevel;
    [SerializeField] private LineRenderer path;
    [SerializeField] public SoundManager source;
    public bool allRoundsComplete = false;
    bool winPlayed = false;


    public void generateBG(Vector4 rect)
    {
        //for(int )
    }

    IEnumerator SpawnRounds()
    {
        for (currentRound = 0; currentRound < currentLevel.rounds.Length; currentRound++)
        {
            for (int currentType = 0; currentType < currentLevel.rounds[currentRound].enemyCount.Length; currentType++)
            {
                for (int i = 0; i < currentLevel.rounds[currentRound].enemyCount[currentType]; i++)
                {
                    GameObject enemy = Instantiate(enemies[currentLevel.rounds[currentRound].enemyType[currentType]],
                        currentLevel.path[0],
                        Quaternion.identity);
                    enemy.GetComponent<Enemy>().source = source;
                    yield return new WaitForSeconds(currentLevel.rounds[currentRound].spawnGap);
                }
                yield return new WaitForSeconds(currentLevel.rounds[currentRound].enemyTypeSpawnGap);
            }
            GameObject.FindObjectOfType<GameMenu>().updateRoundDetails(currentRound+2,currentLevel.rounds.Length);
            yield return new WaitForSeconds(3f);
        }
        allRoundsComplete = true;
        yield return null;
    }

    public IEnumerator setStartCoins()
    {
        yield return new WaitForSeconds(1f);
        GameObject.FindObjectOfType<GameMenu>().updateCoinCount(currentLevel.startingCoins);
        yield return null;
    }

    // Start is called before the first frame update
    public void StartGame()
    {
        GameObject.FindObjectOfType<GameMenu>().updateRoundDetails(1,currentLevel.rounds.Length);
        GameObject.FindObjectOfType<GameMenu>().updateHealthCount(currentLevel.startingHealth);
        //path = GetComponent<LineRenderer>();
        StartCoroutine(SpawnRounds());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isGameStarted = true;
        }
        if (isGameStarted)
        {
            if (allRoundsComplete && !winPlayed)
            {
                Debug.Log("checking length- " + GameObject.FindGameObjectsWithTag("enemy").Length);
                if (GameObject.FindGameObjectsWithTag("enemy").Length == 0)
                {
                    GameManager gm = FindObjectOfType<GameManager>();
                    StartCoroutine(gm.playWinAnim());
                    winPlayed = true;
                }
            }
        }
    }
}
