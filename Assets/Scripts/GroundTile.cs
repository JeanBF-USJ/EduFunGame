using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private GroundSpawner _groundSpawner;
    public GameObject coinPrefab;
    
    void Start()
    {
        _groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        SpawnCoins();
    }

    private void OnTriggerExit(Collider other)
    {
        _groundSpawner.SpawnTile();
        Destroy(gameObject, 2); // destroy the tile we exited 2 seconds later
    }

    private void SpawnCoins()
    {
        int coinsToSpawn = 5;
        for (int i = 0; i < coinsToSpawn; i++)
        {
            GameObject temp = Instantiate(coinPrefab, transform);
            temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());
        }
    }

    private Vector3 GetRandomPointInCollider(Collider collider)
    {
        Vector3 point = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(collider.bounds.min.y, collider.bounds.max.y),
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );

        point.y = 1;
        return point;
    }
}
