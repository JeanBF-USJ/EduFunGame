using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool _dead;
    private float _currentCooldown; // COOLDOWN IS BUGGY
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5;
    [SerializeField] private int currentLane = 1;
    [SerializeField] public float laneDistance = 3f;
    [SerializeField] private float laneChangeSpeed = 5f;
    [SerializeField] private float laneChangeCooldown = 0.05f;

    public void Start()
    {
        _dead = false;
        _currentCooldown = 0f;
    }

    private void FixedUpdate()
    {
        if (_dead) return;
        
        Vector3 forwardMove = transform.forward * (speed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + forwardMove);

        float targetXPosition = Mathf.Clamp((currentLane - 1) * laneDistance, -laneDistance, laneDistance);
        Vector3 newPosition = new Vector3(targetXPosition, rb.position.y, rb.position.z);
        rb.MovePosition(Vector3.Lerp(rb.position, newPosition, laneChangeSpeed * Time.fixedDeltaTime));
    }

    private void Update()
    {
        if (_dead) return;
        
        float horizontalInput = Input.GetAxis("Horizontal");
        
        // avoid spam + control swapping from one lane to another
        if (_currentCooldown > 0)
        {
            _currentCooldown -= Time.deltaTime;
            return;
        }
        
        if (horizontalInput > 0 && currentLane < 2) currentLane++;
        else if (horizontalInput < 0 && currentLane > 0) currentLane--;
        StartCooldown();
    }

    private void StartCooldown()
    {
        _currentCooldown = laneChangeCooldown;
    }

    public void Die(bool won)
    {
        _dead = true;
        FindObjectOfType<GameManager>().GameOver(won);
    }
}