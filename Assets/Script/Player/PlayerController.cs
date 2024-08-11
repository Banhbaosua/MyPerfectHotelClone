using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInputCtl playerInputCtl;
    [SerializeField] CharacterController characterController;
    [SerializeField] float moveSpeed;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if(playerInputCtl.IsTouchMoving) 
        {
            MovementHandle();
        }
    }

    void MovementHandle()
    {   
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
        var newPos = playerPos + playerInputCtl.GetTouchDirection();
        var direction2D = newPos - playerPos;
        var direction3D = new Vector3(direction2D.x, 0, direction2D.y);
        characterController.Move(moveSpeed * Time.deltaTime * direction3D.normalized);
        LookHandle(direction3D.normalized);
    }

    void LookHandle(Vector3 direction)
    {
        if(direction != Vector3.zero)
            this.transform.rotation = Quaternion.LookRotation(direction);
    }
}
