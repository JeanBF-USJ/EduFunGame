using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    private Vector3 _offset;

    void Start()
    {
        _offset = transform.position - player.position;
    }
    
    void Update()
    {
        transform.position = player.position + _offset;
    }
}
