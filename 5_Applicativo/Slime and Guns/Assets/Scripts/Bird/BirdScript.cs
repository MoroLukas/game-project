using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public float jumpStrength = 7f;

    // Deve essere senza parametri per Send Messages
    public void OnJump()
    {
        myRigidBody.linearVelocity = new Vector2(
            myRigidBody.linearVelocity.x,
            jumpStrength
        );
    }

    public void OnGoRight()
    {
        myRigidBody.linearVelocity = new Vector2(
            myRigidBody.linearVelocity.x,
            jumpStrength
        );
    }
}
