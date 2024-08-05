using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
    public bool showHandleWhenNotInUse = true;

    public bool showBgWhenNotInUse = true;
    public bool absoluteAiming;

    public bool clampToMagnitude;

    public float magnitudeMultiplier = 1f;
    public float absoluteMagnitudeMultiplier = 10f;
    public bool invertXOutputValue;
    public bool invertYOutputValue;
    public Vector2 sensitivityModifiers = Vector2.one;
    public float joystickRange = 100;

    public float longPressDuration = 0.3f;
    public float longPressTimer;

    //Stored Pointer Values
    private Vector2 pointerDownPosition;
    private Vector2 currentPointerPosition;
    private Vector2? lastOutputPosition;

    [Header("Output")]
    public Event touchZoneOutputEvent;

    public ButtonEvent touchZoneButtonOutputEvent;
    public ButtonEvent touchZoneLongPressOutputEvent;

    private Vector2 handlePositionStart;
    public bool longPressIsDown;

    public TMP_Text pointerPositionDebugLabel;

    void Start()
    {
        SetupHandle();
        if (pointerPositionDebugLabel) pointerPositionDebugLabel.text = "";
    }

    private void SetupHandle()
    {
        handlePositionStart = handleRect.anchoredPosition;
    }

    private void Update()
    {
        if (longPressIsDown)
        {
            longPressTimer += Time.deltaTime;
            if (longPressTimer > longPressDuration)
            {
                OutputLongPressEventValue(true);
                longPressIsDown = false;
            }
        }
    }

    // public void OnBgRectPointerDown(PointerEventData eventData)
    // {
    //     Debug.Log("OnBgRectPointerDown");
    //     OutputButtonEventValue(true);
    // }

    // public void OnBgRectPointerUp(PointerEventData eventData)
    // {
    //     Debug.Log("OnBgRectPointerUp");

    //     OutputButtonEventValue(false);
    //     longPressIsDown = false;
    //     longPressTimer = 0;
    // }


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position,
            eventData.pressEventCamera, out pointerDownPosition);
        Debug.Log($"pointerDownPosition: {pointerDownPosition.ToString()}");

        if (pointerPositionDebugLabel) pointerPositionDebugLabel.text = "1 " + pointerDownPosition.ToString();
        lastOutputPosition = null;

        if (handleRect)
        {
            // SetObjectActiveState(handleRect.gameObject, true);
            // SetObjectActiveState(bgRect.gameObject, true);
            UpdateHandleRectPosition(handleRect, pointerDownPosition, true);
            UpdateHandleRectPosition(bgRect, pointerDownPosition, true);
        }

        if (Vector2.Distance(pointerDownPosition, handlePositionStart) < joystickRange)
        {
            longPressIsDown = true;
            OutputButtonEventValue(true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position,
            eventData.pressEventCamera, out currentPointerPosition);

        if (pointerPositionDebugLabel) pointerPositionDebugLabel.text = "D " + currentPointerPosition.ToString();

        var positionDelta = GetDeltaBetweenPositions(pointerDownPosition, currentPointerPosition);

        var clampedPosition = absoluteAiming ? positionDelta : ClampValuesToMagnitude(positionDelta);

        var invertedPosition = ApplyInversionFilter(clampedPosition / joystickRange);

        var outputPosition = ApplySensitivityFilter(invertedPosition, sensitivityModifiers);

        lastOutputPosition ??= outputPosition;

        UpdateHandleRectPosition(handleRect, pointerDownPosition + clampedPosition,
            true);
        OutputPointerEventValue(
            absoluteAiming
                ? (outputPosition - lastOutputPosition.Value) * absoluteMagnitudeMultiplier
                : outputPosition * magnitudeMultiplier
        );
        lastOutputPosition = outputPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDownPosition = Vector2.zero;
        currentPointerPosition = Vector2.zero;

        OutputLongPressEventValue(false);
        OutputButtonEventValue(false);
        longPressIsDown = false;
        longPressTimer = 0;

        OutputPointerEventValue(Vector2.zero);

        if (handleRect)
        {
            // SetObjectActiveState(handleRect.gameObject, false);
            // SetObjectActiveState(bgRect.gameObject, false);
            UpdateHandleRectPosition(handleRect, handlePositionStart, showHandleWhenNotInUse);
            UpdateHandleRectPosition(bgRect, handlePositionStart, showBgWhenNotInUse);
        }
    }

    void OutputButtonEventValue(bool isDown)
    {
        Debug.Log($"OutputButtonEventValue {isDown}");
        touchZoneButtonOutputEvent.Invoke(isDown);
    }

    void OutputLongPressEventValue(bool isDown)
    {
        Debug.Log($"OutputLongPressEventValue {isDown}");
        touchZoneLongPressOutputEvent.Invoke(isDown);
    }

    void OutputPointerEventValue(Vector2 pointerPosition)
    {
        touchZoneOutputEvent.Invoke(pointerPosition);
    }

    void UpdateHandleRectPosition(RectTransform handle, Vector2 newPosition, bool showHandle)
    {
        handle.gameObject.SetActive(showHandle);
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
        return Vector2.ClampMagnitude(position, joystickRange);
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