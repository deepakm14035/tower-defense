using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : Spell
{
    public float damageAmountPerSecond=10;
    Enemy enemy;
    public override void applySpell(GameObject target)
    {
        base.applySpell(target);
        target.GetComponent<Enemy>().DamageEnemy(damageAmountPerSecond);
        enemy = target.GetComponent<Enemy>();
        StartCoroutine(damageEnemy());
    }

    IEnumerator damageEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            enemy.DamageEnemy(damageAmountPerSecond);
        }
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
