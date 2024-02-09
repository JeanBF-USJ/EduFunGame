using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private GroundSpawner _groundSpawner;
    
    void Start()
    {
        _groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
    }

    private void OnTriggerExit(Collider other)
    {
        _groundSpawner.SpawnTile();
        Destroy(gameObject, 2); // destroy the tile we exited 2 seconds later
    }

    void Update()
    {
        
    }
}
