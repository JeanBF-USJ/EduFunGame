using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameSelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject gameItemPrefab;
    
    [SerializeField] private Transform forYourAgeContainer;
    [SerializeField] private Transform allGamesContainer;
    [SerializeField] private Transform favoriteGamesContainer;
    
    [SerializeField] private GameObject gameSelectionMenu;

    private APIManager _apiManager;
    private HubManager _hubManager;
    private GameDescriptionManager _gameDescriptionManager;
    
    private readonly int MIN_AGE = 6;
    private readonly int MAX_AGE = 18;

    private void Start()
    {
        _apiManager = GetComponent<APIManager>();
        _hubManager = GetComponent<HubManager>();
        _gameDescriptionManager = GetComponent<GameDescriptionManager>();
    }

    public void OpenGameSelectionMenu()
    {
        FillGameSelectionMenu();
        gameSelectionMenu.SetActive(true);
    }
    
    public void CloseGameSelectionMenu()
    {
        gameSelectionMenu.SetActive(false);
        _gameDescriptionManager.CloseGameDescription();
    }

    public void FillGameSelectionMenu()
    {
        string apiEndpoint = "/games";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, null, true, DisplayGameItems));
    }
    
    public void EmptyGameItems()
    {
        foreach (Transform child in allGamesContainer.transform) Destroy(child.gameObject);
        foreach (Transform child in forYourAgeContainer.transform) Destroy(child.gameObject);
        foreach (Transform child in favoriteGamesContainer.transform) Destroy(child.gameObject);
    }

    private void DisplayGameItems(UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            int age = CalculateAge();
            EmptyGameItems();
            
            GamesResponse response = JsonUtility.FromJson<GamesResponse>(www.downloadHandler.text);
            
            GameObject newItem;
            foreach (Game game in response.games)
            {
                newItem = Instantiate(gameItemPrefab, allGamesContainer);
                FillItemDetails(newItem, game);
                
                if (game.favorite)
                {
                    newItem = Instantiate(gameItemPrefab, favoriteGamesContainer);
                    FillItemDetails(newItem, game);
                }

                if ((age > MAX_AGE && game.max_age == MAX_AGE) || (age < MIN_AGE && game.min_age == MIN_AGE) ||
                    (age >= game.min_age && age <= game.max_age))
                {
                    newItem = Instantiate(gameItemPrefab, forYourAgeContainer);
                    FillItemDetails(newItem, game);
                }
            }
        }
    }

    private void FillItemDetails(GameObject newItem, Game game)
    {
        GameItem gameItem = newItem.GetComponent<GameItem>();
        gameItem.id = game._id;
        gameItem.itemName.text = game.name;
        gameItem.description = game.description;
        gameItem.category = game.categoryName;
        gameItem.minAge = game.min_age.ToString();
        gameItem.maxAge = game.max_age.ToString();
        gameItem.favorite = game.favorite;

        Texture2D texture = Resources.Load<Texture2D>("GameIcons/" + game.name);
        gameItem.image.texture = texture;
    }

    private int CalculateAge()
    {
        DateTime birthdate = DateTime.ParseExact(_hubManager.GetBirthdate(), "dd/MM/yyyy", null);
        DateTime now = DateTime.Today;
        int age = now.Year - birthdate.Year;
        if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day)) age--;

        return age;
    }
}

[Serializable]
public class GamesResponse
{
    public Game[] games;
}

[Serializable]
public class Game
{
    public string _id;
    public string name;
    public string description;
    public string categoryName;
    public int min_age;
    public int max_age;
    public bool favorite;
}