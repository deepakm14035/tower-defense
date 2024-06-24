using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //LineRenderer positions;
    [SerializeField] int m_currentIndex = 0;
    public float m_speed;
    [SerializeField] private Renderer m_healthBar;
    [SerializeField] private GameObject m_onDestroyPS;
    [SerializeField] public SoundManager source;
    [SerializeField] public AudioClip dyingSound;
    public float m_speedUpdated;
    public float m_health;
    [SerializeField] private int m_coinReward;
    [SerializeField] private int m_damage;
    GameMenu m_UIControllerObj;
    public float distanceCovered;
    public float maxHealth;
    public delegate void OnDestroyedDelegate();
    public event OnDestroyedDelegate OnDestroyedEvent;
    static float totalLength = 0;

     void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("triggering");
        //Debug.Log(gameObject.name+", "+collision.gameObject.name);
        if (collision.gameObject.tag.Equals("projectile"))
        {
            DamageEnemy(collision.gameObject.GetComponent<Projectile>().damage);
            if (collision.gameObject.GetComponent<Projectile>().shootingSound != null)
                source.playAudio(collision.gameObject.GetComponent<Projectile>().shootingSound,0.3f);
            
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
        //Debug.Log(m_health+", "+maxHealth);
        m_healthBar.material.SetFloat("_remainingHealth", m_health / maxHealth);
        if (m_health <= 0f)
        {
            if (m_UIControllerObj == null)
                m_UIControllerObj = GameObject.FindObjectOfType<GameMenu>();

            m_UIControllerObj.addCoins(m_coinReward);
            //Instantiate(m_onDestroyPS, transform.position, Quaternion.Euler(90f,0f,0f));
            OnDestroyedEvent?.Invoke();
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
        m_UIControllerObj = GameObject.FindObjectOfType<GameMenu>();
        distanceCovered = 0f;
        maxHealth = m_health;
        //positions = GameObject.FindGameObjectWithTag("path").GetComponent<LineRenderer>();
        Vector3[] arr = FindObjectOfType<LevelGenerator>().currentLevel.path;
        m_speedUpdated = m_speed;
        if (totalLength == 0f)
        {
            for(int i = 0; i < arr.Length-1; i++)
            {
                totalLength += Vector3.Distance(arr[i], arr[i+1]);
            }
        }
    }
    public Vector3 getMovementDirection()
    {
        Vector3[] arr = FindObjectOfType<LevelGenerator>().currentLevel.path;
        if(m_currentIndex>=arr.Length) m_currentIndex=arr.Length-1;
        return Vector3.Normalize( arr[m_currentIndex] - transform.position);
    }

    public void SetCurrentPathIndex(int newIndex)
    {
        m_currentIndex = newIndex;
    }
    public int getCurrentPathIndex()
    {
        return m_currentIndex;
    }

    private void GameLost()
    {
        m_UIControllerObj.showGameOver();

        MenuManagement.MenuManager.Instance.loadMenu(MenuManagement.LoseMenu.Instance);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] arr = FindObjectOfType<LevelGenerator>().currentLevel.path;
        if (m_currentIndex < arr.Length)
        {
            if (Vector3.Distance(transform.position, arr[m_currentIndex]) < 0.01f)
            {
                m_currentIndex++;
                if (m_currentIndex == arr.Length)
                {
                    if(m_UIControllerObj==null)
                        m_UIControllerObj = GameObject.FindObjectOfType<GameMenu>();

                    m_UIControllerObj.updateHealthCount(m_UIControllerObj.getHealthCount()- m_damage);
                    GameMenu.instance.playPlayerHurt();
                    if (m_UIControllerObj.getHealthCount() <= 0)
                    {
                        GameLost();
                    }
                    GameObject.Find("Audio Source").GetComponent<SoundManager>().playAudio(dyingSound,0.3f);
                    OnDestroyedEvent?.Invoke();
                    GameObject.Destroy(gameObject);
                    LevelGenerator lg = FindObjectOfType<LevelGenerator>();
                    Debug.Log("checking - "+ lg.allRoundsComplete);
                    
                    return;
                }
                Vector3 direction = arr[m_currentIndex] - transform.position;
                //transform.LookAt(positions.GetPosition(currentIndex),new Vector3(0,0,1f));
                float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
            }

            if (Vector3.Distance(transform.position, arr[m_currentIndex]) >= 0.01f)
            {
                float distance = (Vector3.Normalize(arr[m_currentIndex] - transform.position) * Time.deltaTime * m_speedUpdated).magnitude;
                if (distance >= Vector3.Distance(transform.position, arr[m_currentIndex]))
                    transform.position = arr[m_currentIndex];
                else
                    transform.position += Vector3.Normalize(arr[m_currentIndex] - transform.position) * Time.deltaTime * m_speedUpdated;
                distanceCovered += Time.deltaTime * m_speedUpdated;
            }
        }
    }
}
