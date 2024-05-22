using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerInputManager : Singleton<PlayerInputManager>
{
    public PlayerInput playerControls;

    void Awake()
    {
        playerControls = new PlayerInput();
        playerControls.Enable();
    }

    public void DisableGameplayInputs()
    {
        playerControls.Gameplay.Disable();
    }

    public void DisableMenuInputs()
    {
        playerControls.Menus.Disable();
    }

    public void EnableMenuInputs()
    {
        playerControls.Menus.Disable();
    }


    public void EnableGameplayInputs()
    {
        playerControls.Gameplay.Enable();
    }

    public void DisableCursor(){
        playerControls.Gameplay.CursorPos.Disable();
    }
    public void EnableCursor(){
        playerControls.Gameplay.CursorPos.Enable();
    }

}
