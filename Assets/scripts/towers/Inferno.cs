using UnityEngine;
using System.Collections;
[RequireComponent(typeof(LineRenderer))]
public class Inferno : Shooter
{
    public float multiplier;
    public float timeBetweenDamageIncrease;
    //float m_damage;
    float timeSinceLastIncrement =0f;
    [SerializeField] private Texture[] textures;
    float remainingTime=10f;
    int currentSpriteIndex = 0;
    [SerializeField] private float timeBetweenSpriteChange;
    Transform prevTarget;

    void drawLine()
    {
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        if(isTargetSet)
            GetComponent<LineRenderer>().SetPosition(1, getTarget().position);
        else
            GetComponent<LineRenderer>().SetPosition(1, transform.position);
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            GetComponent<LineRenderer>().material.SetTexture(1,textures[currentSpriteIndex]);
            currentSpriteIndex=(currentSpriteIndex+1) % textures.Length;
            remainingTime = timeBetweenSpriteChange;
        }

    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        remainingTime = timeBetweenSpriteChange;
        updatedDamage = damage;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        drawLine();
        if (isTargetSet)
        {
            if (prevTarget != target)
            {
                Debug.Log("switching");
                prevTarget = target;
                updatedDamage = damage;
                timeSinceLastIncrement = 0f;
                return;
            }
            if (timeSinceLastIncrement > timeBetweenDamageIncrease)
            {
                updatedDamage *= multiplier;
                timeSinceLastIncrement = 0f;
            }
            timeSinceLastIncrement += Time.deltaTime;
            target.gameObject.GetComponent<Enemy>().DamageEnemy(updatedDamage * Time.deltaTime);
            prevTarget = target;
        }
        else
        {
            updatedDamage = damage;
            timeSinceLastIncrement = 0f;
        }
    }
}
