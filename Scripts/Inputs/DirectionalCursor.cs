using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionalCursor : Singleton<DirectionalCursor>
{

    PlayerInput controls;

    Transform player;
    public Vector2 pos;
    public float range, speed;
    private void Start() {
        player = FindObjectOfType<PlayerMovement>().transform;
        controls = PlayerInputManager.Instance.playerControls;

        controls.Gameplay.CursorPos.performed += ctx => pos = ctx.ReadValue<Vector2>();
    }

    void Update()
    {
        var adjustedPos = new Vector2(pos.x * range * 1.5f, pos.y * range);
        transform.position =Vector2.Lerp(transform.position, (Vector2) player.position + adjustedPos, speed * Time.deltaTime);
    }
    

}
