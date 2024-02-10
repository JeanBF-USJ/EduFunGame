using UnityEngine;

public class Coin : MonoBehaviour
{
    public float turnSpeed = 90f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Player")
        {
            return;
        }
        
        GameManager.GetInstance().IncrementScore();
        Destroy(gameObject);
    }
    
    void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }
}
