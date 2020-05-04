using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnUIChecker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PutObjects putObjects;

    private void Start()
    {
        putObjects = GameObject.FindGameObjectWithTag("GameController").GetComponent<PutObjects>();
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        putObjects.SetMouseOnUI(true);
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        putObjects.SetMouseOnUI(false);
    }
}
