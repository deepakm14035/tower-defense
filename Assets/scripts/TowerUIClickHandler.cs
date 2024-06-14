using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerUIClickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    GameMenu _gameMenu;

    public GameMenu.ButtonType buttonType;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_gameMenu == null)
        {
            _gameMenu = FindObjectOfType<GameMenu>();
        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_gameMenu == null)
        {
            _gameMenu = FindObjectOfType<GameMenu>();
        }
        _gameMenu.handleClick(buttonType);

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
