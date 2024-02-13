using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] private GameObject groundTile;
    private Vector3 _nextSpawnPoint;
    private int _tileCount;
    
    private void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            SpawnTile();
        }
    }

    public void SpawnTile()
    {
        _tileCount++;
        GameObject temp = Instantiate(groundTile, _nextSpawnPoint, Quaternion.identity);
        _nextSpawnPoint = temp.transform.GetChild(1).transform.position;
        
        if (_tileCount > 0 && _tileCount % 10 == 0)
        {
            temp.transform.GetChild(2).gameObject.SetActive(true);
            temp.transform.GetChild(3).gameObject.SetActive(true);
            temp.transform.GetChild(4).gameObject.SetActive(true);
        }
        
    }

}