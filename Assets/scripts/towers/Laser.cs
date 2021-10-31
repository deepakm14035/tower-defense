using UnityEngine;
using System.Collections;
[RequireComponent(typeof(LineRenderer))]
public class Laser : Shooter
{
    void drawLine()
    {
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        if (isTargetSet)
            GetComponent<LineRenderer>().SetPosition(1, getTarget().position+(getTarget().position-transform.position).normalized*100f);
        else
            GetComponent<LineRenderer>().SetPosition(1, transform.position);
    }

    void attack()
    {
        RaycastHit2D[] arr= Physics2D.RaycastAll(transform.position, getTarget().position - transform.position);
        for(int i = 0; i < arr.Length; i++)
        {
            if(arr[i].transform.gameObject.GetComponent<Enemy>()!=null)
                arr[i].transform.gameObject.GetComponent<Enemy>().DamageEnemy(updatedDamage*Time.deltaTime);
        }
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        drawLine();
        if(isTargetSet)
            attack();
    }
}
