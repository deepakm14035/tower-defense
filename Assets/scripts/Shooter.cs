using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject icon;
    [SerializeField] private GameObject barrel;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    public TowerDetails.Tower towerDetails;

    public float range;
    public float damage;
    [SerializeField] private float rotateSpeed;
    public float timeBetweenShots;
    public int cost;
    public int totalCost;
    public int currentLevel=1;
    public bool isTargetSet = false;
    public float updatedDamage;
    public float updatedTimeBetweenShots;
    protected Transform target;
    float timeSinceLastShot = 0f;

    public Transform getTarget()
    {
        return target;
    }

    private void setTarget()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > range)
            {
                target = null;
            }
        }
        if (target == null)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
            float closestEnemyDistance = 0f;
            GameObject closestEnemy=null;
            for(int i = 0; i < enemies.Length; i++)
            {
                //if (Vector3.Distance(transform.position, enemies[i].transform.position) < closestEnemyDistance)
                if (enemies[i]!=null && enemies[i].GetComponent<Enemy>().distanceCovered > closestEnemyDistance && Vector3.Distance(transform.position, enemies[i].transform.position)<range)
                    {
                        closestEnemyDistance = enemies[i].GetComponent<Enemy>().distanceCovered;
                    closestEnemy = enemies[i];
                }
            }
            if (closestEnemy != null)
                target = closestEnemy.transform;
            else
                target = null;
        }
        isTargetSet = target != null;
    }

    public void upgradeLevel(TowerDetails.Upgrades upgrade)
    {
        currentLevel++;
        damage *= upgrade.damageMultiplier;
        updatedDamage *= upgrade.damageMultiplier;
        range *= upgrade.areaMultiplier;
        updatedTimeBetweenShots /= upgrade.speedMultiplier;
        timeBetweenShots /= upgrade.speedMultiplier;
        rotateSpeed *= upgrade.speedMultiplier;
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(transform.position, new Vector3(0f,0f,1f), range);
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        timeSinceLastShot = timeBetweenShots + 1f;
        updatedDamage = damage;
        updatedTimeBetweenShots = timeBetweenShots;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //Debug.Log("in shooter");
        setTarget();
        if (target != null && barrel!=null)
        {
            Vector3 direction = target.position - transform.position;
            float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            barrel.transform.rotation = Quaternion.Lerp(barrel.transform.rotation,Quaternion.Euler(0f,0f,rot_z),Time.deltaTime*rotateSpeed);
            float diff = Mathf.Abs(rot_z - barrel.transform.rotation.eulerAngles.z);
            if (diff > 180f)
                diff=diff - 360f;
            if (diff < 5f && timeSinceLastShot> updatedTimeBetweenShots)
            {
                timeSinceLastShot = 0f;
                if (projectilePrefab != null)
                {
                    GameObject go = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, barrel.transform.rotation);
                    go.GetComponent<Projectile>().target = target;
                    go.GetComponent<Projectile>().damage = updatedDamage;
                }
            }
            timeSinceLastShot += Time.deltaTime;
        }
    }
}
