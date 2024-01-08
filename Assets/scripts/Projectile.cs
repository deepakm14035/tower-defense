using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public Transform target;
    public float damage;
    public int count;
    public GameObject explosionPrefab;
    public Vector3 displacement;
    public float magnitude;
    public float explosionRange;
    public float rotationSpeed=100.0f;
    public bool destroyOnEnemyDeath = true;
    [SerializeField] public AudioClip shootingSound;

    bool damageModified=false;
    public void onDestroy()
    {
        for(int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(explosionPrefab,transform.position,transform.rotation);
            go.AddComponent<MoveTo>();
            go.GetComponent<MoveTo>().target = transform.position + Vector3.up * Random.RandomRange(-explosionRange, explosionRange) + Vector3.right * Random.RandomRange(-explosionRange, explosionRange);
        }
        GameObject.Destroy(gameObject);
    }

    public void MultiplyDamage(float multiplier)
    {
        if(damageModified) return;
        damage *= multiplier;
        damageModified = true;
        transform.localScale *= multiplier;
    }

    public Transform getEnemyAtEndOfLine()
    {
        Transform newTarget;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        float closestEnemyDistance = 0f;
        GameObject closestEnemy = null;
        for (int i = 0; i < enemies.Length; i++)
        {
            //if (Vector3.Distance(transform.position, enemies[i].transform.position) < closestEnemyDistance)
            if (enemies[i] != null && enemies[i].GetComponent<Enemy>().distanceCovered > closestEnemyDistance)
            {
                closestEnemyDistance = enemies[i].GetComponent<Enemy>().distanceCovered;
                closestEnemy = enemies[i];
            }
        }
        if (closestEnemy != null)
            newTarget = closestEnemy.transform;
        else
            newTarget = null;
        return newTarget;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.identity;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            target = getEnemyAtEndOfLine();
            if (destroyOnEnemyDeath || target == null) {
                GameObject.Destroy(gameObject);
                return;
            }

        }
        Vector3 direction = target.position - transform.position;
        //transform.LookAt(positions.GetPosition(currentIndex),new Vector3(0,0,1f));
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rot_z), Time.deltaTime*rotationSpeed);
        displacement = transform.right * Time.deltaTime * speed;
        magnitude = Vector3.Magnitude(displacement);
        transform.position += transform.right*Time.deltaTime * speed;
    }
}
