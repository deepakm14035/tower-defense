using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    LineRenderer positions;
    [SerializeField] int m_currentIndex = 0;
    [SerializeField] private float m_speed;
    [SerializeField] private Renderer m_healthBar;
    [SerializeField] private GameObject m_onDestroyPS;
    public float m_speedUpdated;
    public float m_health;
    [SerializeField] private int m_coinReward;
    [SerializeField] private int m_damage;
    UIController m_UIControllerObj;
    public float distanceCovered;
    public float maxHealth;
    static float totalLength = 0;

     void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("triggering");
        Debug.Log(gameObject.name+", "+collision.gameObject.name);
        if (collision.gameObject.tag.Equals("projectile"))
        {
            DamageEnemy(collision.gameObject.GetComponent<Projectile>().damage);
            collision.gameObject.GetComponent<Projectile>().onDestroy();
        }
        if (collision.gameObject.tag.Equals("decelerator"))
        {
            m_speedUpdated = m_speed * collision.gameObject.transform.parent.gameObject.GetComponent<Decelerator>().mSlowPercentage/100f;
        }
    }

    public void DamageEnemy(float damage)
    {
        m_health -= damage;
        Debug.Log(m_health+", "+maxHealth);
        m_healthBar.material.SetFloat("_remainingHealth", m_health / maxHealth);
        if (m_health <= 0f)
        {
            m_UIControllerObj.updateCoinCount(m_UIControllerObj.getCoinCount() + m_coinReward);
            //Instantiate(m_onDestroyPS, transform.position, Quaternion.Euler(90f,0f,0f));
            GameObject.Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("decelerator"))
        {
            m_speedUpdated = m_speed;// * collision.gameObject.GetComponent<Decelerator>().mSlowPercentage;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_UIControllerObj = GameObject.FindObjectOfType<UIController>();
        distanceCovered = 0f;
        maxHealth = m_health;
        positions = GameObject.FindGameObjectWithTag("path").GetComponent<LineRenderer>();
        m_speedUpdated = m_speed;
        if (totalLength == 0f)
        {
            for(int i = 0; i < positions.positionCount-1; i++)
            {
                totalLength += Vector3.Distance(positions.GetPosition(i), positions.GetPosition(i+1));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_currentIndex < positions.positionCount)
        {
            if (Vector3.Distance(transform.position, positions.GetPosition(m_currentIndex)) < 0.01f)
            {
                m_currentIndex++;
                if (m_currentIndex == positions.positionCount)
                {
                    m_UIControllerObj.updateHealthCount(m_UIControllerObj.getHealthCount()- m_damage);
                    if (m_UIControllerObj.getHealthCount() <= 0)
                    {
                        m_UIControllerObj.showGameOver();
                    }
                    GameObject.Destroy(gameObject);
                    return;
                }
                Vector3 direction = positions.GetPosition(m_currentIndex) - transform.position;
                //transform.LookAt(positions.GetPosition(currentIndex),new Vector3(0,0,1f));
                float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
            }

            if (Vector3.Distance(transform.position, positions.GetPosition(m_currentIndex)) >= 0.01f)
            {
                float distance = (Vector3.Normalize(positions.GetPosition(m_currentIndex) - transform.position) * Time.deltaTime * m_speedUpdated).magnitude;
                if (distance >= Vector3.Distance(transform.position, positions.GetPosition(m_currentIndex)))
                    transform.position = positions.GetPosition(m_currentIndex);
                else
                    transform.position += Vector3.Normalize(positions.GetPosition(m_currentIndex) - transform.position) * Time.deltaTime * m_speedUpdated;
                distanceCovered += Time.deltaTime * m_speedUpdated;
            }
        }
    }
}
