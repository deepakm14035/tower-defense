using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MenuManagement;

public class GameMenu : Menu<GameMenu>
{
    enum ItemType
    {
        Main, SellConfirm
    }

    public enum ButtonType
    {
        UpgradeButton, SellButton, ConfirmButton, CancelButton, SelectButton
    }


    [SerializeField]  Button m_buttonSell;
    [SerializeField]  Button m_buttonUpgrade;
    [SerializeField]  Button m_buttonSpecialAbility;

    [SerializeField]  Button m_buttonSellConfirm;
    [SerializeField]  Button m_buttonSellCancel;
    [SerializeField]  Button m_textConfirm;
    [SerializeField]  Text m_textErrorLog;

    [SerializeField]  GameObject m_towerRangeObject;

    [SerializeField] static float m_buttonDistanceFromCenter;



    public GameObject[] towers;
    public GameObject[] spells;
    public GameObject[] placeholderRows;
    public GameObject[] spellPlaceholderRows;
    public Sprite[] towerUpgradeSprites;
    public GameObject upgradePS;
    public GameObject towerEditPanelBG;
    [SerializeField] private Text coinCount;
    [SerializeField] private Text roundText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text gameplaySpeedText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TowerDetails towerDetails;
    [SerializeField] private TowerDetails.Tower selectedTowerDetails;
    [SerializeField] private Shooter selectedTower;
    public static GameMenu instance;

    public int towersPerRow = 4;
    int speed = 0;
    public static GameMenu getInstance()
    {
        return instance;
    }

    public void updateCoinCount(int newVal)
    {
        coinCount.text = newVal + "";
    }

    public int getCoinCount()
    {
        return int.Parse(coinCount.text);
    }

    public void updateHealthCount(int newVal)
    {
        healthText.text = newVal + "";
    }

    public int getHealthCount()
    {
        return int.Parse(healthText.text);
    }
    public void showGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void updateTowerRange(float range, Vector3 position)
    {
        m_towerRangeObject.transform.localScale = new Vector3(range, range, 1f);
        m_towerRangeObject.transform.position = position;
    }

    public void updateRoundDetails(int currRound, int totalRounds)
    {
        roundText.text = currRound + " of "+totalRounds;
        Debug.Log(roundText.text);
    }
    public void updateSpeedDetails()
    {
        speed=(speed+1)%3;
        gameplaySpeedText.text = (speed+1) + " of 3";
        Time.timeScale = 1f + speed * 0.5f;
        Debug.Log(Time.timeScale);
    }

    void placeItems(ItemType item)
    {
        if (selectedTower == null) return;
        Vector3 towerPos = Camera.main.WorldToScreenPoint(selectedTower.gameObject.transform.position);
        if (item == ItemType.Main)
        {
            m_buttonUpgrade.gameObject.GetComponent<RectTransform>().localPosition = towerPos + Vector3.up * m_buttonDistanceFromCenter;
            m_buttonSell.gameObject.GetComponent<RectTransform>().localPosition = towerPos -
                Vector3.right * m_buttonDistanceFromCenter * Mathf.Cos(45f * Mathf.Deg2Rad) +
                Vector3.up * m_buttonDistanceFromCenter * Mathf.Sin(45f * Mathf.Deg2Rad);
            m_buttonSpecialAbility.gameObject.GetComponent<RectTransform>().localPosition = towerPos +
                Vector3.right * m_buttonDistanceFromCenter * Mathf.Cos(45f * Mathf.Deg2Rad) +
                Vector3.up * m_buttonDistanceFromCenter * Mathf.Sin(45f * Mathf.Deg2Rad);

        }
        if (item == ItemType.SellConfirm)
        {
            m_textConfirm.gameObject.GetComponent<RectTransform>().localPosition = towerPos + Vector3.up * m_buttonDistanceFromCenter;
            m_buttonSellCancel.gameObject.GetComponent<RectTransform>().localPosition = towerPos -
                Vector3.right * m_buttonDistanceFromCenter * Mathf.Cos(45f * Mathf.Deg2Rad) +
                Vector3.up * m_buttonDistanceFromCenter * Mathf.Sin(45f * Mathf.Deg2Rad);
            m_buttonSellConfirm.gameObject.GetComponent<RectTransform>().localPosition = towerPos +
                Vector3.right * m_buttonDistanceFromCenter * Mathf.Cos(45f * Mathf.Deg2Rad) +
                Vector3.up * m_buttonDistanceFromCenter * Mathf.Sin(45f * Mathf.Deg2Rad);

        }
        towerEditPanelBG.GetComponent<RectTransform>().localPosition = towerPos + 
            Vector3.up * 40.0f;
    }

    public void sell()
    {
        Debug.Log("selling");
        placeItems(ItemType.SellConfirm);
        enableDisableItems(ItemType.Main, false);
        placeItems(ItemType.SellConfirm);
        enableDisableItems(ItemType.SellConfirm, true);
    }

    void sellConfirm()
    {
        Debug.Log("sell confirm");
        enableDisableItems(ItemType.SellConfirm, false);
        GameObject.Destroy(selectedTower.gameObject);
    }

    void cancel()
    {
        enableDisableItems(ItemType.SellConfirm, false);
        enableDisableItems(ItemType.Main, false);
    }

    IEnumerator clearText(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        m_textErrorLog.text = "";
    }

    public void showCoinAlert()
    {
        m_textErrorLog.text = "Not enough coins!";
        StartCoroutine(clearText(3.0f));
    }

    public void playPlayerHurt()
    {
        Camera.main.gameObject.GetComponent<Animator>().SetTrigger("shakeCamera");
    }

    void levelUp()
    {
        Debug.Log("level up!");
        int currentLevel = selectedTower.currentLevel;
        if (currentLevel >= selectedTowerDetails.levels.Length) return;
        int cost = selectedTowerDetails.levels[currentLevel].cost;
        if(GameMenu.instance.getCoinCount() - cost < 0)
        {
            showCoinAlert();
            return;
        }
        updateCoinCount(GameMenu.instance.getCoinCount() - cost);
        selectedTower.gameObject.GetComponent<SpriteRenderer>().sprite = towerUpgradeSprites[currentLevel];
        selectedTower.upgradeLevel(selectedTowerDetails.levels[currentLevel]);
        updateTowerRange(selectedTower.range, selectedTower.gameObject.transform.position);
        Instantiate(upgradePS, selectedTower.gameObject.transform);
        enableDisableItems(ItemType.SellConfirm, false);
        enableDisableItems(ItemType.Main, false);
    }

    void enableDisableItems(ItemType type, bool val)
    {
        if (type == ItemType.Main)
        {
            m_buttonUpgrade.gameObject.SetActive(val);
            m_buttonSell.gameObject.SetActive(val);
            m_buttonSpecialAbility.gameObject.SetActive(val);
        }
        else if (type == ItemType.SellConfirm)
        {
            Debug.Log(val);
            m_buttonSellConfirm.gameObject.SetActive(val);
            m_buttonSellCancel.gameObject.SetActive(val);
            m_textConfirm.gameObject.SetActive(val);
        }
        m_towerRangeObject.SetActive(val);
        towerEditPanelBG.SetActive(val);
    }

    public void handleClick(ButtonType buttonType)
    {
        if (buttonType == ButtonType.UpgradeButton)
        {
            levelUp();
        }else if (buttonType == ButtonType.SellButton)
        {
            sell();
        }
        else if (buttonType == ButtonType.ConfirmButton)
        {
            sellConfirm();
        }
        else if (buttonType == ButtonType.CancelButton)
        {
            cancel();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
            instance = this;

        m_buttonDistanceFromCenter = 100.0f;
        /*if (m_buttonSell == null)
        {
            m_buttonUpgrade = GameObject.Find("UpgradeButton").GetComponent<Button>();
            m_buttonSell = GameObject.Find("SellButton").GetComponent<Button>();
            m_buttonSpecialAbility = GameObject.Find("SpecialAbilityButton").GetComponent<Button>();
            m_textConfirm = GameObject.Find("TickButton").GetComponent<Button>();
            m_buttonSellConfirm = GameObject.Find("TickButton").GetComponent<Button>();
            m_buttonSellCancel = GameObject.Find("CancelButton").GetComponent<Button>();
            m_towerRangeObject = GameObject.Find("ShooterRange");
        }*/
        m_towerRangeObject = GameObject.Find("ShooterRange");
        enableDisableItems(ItemType.SellConfirm, false);
        enableDisableItems(ItemType.Main, false);
        placeItems(ItemType.Main);
        placeItems(ItemType.SellConfirm);

        if (m_buttonSell != null)
        {
            Debug.Log("sell button listener");
            m_buttonSell.onClick.AddListener(sell);
        }
        /*if (m_buttonUpgrade != null)
            m_buttonUpgrade.onClick.AddListener(levelUp);
        if (m_buttonSpecialAbility != null)
            m_buttonSpecialAbility.onClick.AddListener(sell);
        //if (m_buttonSellConfirm != null)
            m_buttonSellConfirm.onClick.AddListener(sellConfirm);
        if (m_buttonSellCancel != null)
            m_buttonSellCancel.onClick.AddListener(cancel);
            */
        int rowNum = 0;
        for(int i = 0; i < towers.Length; i++)
        {
            GameObject go = Instantiate(towers[i].GetComponent<Shooter>().icon,placeholderRows[rowNum].transform);
            go.GetComponent<DragNDrop>().towerPrefab = towers[i];
            go.GetComponent<DragNDrop>().costText.text = towers[i].GetComponent<Shooter>().cost+"";
            towers[i].GetComponent<Shooter>().towerDetails = towerDetails.towers[i];
            if (i > 0 && (i+1) % towersPerRow == 0)
                rowNum++;
        }

        rowNum = 0;
        for (int i = 0; i < spells.Length; i++)
        {
            GameObject go = Instantiate(spells[i].GetComponent<Spell>().icon, spellPlaceholderRows[rowNum].transform);
            go.GetComponent<DragNDrop>().towerPrefab = spells[i];
            go.GetComponent<DragNDrop>().costText.text = spells[i].GetComponent<Spell>().cost + "";
            //spells[i].GetComponent<Shooter>().towerDetails = towerDetails.towers[i];
            if (i > 0 && (i + 1) % towersPerRow == 0)
                rowNum++;
        }
    }

    public void OnPause()
    {
        MenuManager.Instance.loadMenu(PauseMenu.Instance);
        Time.timeScale = 0.0f;
    }

    public void handleTowerClick(GameObject towerObj)
    {
        selectedTower = towerObj.GetComponent<Shooter>();
        selectedTowerDetails = selectedTower.towerDetails;
        enableDisableItems(ItemType.Main, true);
        placeItems(ItemType.Main);
        updateTowerRange(towerObj.GetComponent<Shooter>().range, towerObj.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("game menu");
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D res = Physics2D.Raycast(position, Vector3.forward, 20f, 1 << LayerMask.NameToLayer("tower"));
            if (res.collider != null && res.collider.gameObject.GetComponent<Shooter>() != null)
            {
                handleTowerClick(res.collider.gameObject);
            }
            else
            {
                enableDisableItems(ItemType.SellConfirm, false);
                enableDisableItems(ItemType.Main, false);
            }
        }
    }
        
}
