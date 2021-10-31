using UnityEngine;
using System.Collections;

public class SelfHealer : Enemy
{
    [SerializeField] float timeForHeal;

    [SerializeField] float healAmount;
    float timer = 0f;

    // Use this for initialization
    void Start()
    {
        timer = timeForHeal;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_health < maxHealth - 1f)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                m_health = Mathf.Clamp(m_health + healAmount, 0f, maxHealth);
            }
        }
        else
            timer = timeForHeal;
    }
}
