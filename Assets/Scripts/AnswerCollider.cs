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
        
        Transform parentTransform = transform.parent;
        
        Transform[] options = new Transform[] {
            parentTransform.transform.GetChild(2),
            parentTransform.transform.GetChild(3),
            parentTransform.transform.GetChild(4)
        };
        
        foreach (var option in options)
        {
            option.gameObject.SetActive(false);
        }
        
        if (!_correctAnswer)
        {
            other.GetComponent<PlayerMovement>().Die();
        }
    }
}