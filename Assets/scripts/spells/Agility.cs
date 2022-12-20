using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agility : Spell
{
    public float speedIncreasePercent = 0.3f;

    public override void applySpell(GameObject target)
    {
        Debug.Log("[Agility.applySpell]");
        base.applySpell(target);
        target.GetComponent<Shooter>().updatedTimeBetweenShots *= (1.0f -speedIncreasePercent);
        applied = true;
    }
    public void OnDestroy()
    {
        for (int i = 0; i < shooterList.Count; i++)
        {
            shooterList[i].updatedTimeBetweenShots /= (1.0f - speedIncreasePercent);
        }
    }
}
