using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour, IPointerEnterHandler
{
    public GameObject selected;

    public void OnPointerEnter(PointerEventData eventData)
    {
    }
}

