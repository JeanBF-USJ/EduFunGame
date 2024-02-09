using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTile;
    private Vector3 _nextSpawnPoint;
    
    private void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            SpawnTile();
        }
    }

    public void SpawnTile()
    {
        GameObject temp = Instantiate(groundTile, _nextSpawnPoint, Quaternion.identity);
        _nextSpawnPoint = temp.transform.GetChild(1).transform.position;
    }

}