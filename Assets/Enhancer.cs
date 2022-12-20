using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhancer : Shooter
{
    public float damageMultiplier=2.0f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("projectile"))
        {
            collision.gameObject.GetComponent<Projectile>().MultiplyDamage(damageMultiplier);
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
