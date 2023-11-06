using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIVirtualTouchZoneJoystickBg : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // public UIVirtualTouchZone touchZone;

    public void OnPointerDown(PointerEventData eventData)
    {
    //     if (touchZone)
    //     {
    //         touchZone.OnBgRectPointerDown(eventData);
    //         touchZone.OnPointerDown(eventData);
    //     }
    }
    
    
    public void OnPointerUp(PointerEventData eventData)
    {
    //     if (touchZone)
    //     {
    //         touchZone.OnBgRectPointerUp(eventData);
    //         touchZone.OnPointerUp(eventData);
    //     }
    }
}
