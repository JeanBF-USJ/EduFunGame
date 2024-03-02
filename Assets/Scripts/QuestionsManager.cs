using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class QuestionsManager : MonoBehaviour
{
    [SerializeField] private GameObject playerParent;
    [SerializeField] private TextMeshProUGUI questionText;
    
    private APIManager _apiManager;
    private Question[] _questions;
    private int _questionsCounter;
    private int _answersCounter;

    private void Start()
    {
        _apiManager = GetComponent<APIManager>();
        string apiEndpoint = "/questions";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, null, true, HandleResponse));
    }

    private void HandleResponse(UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            Questions response = JsonUtility.FromJson<Questions>(www.downloadHandler.text);
            _questions = response.questions;
            NextQuestion();
        }
    }

    public void NextQuestion()
    {
        if (_questionsCounter == _questions.Length)
        {
            _questionsCounter = 0;
            _answersCounter = 0;
            playerParent.GetComponent<PlayerMovement>().Die(true);
        }
        else questionText.text = _questions[_questionsCounter++].question;
    }

    public Question[] GetQuestions()
    {
        return _questions;
    }

    public string[] GetAnswers()
    {
        if (_answersCounter == _questions.Length) return new string[] { "", "", "" };
        
        Question currentQuestion = _questions[_answersCounter++];
        return new string[]{currentQuestion.correct_answer, currentQuestion.wrong_answers[0], currentQuestion.wrong_answers[1]};
    }
}


[System.Serializable]
public class Question
{
    public string category_id;
    public string question;
    public string correct_answer;
    public string[] wrong_answers;
}

[System.Serializable]
public class Questions
{
    public Question[] questions;
}