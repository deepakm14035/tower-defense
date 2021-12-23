using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragNDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject towerPrefab;
    public GameObject towerCreationIcon;
    public Text costText;
    GameObject newTower;
    GameMenu uiController;
    bool isEmpty = true;

    void Start()
    {
        uiController = GameObject.FindObjectOfType<GameMenu>();
    }

    bool isSpaceEmpty()
    {
        RaycastHit2D res = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 20f, 1 << LayerMask.NameToLayer("tower"));
        Debug.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition) - Vector3.forward * 5f, Camera.main.ScreenToWorldPoint(Input.mousePosition) - Vector3.forward * 5f + Vector3.forward * 20f);
        return res.collider == null || res.collider.gameObject.tag.Equals("decelerator");
    }

    void changeColor(Vector3 color)
    {
        SpriteRenderer[] go = newTower.GetComponentsInChildren<SpriteRenderer>();
        for(int i = 0; i < go.Length; i++)
        {
            go[i].color = new Color(go[i].color.r*color.x, go[i].color.g * color.y, go[i].color.b * color.z, go[i].color.a);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (newTower == null) return;
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition) - Vector3.forward * 5f);
        if(isSpaceEmpty()!= isEmpty)
        {
            isEmpty = isSpaceEmpty();
            if (isEmpty)
                changeColor(new Vector3(1/1.3f, 1/0.8f, 1/0.8f));
            else
                changeColor(new Vector3(1.3f,0.8f,0.8f));
        }
        newTower.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newTower.transform.position = new Vector3(newTower.transform.position.x, newTower.transform.position.y, 0f);
    }
    /*public void OnEndDrag(PointerEventData eventData)
    {
        newTower = null;
    }*/

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("dragging + "+ Input.mousePosition);
        if(newTower==null && uiController.getCoinCount()> int.Parse(costText.text))
            newTower = Instantiate(towerCreationIcon, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isSpaceEmpty()) {
            GameObject.Destroy(newTower);
            return;
        }
        //Debug.Log("creating");
        //LevelGenerator uic = GameObject.FindObjectOfType<LevelGenerator>();
        GameObject.Destroy(newTower);
        if (GameMenu.instance.getCoinCount() - towerPrefab.GetComponent<Shooter>().cost < 0)
        {
            GameMenu.instance.showCoinAlert();
            return;
        }
        newTower = Instantiate(towerPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition),Quaternion.identity);
        newTower.GetComponent<Shooter>().source = FindObjectOfType<SoundManager>();
        newTower.transform.position = new Vector3(newTower.transform.position.x, newTower.transform.position.y, 0f);

        GameMenu.instance.updateCoinCount(GameMenu.instance.getCoinCount()-newTower.GetComponent<Shooter>().cost);
        newTower = null;
    }
}