using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject gameSelectionMenu;
    
    private void Start()
    {
        
    }

    public void OpenGameSelectionMenu()
    {
        gameSelectionMenu.SetActive(true);
    }
    
    public void CloseGameSelectionMenu()
    {
        gameSelectionMenu.SetActive(false);
    }
}
