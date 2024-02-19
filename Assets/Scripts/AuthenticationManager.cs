using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthenticationManager : MonoBehaviour
{
    [SerializeField] GameObject loginScreen;
    [SerializeField] GameObject registerScreen;

    public void AlreadyHaveAnAccount()
    {
        loginScreen.gameObject.SetActive(true);
        registerScreen.gameObject.SetActive(false);
    }
    
    public void DontHaveAnAccount()
    {
        registerScreen.gameObject.SetActive(true);
        loginScreen.gameObject.SetActive(false);
    }

    public void Login()
    {
        SceneManager.LoadScene(1);
    }

    public void Register()
    {
        SceneManager.LoadScene(1);
    }
}