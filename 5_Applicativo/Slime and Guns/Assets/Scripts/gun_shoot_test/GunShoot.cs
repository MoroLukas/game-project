using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class GunShoot : MonoBehaviour
{
    public Animator animator;
    void Update() {
        /*
        Fire1 Left Mouse Button(Mouse0)  Sparo / Azione primaria
        Ctrl sinistro Alternativa
        Joystick Button 0                
        */
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Shoot");
        } 
    }
}
