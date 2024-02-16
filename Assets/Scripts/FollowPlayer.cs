using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 _offset;

    void Start()
    {
        _offset = transform.position - player.position;
    }
    
    void Update()
    {
        Vector3 targetPos = player.position + _offset;
        targetPos.x = 0;
        transform.position = targetPos;
    }
}