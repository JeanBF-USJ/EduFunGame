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

    [Header("ReportInfo")] [SerializeField]
    private GameObject thankYourForYourReport;
    [SerializeField] private GameObject reportScreen;
    [SerializeField] private TMP_InputField reportTitleField;
    [SerializeField] private TMP_InputField reportReasonField;
    
    [Header("")]
    [SerializeField] private GameObject gameDescriptionScreen;

    private APIManager _apiManager;
    private HubManager _hubManager;
    private GameSelectionManager _gameSelectionManager;
    
    private void Start()
    {
        _apiManager = GetComponent<APIManager>();
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
        reportScreen.gameObject.SetActive(false);
        CloseThankYouForYourReport();
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

    public void SubmitReportGame()
    {
        string apiEndpoint = "/report";
        string jsonStr = "{\"gameName\":\"" + gameName.text + "\",\"title\":\"" + reportTitleField.text
                          + "\",\"reason\":\"" + reportReasonField.text + "\"}";
        StartCoroutine(_apiManager.SendRequest(apiEndpoint, jsonStr, true, ReportResponse));
        
        reportTitleField.text = "";
        reportReasonField.text = "";
    }
    
    private void ReportResponse(UnityWebRequest www)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            OpenThankYouForYourReport();
        }
        CloseReportGame();
    }
    
    private void OpenThankYouForYourReport()
    {
        thankYourForYourReport.gameObject.SetActive(true);
    }

    public void CloseThankYouForYourReport()
    {
        thankYourForYourReport.gameObject.SetActive(false);
    }

    public void RateGame()
    {
        
    }

    public void ViewLeaderboard()
    {
        
    }
}
