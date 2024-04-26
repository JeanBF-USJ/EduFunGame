using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private GroundSpawner _groundSpawner;
    private PlayerMovement _playerMovement;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int numberOfLanes = 3;

    void Start()
    {
        _groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        _playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        SpawnCoins();
    }

    private void OnTriggerExit(Collider other)
    {
        _groundSpawner.SpawnTile();
        Destroy(gameObject, 2); // destroy the tile we exited 2 seconds later
    }

    private void SpawnCoins()
    {
        int coinsToSpawn = 3;
        int maxCoinsInOneLane = 3;

        for (int i = 0; i < coinsToSpawn;)
        {
            int lane = Random.Range(0, numberOfLanes);
            int coinsInLane = Random.Range(1, maxCoinsInOneLane + 1);

            float laneWidth = _playerMovement.laneDistance;
            float xOffset = (lane - (numberOfLanes - 1) * 0.5f) * laneWidth;

            float verticalSpacing = 5f;

            for (int j = 0; j < coinsInLane; j++)
            {
                GameObject temp = Instantiate(coinPrefab, transform);
                float yOffset = j * verticalSpacing;
                temp.transform.position = new Vector3(
                    Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f) + xOffset,
                    1,
                    Random.Range(transform.position.z - 0.5f, transform.position.z + 0.5f) + yOffset
                );
                i++;
                if (i >= coinsToSpawn) break;
            }
        }
    }
}