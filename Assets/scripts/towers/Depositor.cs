using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//collects energy and stays weak for some x seconds. then uses extra power for next x seconds 
public class Depositor : Shooter
{
    float timeStored = 0f;
    bool isStoringTime = false;
    bool energyDeposited = false;
    [SerializeField] private float timeStoreLimit;
    [SerializeField] private float reducePercent;
    [SerializeField] private float increasePercent;
    List<GameObject> nearbyTowers;

    public void startStoreTime()
    {
        if(!energyDeposited)
            isStoringTime = true;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!nearbyTowers.Contains(collision.gameObject) && collision.gameObject.GetComponent<Shooter>()!=null)
        {
            nearbyTowers.Add(collision.gameObject);
        }
    }

    void reduceTowerSpeeds()
    {
        for (int i = 0; i < nearbyTowers.Count; i++)
        {
            nearbyTowers[i].GetComponent<Shooter>().updatedDamage = nearbyTowers[i].GetComponent<Shooter>().damage * reducePercent;
            nearbyTowers[i].GetComponent<Shooter>().updatedTimeBetweenShots = nearbyTowers[i].GetComponent<Shooter>().timeBetweenShots / reducePercent;
        }
    }
    void resetTowerSpeeds()
    {
        for (int i = 0; i < nearbyTowers.Count; i++)
        {
            nearbyTowers[i].GetComponent<Shooter>().updatedDamage = nearbyTowers[i].GetComponent<Shooter>().damage;
            nearbyTowers[i].GetComponent<Shooter>().updatedTimeBetweenShots = nearbyTowers[i].GetComponent<Shooter>().timeBetweenShots;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        nearbyTowers = new List<GameObject>();
        resetTowerSpeeds();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStoringTime)
        {
            reduceTowerSpeeds();
            timeStored += Time.deltaTime;
            if(timeStored >= timeStoreLimit)
            {
                isStoringTime = false;
                energyDeposited = true;
                resetTowerSpeeds();
            }
        }
    }
}
