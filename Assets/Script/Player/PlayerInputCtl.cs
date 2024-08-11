using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using EnhanceTouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerInputCtl : MonoBehaviour
{
    private Vector2 startVector;
    private Vector2 endVector;
    private bool isTouchMoving;
    public bool IsTouchMoving => isTouchMoving;
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();

        EnhanceTouch.Touch.onFingerDown += Touch_onFingerDown;
        EnhanceTouch.Touch.onFingerMove += Touch_onFingerMove;
        EnhanceTouch.Touch.onFingerUp += Touch_onFingerUp;
    }

    private void Touch_onFingerUp(Finger obj)
    {
        startVector = Vector2.zero; 
        endVector = Vector2.zero;
        isTouchMoving = false;
    }

    private void Touch_onFingerMove(Finger obj)
    {
        endVector = obj.currentTouch.screenPosition;
        isTouchMoving = true;
    }

    private void Touch_onFingerDown(Finger obj)
    {
        startVector = obj.screenPosition;
    }

    private void OnDisable()
    {
        EnhanceTouch.Touch.onFingerDown -= Touch_onFingerDown;
        EnhanceTouch.Touch.onFingerMove -= Touch_onFingerMove;
        EnhanceTouch.Touch.onFingerUp -= Touch_onFingerUp;
        EnhancedTouchSupport.Disable();
    }

    public Vector2 GetTouchDirection()
    {
        return (endVector - startVector).normalized;
    }
}
