using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardItem : MonoBehaviour
{
    public TextMeshProUGUI ranking;
    public TextMeshProUGUI username;
    public TextMeshProUGUI score;

    public void SetCurrentUser(bool self)
    {
        GetComponent<Image>().color = self ? new Color(243 / 255f, 232 / 255f, 49 / 255f, 1f) 
            : new Color(1f, 1f, 1f, 80 / 255f);
    }
}