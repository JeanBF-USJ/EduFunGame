using UnityEngine;

public class GroundSpawner : MonoBehaviour
{

    public GameObject groundTile;
    private Vector3 nextSpawnPoint;
    
    private void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            SpawnTile();
        }
    }

    public void SpawnTile()
    {
        GameObject temp = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = temp.transform.GetChild(1).transform.position;
    }

}