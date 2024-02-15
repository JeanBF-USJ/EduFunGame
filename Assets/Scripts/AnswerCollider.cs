using UnityEngine;

public class AnswerCollider : MonoBehaviour
{
    private bool _correctAnswer;

    public void SetCorrectAnswer(bool value)
    {
        _correctAnswer = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.ToLower() != "player") return;
        
        if (!_correctAnswer)
        {
            other.GetComponent<PlayerMovement>().Die();
        }
    }
}