using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerClickHandler : MonoBehaviour
{

    [SerializeField] static Button m_buttonSell;
    [SerializeField] static Button m_buttonUpgrade;
    [SerializeField] static Button m_buttonSpecialAbility;

    [SerializeField] static Button m_buttonSellConfirm;
    [SerializeField] static Button m_buttonSellCancel;
    [SerializeField] static Button m_textConfirm;

    [SerializeField] static GameObject m_towerRangeObject;

    [SerializeField] static float m_buttonDistanceFromCenter;


    public TowerDetails.Tower towerDetails;
    int m_noOfButtons;

    enum ItemType
    {
        Main, SellConfirm
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
