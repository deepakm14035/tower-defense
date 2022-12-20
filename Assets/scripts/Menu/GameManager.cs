using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //public float _score = 0;
    //public int _currentLevel;
    //public int _currentWorld;
    //public int maxScore;
    float _startHeight = 0f;
    //public GameObject MovingIndicator;
    public LevelGenerator _levelGenerator;
    public MapGenerator _mapGenerator;
    public Text _timerText;
    public Level[] levelList;
    public GameObject winPS;
    public int levelNum;
    //SaveData loadedData;

    bool gameStarted = false;


    void Start()
    {
        _levelGenerator = GameObject.FindObjectOfType<LevelGenerator>();
        _mapGenerator = GameObject.FindObjectOfType<MapGenerator>();
    }

    public void loadGame(int levelNo)
    {
        levelNum = levelNo;
        _mapGenerator.GenerateLevel(levelList[levelNo]);
        StartCoroutine(timerForGameStart(10.0f));
    }

    public void loadGame()
    {
        //levelNum = levelNo;
        _mapGenerator.GenerateLevel(levelList[levelNum]);
        StartCoroutine(timerForGameStart(10.0f));
    }

    public IEnumerator playWinAnim()
    {
        Time.timeScale = 1.0f;
        for (int i = 0; i < 5; i++) {
            Instantiate(winPS, new Vector3(Random.RandomRange(-10.0f, 10.0f), Random.RandomRange(-10.0f, 10.0f),0.0f), Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }
    }
    public int getLevelProgress(int world, int level)
    {
        return 0;
    }

    IEnumerator timerForGameStart(float timer)
    {
        float timePassed = 0.0f;
        _timerText.gameObject.SetActive(true);
        _levelGenerator.setStartCoins();
        while (timePassed < timer)
        {
            timePassed += Time.deltaTime;
            _timerText.text = Mathf.RoundToInt(timer - timePassed) + " secs";
            _timerText.color = new Color(timePassed/timer, 0.5f, 0.5f, 1.0f);
            yield return null;
        }
        _levelGenerator.StartGame();
        _timerText.gameObject.transform.parent.gameObject.SetActive(false);
        gameStarted = true;
    }

    private void Update()
    {
    }

    

}
