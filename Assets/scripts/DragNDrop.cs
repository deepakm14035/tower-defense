using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragNDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject towerPrefab;
    public GameObject towerCreationIcon;
    public Text costText;
    GameObject newTower;
    GameMenu gameMenu;
    bool isEmpty = true;
    private Vector3 mouseDownPosition;

    void Start()
    {
        gameMenu = GameObject.FindObjectOfType<GameMenu>();
    }

    bool isSpaceEmpty()
    {
        RaycastHit2D res = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, 20f, 1 << LayerMask.NameToLayer("tower"));
        Debug.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition) - Vector3.forward * 5f, Camera.main.ScreenToWorldPoint(Input.mousePosition) - Vector3.forward * 5f + Vector3.forward * 20f);
        return res.collider == null || 
            res.collider.gameObject.tag.Equals("decelerator") || 
            res.collider.gameObject.tag.Equals("spell") ||
            (tag=="spellIcon" && res.collider.gameObject.tag.Equals("tower"));
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
        var range = 0f;
        if (towerPrefab.GetComponent<Shooter>() != null)
        {
            range = towerPrefab.GetComponent<Shooter>().range;
        }
        else if (towerPrefab.GetComponent<Spell>() != null)
        {
            range = towerPrefab.GetComponent<Spell>().range;
        }
        GameMenu.Instance.SetTowerMoveMode(newTower.transform, range); 
    }
    /*public void OnEndDrag(PointerEventData eventData)
    {
        newTower = null;
    }*/

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("dragging + "+ Input.mousePosition);
        mouseDownPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (newTower==null && gameMenu.getCoinCount()> int.Parse(costText.text))
            newTower = Instantiate(towerCreationIcon, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);


    }
    public void OnPointerUp(PointerEventData eventData)
    {
        var newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!isSpaceEmpty() || Vector3.Distance(mouseDownPosition, newPosition)<1f ) {
            GameObject.Destroy(newTower);
            return;
        }
        //Debug.Log("creating");
        //LevelGenerator uic = GameObject.FindObjectOfType<LevelGenerator>();
        GameObject.Destroy(newTower);
        if (GameMenu.instance.getCoinCount() - towerPrefab.GetComponent<BuyableItem>().cost < 0)
        {
            GameMenu.instance.showCoinAlert();
            return;
        }
        newTower = Instantiate(towerPrefab, newPosition, Quaternion.identity);
        newTower.GetComponent<BuyableItem>().source = FindObjectOfType<SoundManager>();
        newTower.transform.position = new Vector3(newTower.transform.position.x, newTower.transform.position.y, 0f);

        GameMenu.instance.updateCoinCount(GameMenu.instance.getCoinCount()-newTower.GetComponent<BuyableItem>().cost);
        newTower = null;
        GameMenu.Instance.enableDisableItems(GameMenu.ItemType.None, false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameMenu.SetItemInfo(towerPrefab.GetComponent<BuyableItem>());
        gameMenu.SetDetailsVisibility(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameMenu.SetDetailsVisibility(false);
    }
}