using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public Transform player;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.position;
    }
    
    void Update()
    {
        transform.position = player.position + offset;
    }
}
