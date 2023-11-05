using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIVirtualTouchZone : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [System.Serializable]
    public class Event : UnityEvent<Vector2>
    {
    }

    [System.Serializable]
    public class ButtonEvent : UnityEvent<bool>
    {
    }

    [Header("Rect References")]
    public RectTransform containerRect;

    public RectTransform handleRect;
    public RectTransform bgRect;

    [Header("Settings")]
    public bool clampToMagnitude;

    public float magnitudeMultiplier = 1f;
    public bool invertXOutputValue;
    public bool invertYOutputValue;
    public Vector2 sensitivityModifiers = Vector2.one;
    public float joystickRange = 100;

    public float longPressDuration = 0.3f;
    public float longPressTimer;

    //Stored Pointer Values
    private Vector2 pointerDownPosition;
    private Vector2 currentPointerPosition;

    [Header("Output")]
    public Event touchZoneOutputEvent;

    public ButtonEvent touchZoneButtonOutputEvent;

    private Vector2 handlePositionStart;
    public bool longPressIsDown;

    void Start()
    {
        SetupHandle();
    }

    private void SetupHandle()
    {
        handlePositionStart = handleRect.anchoredPosition;
        // if (handleRect)
        // {
        //     SetObjectActiveState(handleRect.gameObject, false);
        //     SetObjectActiveState(bgRect.gameObject, false);
        // }
    }

    private void Update()
    {
        if (longPressIsDown)
        {
            longPressTimer += Time.deltaTime;
            if (longPressTimer > longPressDuration)
            {
                OutputButtonEventValue(true);
                longPressIsDown = false;
            }
        }
    }

    public void OnBgRectPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnBgRectPointerDown");
        OutputButtonEventValue(true);
    }

    public void OnBgRectPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnBgRectPointerUp");

        OutputButtonEventValue(false);
        longPressIsDown = false;
        longPressTimer = 0;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position,
            eventData.pressEventCamera, out pointerDownPosition);

        if (handleRect)
        {
            // SetObjectActiveState(handleRect.gameObject, true);
            // SetObjectActiveState(bgRect.gameObject, true);
            UpdateHandleRectPosition(handleRect, pointerDownPosition);
            UpdateHandleRectPosition(bgRect, pointerDownPosition);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position,
            eventData.pressEventCamera, out currentPointerPosition);

        var positionDelta = GetDeltaBetweenPositions(pointerDownPosition, currentPointerPosition);

        var clampedPosition = ClampValuesToMagnitude(positionDelta);

        var invertedPosition = ApplyInversionFilter(clampedPosition);

        var outputPosition = ApplySensitivityFilter(invertedPosition, sensitivityModifiers);

        UpdateHandleRectPosition(handleRect, pointerDownPosition + clampedPosition * joystickRange);
        OutputPointerEventValue(outputPosition * magnitudeMultiplier);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDownPosition = Vector2.zero;
        currentPointerPosition = Vector2.zero;

        OutputButtonEventValue(false);
        longPressIsDown = false;
        longPressTimer = 0;

        OutputPointerEventValue(Vector2.zero);

        if (handleRect)
        {
            // SetObjectActiveState(handleRect.gameObject, false);
            // SetObjectActiveState(bgRect.gameObject, false);
            UpdateHandleRectPosition(handleRect, handlePositionStart);
            UpdateHandleRectPosition(bgRect, handlePositionStart);
        }
    }

    void OutputButtonEventValue(bool isDown)
    {
        Debug.Log($"OutputButtonEventValue {isDown}");
        touchZoneButtonOutputEvent.Invoke(isDown);
    }

    void OutputPointerEventValue(Vector2 pointerPosition)
    {
        touchZoneOutputEvent.Invoke(pointerPosition);
    }

    void UpdateHandleRectPosition(RectTransform handle, Vector2 newPosition)
    {
        handle.anchoredPosition = newPosition;
    }

    void SetObjectActiveState(GameObject targetObject, bool newState)
    {
        targetObject.SetActive(newState);
    }

    Vector2 GetDeltaBetweenPositions(Vector2 firstPosition, Vector2 secondPosition)
    {
        return secondPosition - firstPosition;
    }

    Vector2 ClampValuesToMagnitude(Vector2 position)
    {
        return Vector2.ClampMagnitude(position, joystickRange) / joystickRange;
    }

    Vector2 ApplyInversionFilter(Vector2 position)
    {
        if (invertXOutputValue)
        {
            position.x = InvertValue(position.x);
        }

        if (invertYOutputValue)
        {
            position.y = InvertValue(position.y);
        }

        return position;
    }

    Vector2 ApplySensitivityFilter(Vector2 position, Vector2 sensitivity)
    {
        return new Vector2(
            position.x * sensitivity.x,
            position.y * sensitivity.y);
    }

    float InvertValue(float value)
    {
        return -value;
    }
}