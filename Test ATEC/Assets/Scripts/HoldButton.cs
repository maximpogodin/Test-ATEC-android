using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public GameObject manager;
    public LongClick lc;

    void Start()
    {
        manager = GameObject.Find("Manager");
        lc = manager.GetComponent<LongClick>();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (!lc.isMenu)
        {
            lc.DoIt();
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        lc.pointerDown = true;
        lc.isMenu = false;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        lc.Reset();
    }
}
