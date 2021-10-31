using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/*
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

special abilities-
slow down everything on track
poison


     */


public class LevelGenerator : MonoBehaviour
{
    public bool isGameStarted = false;
    int currentRound = 0;
    int roundSpawnIndex = 0;
    int indexInPath = 0;
    public GameObject[] enemies;
    [SerializeField] private Level currentLevel;
    [SerializeField] private LineRenderer path;


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
                        path.GetPosition(indexInPath),
                        Quaternion.identity);
                    yield return new WaitForSeconds(currentLevel.rounds[currentRound].spawnGap);
                }
                yield return new WaitForSeconds(currentLevel.rounds[currentRound].enemyTypeSpawnGap);
            }
            GameObject.FindObjectOfType<UIController>().updateRoundDetails(currentRound+2,currentLevel.rounds.Length);
            yield return new WaitForSeconds(3f);
        }
        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindObjectOfType<UIController>().updateCoinCount(currentLevel.startingCoins);
        GameObject.FindObjectOfType<UIController>().updateRoundDetails(1,currentLevel.rounds.Length);
        GameObject.FindObjectOfType<UIController>().updateHealthCount(currentLevel.startingHealth);
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
    }
}
