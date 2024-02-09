using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public Rigidbody rb;
    private float _horizontalInput;
    public float horizontalMultiplier = 2;

    private void FixedUpdate()
    {
        Vector3 forwardMove = transform.forward * (speed * Time.fixedDeltaTime);
        Vector3 horizontalMove = transform.right * (_horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier);
        rb.MovePosition(rb.position + forwardMove + horizontalMove);
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
    }
}