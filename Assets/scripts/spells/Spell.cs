using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : BuyableItem
{
    public int range = 3;
    public float duration = 5.0f;
    public float intensity = 1.0f;
    public bool applied = false;
    float currentTime=0.0f;
    public List<Shooter> shooterList = new List<Shooter>();
    public virtual void applySpell(GameObject tower) { }

    public void OnTriggerStay2D(Collider2D collision)
    {
        /*Debug.Log("[Spell] " + collision.gameObject.name);
        if (collision.gameObject.tag == "tower")
        {
            Debug.Log("[Spell]1 " + collision.gameObject.name);
            if (!shooterList.Contains(collision.gameObject.GetComponent<Shooter>()))
            {
                Debug.Log("[Spell]2 " + collision.gameObject.name);
                shooterList.Add(collision.gameObject.GetComponent<Shooter>());
                applySpell(collision.gameObject);
            }
        }*/
    }


    // Start is called before the first frame update
    public virtual void Start()
    {
        GameObject[] arr = GameObject.FindGameObjectsWithTag("tower");
        Debug.Log("[Spell]"+arr.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            Debug.Log("[Spell]"+Vector3.Distance(transform.position, arr[i].transform.position));
            if(Vector3.Distance(transform.position, arr[i].transform.position) < range)
            {
                applySpell(arr[i]);
                shooterList.Add(arr[i].GetComponent<Shooter>());
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime+=Time.deltaTime;
        if (currentTime > duration)
        {
            Destroy(gameObject);
        }
    }
}
