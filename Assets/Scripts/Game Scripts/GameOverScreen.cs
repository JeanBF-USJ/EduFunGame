using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShareToX()
    {
        string score = FindObjectOfType<GameManager>().GetScore();
        string tweetContent = "I just scored " + score + " in EduFun's Trivia Game! Do you think you can do better? Only one way to find out! ";
        string encodedTweetContent = System.Uri.EscapeUriString(tweetContent);
        string encodedURL = "https://twitter.com/intent/tweet?text=" + encodedTweetContent + "%0A%23EduFun%20%23EducationalGame";

        Application.OpenURL(encodedURL);
    }

    public void Leave()
    {
        SceneManager.LoadScene(1);
    }
}