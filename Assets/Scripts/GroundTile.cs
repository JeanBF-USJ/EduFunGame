using UnityEngine;

public class GroundTile : MonoBehaviour
{
    
    private GroundSpawner groundSpawner;
    
    void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
    }

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile();
        Destroy(gameObject, 2); // destroy the tile we exited 2 seconds later
    }

    void Update()
    {
        
    }
}
