using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : Spell
{
    public float duration = 3.0f;
    public float timeLapsed = 0.0f;
    List<Enemy> enemyList;
    public override void applySpell(GameObject target)
    {
        Debug.Log("[Agility.applySpell]");
        base.applySpell(target);
        target.GetComponent<Enemy>().m_speedUpdated = 0;
        applied = true;
    }

    public override void Start()
    {
        GameObject[] arr = GameObject.FindGameObjectsWithTag("enemy");
        Debug.Log("[Freeze]" + arr.Length);
        enemyList=new List<Enemy>();
        for (int i = 0; i < arr.Length; i++)
        {
            Debug.Log("[Freeze]" + Vector3.Distance(transform.position, arr[i].transform.position));
            if (Vector3.Distance(transform.position, arr[i].transform.position) < range)
            {
                applySpell(arr[i]);
                enemyList.Add(arr[i].GetComponent<Enemy>());
            }
        }

    }
    public void OnDestroy()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].m_speedUpdated = enemyList[i].m_speed;
        }
    }


    // Update is called once per frame
    void Update()
    {
        timeLapsed+=Time.deltaTime;
        if (timeLapsed > duration)
        {
            Destroy(gameObject);
        }
    }
}
