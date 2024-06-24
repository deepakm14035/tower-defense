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
                                                                                                +                                   -
-basic
-inferno
-laser
-docking multiple                                                                           strong ability/save space      expensive to dock
-small range, high damage
-reduce speed of all enemies in range
-teleport to another position within range                                                  cover more ground               takes time to teleport
-weighing scale - on button click, increasing speed and life of enemy for 
 10 seconds, on another button click, increase attack speed and damage
-unlimited range, takes long time to completely destroy enemy in one shot
-box/line - if any projectile passes through that, their damage and speed gets doubled
-a bad tower that generates more money if it destroys enemy                                     

special abilities-
slow down everything on track
poison
move any tower to another location



story 2-
towers are based on elements like earth, fire, water, electricity, plants, wind
enemies are based on creatures like animals (snow animals, tropical forest animals, swamp animals, desert animals), fishes, birds, robots
                snow animal     tropical forest animal      swamp       desert      fish    bird        robots
earth               damage         no effect                no effect   no effect   damage  damage      no effect
fire                damage          damage                  damage      no effect   damage  damage      both
water               no effect       no effect               no effect   damage    no effect damage      damage
electricity         damage          damage                  no effect   no effect   damage  damage      no effect
plants              damage          no effect               no effect   no effect no effect no effect   damage
wind                no effect       depends (weight)        damage      no effect damage    slow down   damage

     */


public class LevelGenerator : MonoBehaviour
{
    public bool isGameStarted = false;
    int currentRound = 0;
    public GameObject[] enemies;
    public Level currentLevel;
    [SerializeField] private LineRenderer path;
    [SerializeField] public SoundManager source;
    public bool allRoundsComplete = false;
    bool winPlayed = false;
    public Coroutine EnemySpawner;

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

                    if(currentType == currentLevel.rounds[currentRound].enemyCount.Length-1 && i == currentLevel.rounds[currentRound].enemyCount[currentType] - 1)
                    {
                        enemy.GetComponent<Enemy>().OnDestroyedEvent += () =>
                        {
                            GameMenu.instance.addCoins(currentLevel.rounds[currentRound].roundCompletionBonus);
                        };
                    }

                    yield return new WaitForSeconds(currentLevel.rounds[currentRound].spawnGap);
                }
                yield return new WaitForSeconds(currentLevel.rounds[currentRound].enemyTypeSpawnGap);
            }
            GameObject.FindObjectOfType<GameMenu>().updateRoundDetails(currentRound+2,currentLevel.rounds.Length);
            yield return new WaitForSeconds(currentLevel.rounds[currentRound].spawnGap);
        }
        allRoundsComplete = true;
        yield return null;
    }

    public void initializeGameParameters()
    {
        //yield return new WaitForSeconds(1f);
        GameMenu.Instance.updateRoundDetails(1, currentLevel.rounds.Length);
        GameMenu.Instance.updateHealthCount(currentLevel.startingHealth);
        GameMenu.Instance.updateCoinCount(currentLevel.startingCoins);
        GameMenu.Instance.resetSpeedDetails();
        GameMenu.Instance.LoadTowerAndSpells(currentLevel.allowedTowers);
    }

    // Start is called before the first frame update
    public void StartGame()
    {
        EnemySpawner = StartCoroutine(SpawnRounds());
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
