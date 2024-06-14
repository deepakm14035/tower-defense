using MenuManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionMenu1 : Menu<LevelSelectionMenu1>
{
    [SerializeField]
    private GameObject _levelButtonPrefab;
    [SerializeField]
    private GameObject _levelListPanel;
    //[SerializeField]
    //private GameObject[] _levelListRows;
    [SerializeField]
    //private Text _worldName;

    int currentPageNo = 0;
    int noOfPages=1;
    private int _buttonsPerRow = 4;

    void generateLevelSelecter(int worldNo)
    {
        clearButtons();
        //LevelGenerator levelGenerator = GameObject.FindObjectOfType<LevelGenerator>();
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        Level[] levels = gameManager.levelList;
        noOfPages = levels.Length / 9;
        //_worldName.text = levels.getLevelsName();
        int rowNo = 0;
        for (int i = 0; i < levels.Length; i++)
        {
            Debug.Log("rowno-" + rowNo);
            Button newButton = Instantiate(_levelButtonPrefab, _levelListPanel.transform).GetComponent<Button>();
            Debug.Log(newButton);
            newButton.GetComponentInChildren<Text>().text = "" + (i + 1);
            addEventListener(newButton, worldNo, i, 1);// gameManager.getLevelProgress(worldNo, i));
            if (gameManager.getLevelProgress(worldNo, i) == 2)
                newButton.gameObject.GetComponent<Image>().color = new Color32(50, 190, 88, 255);
            if (gameManager.getLevelProgress(worldNo, i) == 0)
            {
                //newButton.gameObject.GetComponent<Image>().color = new Color32(190, 50, 88, 255);
                //newButton.gameObject.GetComponentInChildren<Text>().color = new Color32(160, 160, 160, 255);
            }
            //newButton.gameObject.GetComponent<Animator>().SetTrigger("fadein");
            if (i == (rowNo * _buttonsPerRow + (_buttonsPerRow - 1)))
                rowNo++;
        }
        currentPageNo = worldNo;
    }

    void addEventListener(Button button, int worldNo, int i, int type)
    {
        if (type == 1 || type == 2)
            button.onClick.AddListener(delegate {
                GameObject.FindObjectOfType<MenuManager>().loadMenu(GameMenu.Instance);
                Debug.Log("w" + worldNo + ", i-" + i);
                MenuManager menuManager = GameObject.FindObjectOfType<MenuManager>();
                menuManager.loadMenu(GameMenu.Instance, 1f, false);
                GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
                gameManager.loadGame(i);
            });

    }

    void clearButtons()
    {
        /*for (int i = 0; i < _levelListPanel.GetComponentsInChildren<GameObject>().Length; i++)
        {
            int len = _levelListPanel.GetComponentsInChildren<GameObject>()[i].GetComponentsInChildren<Button>().Length;
            for (int j = 0; j < len; j++)
            {
                GameObject.Destroy(_levelListPanel.GetComponentsInChildren<GameObject>()[i].GetComponentsInChildren<Button>()[j].gameObject);
            }
        }*/
    }

    public void nextList()
    {
        if (checkWorldNo(currentPageNo + 1))
            currentPageNo++;
        Debug.Log("currworl-" + currentPageNo);
        StartCoroutine(fadeThenNextList());
        //generateLevelSelecter(currentWorldNo);
    }
    IEnumerator fadeThenNextList()
    {
        /*for (int i = 0; i < _levelListRows.Length; i++)
        {
            Button[] rowsButtons = _levelListRows[i].GetComponentsInChildren<Button>();
            int len = rowsButtons.Length;
            for (int j = 0; j < len; j++)
            {
                rowsButtons[j].gameObject.GetComponent<Animator>().SetTrigger("fadeout");
            }
        }*/
        yield return new WaitForSeconds(0.33f);
        generateLevelSelecter(currentPageNo);
        yield return null;
    }
    public void prevList()
    {
        //generateLevelSelecter(currentWorldNo);
        StartCoroutine(fadeThenNextList());
    }

    bool checkWorldNo(int num)
    {
        //LevelGenerator levelGenerator = GameObject.FindObjectOfType<LevelGenerator>();
        Debug.Log(num < 0 || num >= noOfPages);
        if (num < 0 || num >= noOfPages)
            return false;
        return true;

    }
    // Start is called before the first frame update
    public void setup()
    {
        generateLevelSelecter(0);
    }

}
