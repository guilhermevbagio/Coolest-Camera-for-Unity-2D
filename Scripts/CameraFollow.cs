using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // jogador
    public float followSpeed = 3f; // velocidade de seguir o jogador
    public float cameraDistance = 10f; // distância da câmera em relação ao jogador

    bool lockOnMouse;
    Transform secondaryTarget;
    DirectionalCursor cursor;
    Vector3 secondaryPosition, cursorPosition, targetPosition;
    Transform Player;

    private void Start() {
        cursor = DirectionalCursor.Instance;
        Player = target;
    }

    private void FixedUpdate()
    {
        // posição da câmera em relação ao jogador
        if(target != null) {
            targetPosition = target.position + new Vector3(0f, 0f, -cameraDistance);
        }
        else{
            target = Player;
            lockOnMouse = true;
        }

        
        cursorPosition = cursor.pos;

        if(lockOnMouse || secondaryTarget == null){
            // posição do mouse no mundo do jogo
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(cursorPosition);
            secondaryPosition = mousePosition;
        }
        else secondaryPosition = secondaryTarget.position;

        secondaryPosition = new Vector3(secondaryPosition.x, secondaryPosition.y, -cameraDistance);

        // ajusta a posição da câmera de acordo com a posição do mouse
        Vector3 adjustedPosition = Vector3.Lerp(targetPosition, secondaryPosition, 0.3f);


        // atualiza a posição da câmera
        transform.position = Vector3.Lerp(transform.position, adjustedPosition, followSpeed * Time.deltaTime);

        ShakeUpdate();
    }

    public void LockScreenAsSecondary(Transform passedTarget){
        lockOnMouse = false;
        Player.GetComponent<PlayerMovement>().boundByCamera = true;
        secondaryTarget = passedTarget;
    }

    public void LockScreenAsPrimary(Transform passedTarget){
        lockOnMouse = false;
        Player.GetComponent<PlayerMovement>().boundByCamera = true;
        secondaryTarget = target;
        target = passedTarget;
    }
    
    public void UnlockScreen(){
        lockOnMouse = true;
        Player.GetComponent<PlayerMovement>().boundByCamera = false;
        secondaryTarget = null;
    }

    Vector3 originalPosition;
    List<ShakeProcess> activeShakes = new List<ShakeProcess>();

    public void ShakeCamera(float duration, float magnitude)
    {
        
        ShakeProcess newShake = new ShakeProcess(duration, magnitude);
        activeShakes.Add(newShake);
        originalPosition = transform.position;
    }

    void ShakeUpdate()
    {
        if(activeShakes.Count > 0){
            
            Vector3 totalOffset = new Vector3(0, 0, -cameraDistance);
            foreach (ShakeProcess shake in activeShakes)
            {
                totalOffset += Random.insideUnitSphere * shake.Magnitude * Time.fixedDeltaTime * 35;
            }

            // Apply the total offset to the camera position
            transform.position = originalPosition + totalOffset;

            // Decrease duration for all active shakes
            for (int i = activeShakes.Count - 1; i >= 0; i--)
            {
                activeShakes[i].Duration -= Time.fixedDeltaTime;
                if (activeShakes[i].Duration <= 0)
                {
                    activeShakes.RemoveAt(i);
                }
            }
        }

    }

    public void StopAllShakes(){
        activeShakes.Clear();
    }

    private class ShakeProcess
    {
        public float Duration { get; set; }
        public float Magnitude { get; set; }

        public ShakeProcess(float duration, float magnitude)
        {
            Duration = duration;
            Magnitude = magnitude;
        }
    }
}
