using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] private GameObject groundTile;
    private Vector3 _nextSpawnPoint;
    private int _tileCount;
    
    private void Start()
    {
        StartCoroutine(WaitForQuestionsManager());
    }

    private IEnumerator WaitForQuestionsManager()
    {
        QuestionsManager questionsManager = null;
        
        while (questionsManager == null)
        {
            questionsManager = FindObjectOfType<QuestionsManager>();
            yield return null;
        }
        
        while (questionsManager.GetQuestions() == null)
        {
            yield return null;
        }
        
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
            Transform[] options = new Transform[] {
                temp.transform.GetChild(2),
                temp.transform.GetChild(3),
                temp.transform.GetChild(4)
            };
            
            System.Random random = new System.Random();
            options = options.OrderBy(_ => random.Next()).ToArray();

            string[] answers = FindObjectOfType<QuestionsManager>().GetAnswers();
            for (int i = 0; i < options.Length; i++)
            {
                options[i].gameObject.SetActive(true);
                options[i].GetComponent<AnswerCollider>().SetCorrectAnswer(i == 0);
                TextMeshPro textMeshProComponent = options[i].GetComponent<TextMeshPro>();
                if (textMeshProComponent != null) {
                    textMeshProComponent.text = answers[i];
                }
            }
        }
        
    }
}