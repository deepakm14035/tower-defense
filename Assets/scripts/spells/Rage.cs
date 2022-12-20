using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rage : Spell
{
    public float damageIncreasePercent = 0.3f;

    public override void applySpell(GameObject target)
    {
        Debug.Log("[Rage.applySpell]");
        base.applySpell(target);
        target.GetComponent<Shooter>().updatedDamage *= (1 + damageIncreasePercent);
        target.transform.localScale = Vector3.one*1.5f;
        applied = true;
    }
    public void OnDestroy()
    {
        for (int i = 0; i < shooterList.Count; i++)
        {
            shooterList[i].updatedDamage = shooterList[i].damage;
        }
    }
}
