using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameDescriptionManager : MonoBehaviour
{
    public string id;
    [SerializeField] private TextMeshProUGUI gameName;
    [SerializeField] private TextMeshProUGUI gameDescription;
    [SerializeField] private TextMeshProUGUI gameCategoryAge;
    [SerializeField] private RawImage gameImage;
    [SerializeField] private GameObject favorite;
    [SerializeField] private GameObject gameDescriptionScreen;

    private HubManager _hubManager;
    private GameSelectionManager _gameSelectionManager;
    
    private void Start()
    {
        _hubManager = GetComponent<HubManager>();
        _gameSelectionManager = GetComponent<GameSelectionManager>();
    }

    public void OpenGameDescription(GameItem gameItem)
    {
        gameName.text = gameItem.itemName.text;
        gameDescription.text = gameItem.description;
        gameCategoryAge.text = gameItem.category + " — " + gameItem.minAge + "-" + gameItem.maxAge;
        
        Texture2D texture = Resources.Load<Texture2D>("GameIcons/" + gameItem.itemName.text);
        gameImage.texture = texture;

        favorite.gameObject.SetActive(gameItem.favorite);
        
        gameDescriptionScreen.gameObject.SetActive(true);
    }
    
    public void CloseGameDescription()
    {
        gameDescriptionScreen.gameObject.SetActive(false);
    }

    public void SelectGame()
    {
        _hubManager.SelectGame(gameName.text);
        _gameSelectionManager.CloseGameSelectionMenu();
    }

    public void FavoriteGame()
    {
        
    }
    
    public void UnFavoriteGame()
    {
        
    }

    public void ReportGame()
    {
        
    }

    public void RateGame()
    {
        
    }
}
