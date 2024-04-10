using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameDescriptionManager : MonoBehaviour
{
    [Header("GameInfo")]
    public string id;
    [SerializeField] private TextMeshProUGUI gameName;
    [SerializeField] private TextMeshProUGUI gameDescription;
    [SerializeField] private TextMeshProUGUI gameCategoryAge;
    [SerializeField] private RawImage gameImage;
    [SerializeField] private GameObject favorite;
    
    [Header("ReportInfo")]
    [SerializeField] private GameObject reportScreen;
    [SerializeField] private TMP_InputField reportTitleField;
    [SerializeField] private TMP_InputField reportReasonField;
    [SerializeField] private Button reportGameButton;

    [Header("RateGame")]
    [SerializeField] private RawImage[] stars;
    [SerializeField] private Texture2D emptyStar;
    [SerializeField] private Texture2D filledStar;
    [SerializeField] private GameObject rateScreen;
    [SerializeField] private Button rateGameButton;
    
    [Header("")]
    [SerializeField] private GameObject thankYou;
    [SerializeField] private GameObject gameDescriptionScreen;

    private int _selectedStar;
    private APIManager _apiManager;
    private HubManager _hubManager;
    private GameSelectionManager _gameSelectionManager;
    
    private void Start()
    {
        _apiManager = GetComponent<APIManager>();
        _hubManager = GetComponent<HubManager>();
        _gameSelectionManager = GetComponent<GameSelectionManager>();

        rateGameButton.interactable = false;
        reportGameButton.interactable = false;
        
        reportTitleField.onValueChanged.AddListener(OnChangeReportFields);
        reportReasonField.onValueChanged.AddListener(OnChangeReportFields);
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
        CloseReportGame();
        CloseRateScreen();
        CloseThankYou();
    }

    public void SelectGame()
    {
        _hubManager.SelectGame(gameName.text);
        _gameSelectionManager.CloseGameSelectionMenu();
    }

    public void FavoriteGame()
    {
        favorite.gameObject.SetActive(true);
        
        string apiEndpoint = "/games/favorite";
        string jsonStr = "{\"name\":\"" + gameName.text + "\"}";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, jsonStr, true, FavoriteResponse));
    }
    
    public void UnFavoriteGame()
    {
        favorite.gameObject.SetActive(false);
        
        string apiEndpoint = "/games/unfavorite";
        string jsonStr = "{\"name\":\"" + gameName.text + "\"}";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, jsonStr, true, FavoriteResponse));
    }

    private void FavoriteResponse(UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            _gameSelectionManager.FillGameSelectionMenu();
        }
    }

    public void OpenReportGame()
    {
        reportScreen.gameObject.SetActive(true);
    }
    
    public void CloseReportGame()
    {
        reportScreen.gameObject.SetActive(false);
    }

    public void OnChangeReportFields(string newValue)
    {
        if ((reportTitleField.text == "" || reportReasonField.text == "") && reportGameButton.interactable) reportGameButton.interactable = false;
        else if (reportTitleField.text != "" && reportReasonField.text != "" && !reportGameButton.interactable) reportGameButton.interactable = true;
    }

    public void SubmitReportGame()
    {
        string apiEndpoint = "/report";
        string jsonStr = "{\"gameName\":\"" + gameName.text + "\",\"title\":\"" + reportTitleField.text
                          + "\",\"reason\":\"" + reportReasonField.text + "\"}";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, jsonStr, true, ReportResponse));
        
        reportTitleField.text = "";
        reportReasonField.text = "";
        reportGameButton.interactable = false;
    }
    
    private void ReportResponse(UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.Success) OpenThankYou();
        CloseReportGame();
    }
    
    private void OpenThankYou()
    {
        thankYou.gameObject.SetActive(true);
    }

    public void CloseThankYou()
    {
        thankYou.gameObject.SetActive(false);
    }

    public void OnStarClick(int starNumber)
    {
        if (_selectedStar == starNumber) return;
        
        _selectedStar = starNumber;
        for (int i = 0; i < stars.Length; i++) stars[i].texture = i < starNumber ? filledStar : emptyStar;
        
        if (!rateGameButton.interactable) rateGameButton.interactable = true;
    }

    public void OpenRateScreen()
    {
        rateScreen.gameObject.SetActive(true);
    }
    
    public void CloseRateScreen()
    {
        rateScreen.gameObject.SetActive(false);
        rateGameButton.interactable = false;
        for (int i = 0; i < stars.Length; i++) stars[i].texture = emptyStar;
    }

    public void RateGame()
    {
        string apiEndpoint = "/games/rate";
        string jsonStr = "{\"gameName\":\"" + gameName.text + "\",\"stars\":\"" + _selectedStar + "\"}";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, jsonStr, true, RateResponse));
    }

    private void RateResponse(UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.Success) OpenThankYou();
        CloseRateScreen();
    }

    public void ViewLeaderboard()
    {
        
    }
}