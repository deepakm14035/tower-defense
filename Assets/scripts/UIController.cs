using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    enum ItemType
    {
        Main, SellConfirm
    }


    [SerializeField] static Button m_buttonSell;
    [SerializeField] static Button m_buttonUpgrade;
    [SerializeField] static Button m_buttonSpecialAbility;

    [SerializeField] static Button m_buttonSellConfirm;
    [SerializeField] static Button m_buttonSellCancel;
    [SerializeField] static Button m_textConfirm;

    [SerializeField] static GameObject m_towerRangeObject;

    [SerializeField] static float m_buttonDistanceFromCenter;



    public GameObject[] towers;
    public GameObject[] placeholderRows;
    [SerializeField] private Text coinCount;
    [SerializeField] private Text roundText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text gameplaySpeedText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TowerDetails towerDetails;
    [SerializeField] private TowerDetails.Tower selectedTowerDetails;
    [SerializeField] private Shooter selectedTower;
    public static UIController instance;

    GameObject newTower;
    public int towersPerRow = 4;
    int speed = 0;

    public static UIController getInstance()
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
    }

    void sell()
    {

        m_buttonSellConfirm.gameObject.SetActive(true);
        m_buttonSellCancel.gameObject.SetActive(true);
    }

    void sellConfirm()
    {
        enableDisableItems(ItemType.SellConfirm, false);
        GameObject.Destroy(gameObject);
    }

    void cancel()
    {
        enableDisableItems(ItemType.SellConfirm, false);
        enableDisableItems(ItemType.Main, false);
    }

    void levelUp()
    {
        int currentLevel = GetComponent<Shooter>().currentLevel - 1;
        int cost = selectedTowerDetails.levels[currentLevel].cost;
        UIController.instance.updateCoinCount(UIController.instance.getCoinCount() - cost);
        GetComponent<Shooter>().upgradeLevel(selectedTowerDetails.levels[currentLevel]);
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
            m_buttonSellConfirm.gameObject.SetActive(val);
            m_buttonSellCancel.gameObject.SetActive(val);
            m_textConfirm.gameObject.SetActive(val);
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
        if (m_buttonSell == null)
        {
            m_buttonUpgrade = GameObject.Find("UpgradeButton").GetComponent<Button>();
            m_buttonSell = GameObject.Find("SellButton").GetComponent<Button>();
            m_buttonSpecialAbility = GameObject.Find("SpecialAbilityButton").GetComponent<Button>();
            m_textConfirm = GameObject.Find("TickButton").GetComponent<Button>();
            m_buttonSellConfirm = GameObject.Find("TickButton").GetComponent<Button>();
            m_buttonSellCancel = GameObject.Find("CancelButton").GetComponent<Button>();
            m_towerRangeObject = GameObject.Find("ShooterRange");
        }
        enableDisableItems(ItemType.SellConfirm, false);
        enableDisableItems(ItemType.Main, false);
        placeItems(ItemType.Main);
        placeItems(ItemType.SellConfirm);

        if (m_buttonSell != null)
            m_buttonSell.onClick.AddListener(sell);
        if (m_buttonUpgrade != null)
            m_buttonUpgrade.onClick.AddListener(levelUp);
        if (m_buttonSpecialAbility != null)
            m_buttonSpecialAbility.onClick.AddListener(sell);
        if (m_buttonSellConfirm != null)
            m_buttonSellConfirm.onClick.AddListener(sellConfirm);
        if (m_buttonSellCancel != null)
            m_buttonSellCancel.onClick.AddListener(cancel);

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D res = Physics2D.Raycast(position, Vector3.forward, 20f, 1 << LayerMask.NameToLayer("tower"));
            if (res.collider != null && res.collider.gameObject.GetComponent<Shooter>()!=null)
            {
                Debug.Log(res.collider.gameObject);
                selectedTowerDetails = res.collider.gameObject.GetComponent<Shooter>().towerDetails;
                enableDisableItems(ItemType.Main, true);
                placeItems(ItemType.Main);
                m_towerRangeObject.transform.localScale = new Vector3(res.collider.gameObject.GetComponent<Shooter>().range,
                    res.collider.gameObject.GetComponent<Shooter>().range, 1f);
                m_towerRangeObject.transform.position = transform.position;
                selectedTower = res.collider.gameObject.GetComponent<Shooter>();
            }
            else
            {
                //Debug.Log(res.collider.gameObject);
                enableDisableItems(ItemType.Main, false);
                enableDisableItems(ItemType.SellConfirm, false);
            }
        }
    }
        
}
