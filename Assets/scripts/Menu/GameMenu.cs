using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MenuManagement;
using System.Linq;

public class GameMenu : Menu<GameMenu>
{
    public enum ItemType
    {
        Main, SellConfirm, None
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
    [SerializeField] private Text coinUpdateValue;
    [SerializeField] private Text roundText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text gameplaySpeedText;
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemDescription;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TowerDetails towerDetails;
    [SerializeField] private TowerDetails.Tower selectedTowerDetails;
    [SerializeField] private Shooter selectedTower;
    public static GameMenu instance;

    private bool _towerMoveMode;
    private Transform _newTowerTransform;
    private float _newTowerRange;

    public int towersPerRow = 4;
    int speed = 0;
    private bool _stopExistingCoinUpdateDisplay;
    
    public static GameMenu getInstance()
    {
        return instance;
    }

    public void updateCoinCount(int newVal)
    {
        coinCount.text = newVal + "";
    }

    public void addCoins(int newVal)
    {

        if (newVal > 0)
        {
            coinUpdateValue.color = Color.green;
            coinUpdateValue.text = "+"+newVal + "";
        }
        else
        {
            coinUpdateValue.color = Color.red;
            coinUpdateValue.text = newVal + "";
        }

        StartCoroutine(DisplayCoinUpdate());

        coinCount.text = (getCoinCount()+newVal) + "";
    }

    private IEnumerator DisplayCoinUpdate()
    {
        if (coinUpdateValue.enabled)
        {
            _stopExistingCoinUpdateDisplay = true;
            yield return new WaitForSeconds(0.1f);
            _stopExistingCoinUpdateDisplay = false;
        }
        
        coinUpdateValue.enabled = true;
        var timeTaken = 0f;
        while (timeTaken<1f || !_stopExistingCoinUpdateDisplay)
        {
            yield return null;
            timeTaken+=Time.deltaTime;
        }
        coinUpdateValue.enabled = false;
        yield return null;
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

    public void SetTowerMoveMode(Transform newTowerTransform, float newTowerRange)
    {
        _towerMoveMode = true;
        _newTowerTransform = newTowerTransform;
        _newTowerRange = newTowerRange;
        enableDisableItems(ItemType.None, true);
    }

    public void updateTowerRange(float range, Vector3 position)
    {
        if (m_towerRangeObject == null)
        {
            m_towerRangeObject = GameObject.Find("ShooterRange");
        }
        m_towerRangeObject.transform.localScale = new Vector3(range*2f, range*2f, 1f);
        m_towerRangeObject.transform.position = position;
        Debug.Log("m_towerRangeObject- "+position);
    }

    public void updateRoundDetails(int currRound, int totalRounds)
    {
        roundText.text = currRound + " of "+totalRounds;
        Debug.Log(roundText.text);
    }
    public void updateSpeedDetails()
    {
        speed=(speed+1)%6;
        gameplaySpeedText.text = (speed+1) + " of 6";
        Time.timeScale = 1f + speed * 0.5f;
        Debug.Log(Time.timeScale);
    }

    public void resetSpeedDetails()
    {
        speed = 0;
        gameplaySpeedText.text = (speed + 1) + " of 6";
        Time.timeScale = 1f + speed * 0.5f;
        Debug.Log(Time.timeScale);
    }

    void placeItems(ItemType item)
    {
        if (selectedTower == null) return;
        Vector3 towerPos = Camera.main.WorldToScreenPoint(selectedTower.gameObject.transform.position);
        if (item == ItemType.Main)
        {
            m_buttonUpgrade.gameObject.GetComponent<RectTransform>().position = towerPos + Vector3.up * m_buttonDistanceFromCenter;
            m_buttonSell.gameObject.GetComponent<RectTransform>().position = towerPos -
                Vector3.right * m_buttonDistanceFromCenter * Mathf.Cos(30f * Mathf.Deg2Rad) +
                Vector3.up * m_buttonDistanceFromCenter * Mathf.Sin(30f * Mathf.Deg2Rad);
            m_buttonSpecialAbility.gameObject.GetComponent<RectTransform>().position = towerPos +
                Vector3.right * m_buttonDistanceFromCenter * Mathf.Cos(30f * Mathf.Deg2Rad) +
                Vector3.up * m_buttonDistanceFromCenter * Mathf.Sin(30f * Mathf.Deg2Rad);

        }
        if (item == ItemType.SellConfirm)
        {
            m_textConfirm.gameObject.GetComponent<RectTransform>().position = towerPos + Vector3.up * m_buttonDistanceFromCenter;
            m_buttonSellCancel.gameObject.GetComponent<RectTransform>().position = towerPos -
                Vector3.right * m_buttonDistanceFromCenter * Mathf.Cos(30f * Mathf.Deg2Rad) +
                Vector3.up * m_buttonDistanceFromCenter * Mathf.Sin(30f * Mathf.Deg2Rad);
            m_buttonSellConfirm.gameObject.GetComponent<RectTransform>().position = towerPos +
                Vector3.right * m_buttonDistanceFromCenter * Mathf.Cos(30f * Mathf.Deg2Rad) +
                Vector3.up * m_buttonDistanceFromCenter * Mathf.Sin(30f * Mathf.Deg2Rad);

        }
        towerEditPanelBG.GetComponent<RectTransform>().position = towerPos + 
            Vector3.up;
    }

    public void sell()
    {
        Debug.Log("selling");
        placeItems(ItemType.SellConfirm);
        m_buttonSell.gameObject.GetComponentInChildren<Text>().text = getTowerSellingCost() + "";
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

    private int getTowerUpgradeCost()
    {
        int currentLevel = selectedTower.currentLevel;
        if (currentLevel >= selectedTowerDetails.levels.Length) return -1;
        return selectedTowerDetails.levels[currentLevel].cost;
    }

    void levelUp()
    {
        Debug.Log("level up!");
        int currentLevel = selectedTower.currentLevel;
        if (currentLevel >= selectedTowerDetails.levels.Length) return;
        int cost = getTowerUpgradeCost();
        if (cost < 0)
        {
            return;
        }

        if(GameMenu.instance.getCoinCount() - cost < 0)
        {
            showCoinAlert();
            return;
        }
        addCoins(- cost);
        selectedTower.gameObject.GetComponent<SpriteRenderer>().sprite = towerUpgradeSprites[currentLevel];
        selectedTower.upgradeLevel(selectedTowerDetails.levels[currentLevel]);
        updateTowerRange(selectedTower.range, selectedTower.gameObject.transform.position);
        Instantiate(upgradePS, selectedTower.gameObject.transform);
        enableDisableItems(ItemType.SellConfirm, false);
        enableDisableItems(ItemType.Main, false);
    }

    public void enableDisableItems(ItemType type, bool val)
    {
        if (type == ItemType.Main)
        {
            m_buttonUpgrade.gameObject.SetActive(val);
            m_buttonSell.gameObject.SetActive(val);
            m_buttonSpecialAbility.gameObject.SetActive(val);
        towerEditPanelBG.SetActive(val);
        }
        else if (type == ItemType.SellConfirm)
        {
            Debug.Log(val);
            m_buttonSellConfirm.gameObject.SetActive(val);
            m_buttonSellCancel.gameObject.SetActive(val);
            m_textConfirm.gameObject.SetActive(val);
            towerEditPanelBG.SetActive(val);
        }
        else
        {
            enableDisableItems(ItemType.Main, false);
            enableDisableItems(ItemType.SellConfirm, false);
        }
        m_towerRangeObject.SetActive(val);
    }

    private int getTowerSellingCost()
    {
        int currentTowerLevel = selectedTower.currentLevel;
        int buyingCost = selectedTower.cost;
        for(int i=0;i< selectedTowerDetails.levels.Length; i++)
        {
            if (i >= currentTowerLevel) break;
            buyingCost += selectedTowerDetails.levels[i].cost;
        }
        return buyingCost/2;
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

        m_buttonDistanceFromCenter = 60;
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
    }

    public void LoadTowerAndSpells(int[] availableTowers)
    {
        //clear existing tower and spell data in UI

        for(int i = 0; i < placeholderRows.Length; i++)
        {
            var children = placeholderRows[i].GetComponentsInChildren<Transform>().ToList();
            children.RemoveAt(0);
            foreach (var child in children)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < spellPlaceholderRows.Length; i++)
        {
            var children = spellPlaceholderRows[i].GetComponentsInChildren<Transform>().ToList();
            children.RemoveAt(0);
            foreach (var child in children)
            {
                GameObject.Destroy(child.gameObject);
            }
        }


        int rowNum = 0;
        var allowedTowers = towers.Where((tower, index) => availableTowers.Contains(index)).ToList();
        for (int i = 0; i < allowedTowers.Count; i++)
        {
            GameObject go = Instantiate(towers[i].GetComponent<Shooter>().icon, placeholderRows[rowNum].transform);
            go.GetComponentInChildren<DragNDrop>().towerPrefab = allowedTowers[i];
            go.GetComponentInChildren<DragNDrop>().costText.text = allowedTowers[i].GetComponent<Shooter>().cost + "";
            towers[i].GetComponent<Shooter>().towerDetails = towerDetails.towers[i];
            if (i > 0 && (i + 1) % towersPerRow == 0)
                rowNum++;
        }

        rowNum = 0;
        for (int i = 0; i < spells.Length; i++)
        {
            GameObject go = Instantiate(spells[i].GetComponent<Spell>().icon, spellPlaceholderRows[rowNum].transform);
            go.GetComponentInChildren<DragNDrop>().towerPrefab = spells[i];
            go.GetComponentInChildren<DragNDrop>().costText.text = spells[i].GetComponent<Spell>().cost + "";
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
        m_buttonUpgrade.gameObject.GetComponentInChildren<Text>().text = getTowerUpgradeCost() + "";
        m_buttonSell.gameObject.GetComponentInChildren<Text>().text = getTowerSellingCost() + "";
        enableDisableItems(ItemType.Main, true);
        placeItems(ItemType.Main);
        updateTowerRange(towerObj.GetComponent<Shooter>().range, towerObj.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("game menu");
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D res = Physics2D.Raycast(position, Vector3.forward, 20f, 1 << LayerMask.NameToLayer("tower") | 1 << LayerMask.NameToLayer("towerDrag"));
            if (res.collider != null && res.collider.gameObject.GetComponent<Shooter>() != null)
            {
                if (_towerMoveMode)
                {
                    _towerMoveMode = false;
                    _newTowerTransform = null;
                    return;
                }
                handleTowerClick(res.collider.gameObject);
            }
            else
            {
                enableDisableItems(ItemType.SellConfirm, false);
                enableDisableItems(ItemType.Main, false);
            }
        }

        if (_towerMoveMode)
        {
            updateTowerRange(_newTowerRange, _newTowerTransform.position);
        }
    }

    public void SetItemInfo(BuyableItem item)
    {
        itemName.text = item.name;
        itemDescription.text = item.description;
    }

    public void SetDetailsVisibility(bool value)
    {
        itemName.enabled = value;
        itemDescription.enabled = value;
    }

    public void ResetGame()
    {
        speed = 0;
        gameOverPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
        
}
