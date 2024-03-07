using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentLevel;
    [SerializeField] private TextMeshProUGUI nextLevel;
    [SerializeField] private RectTransform levelBar;
    [SerializeField] private RectTransform progressBar;

    public void SetPlayerLevel(float cumulativeXp, float scalingFactor)
    {
        LevelInfo levelInfo = CalculateLevelAndProgress(cumulativeXp, scalingFactor);
        currentLevel.text = "LEVEL " + levelInfo.Level;
        nextLevel.text = string.Format("{0:N0}", levelInfo.RemainingXpForNextLevel) + " XP FOR LEVEL " + (levelInfo.Level + 1);

        float progressPercentage = levelInfo.ProgressWithinLevel / (levelInfo.ProgressWithinLevel + levelInfo.RemainingXpForNextLevel);
        
        Vector2 newSize = progressBar.sizeDelta;
        newSize.x = progressPercentage * levelBar.sizeDelta.x;
        progressBar.sizeDelta = newSize;
        
        progressBar.anchoredPosition = new Vector2(levelBar.anchoredPosition.x, progressBar.anchoredPosition.y);
    }
    private LevelInfo CalculateLevelAndProgress(float cumulativeXp, float scalingFactor)
    {
        int level = Mathf.FloorToInt(Mathf.Sqrt(cumulativeXp / scalingFactor));
        float currentLevelXp = Mathf.Pow(level, 2) * scalingFactor;
        float progressWithinLevel = cumulativeXp - currentLevelXp;
        float remainingXpForNextLevel = Mathf.Pow(level + 1, 2) * scalingFactor - cumulativeXp;

        return new LevelInfo(level, progressWithinLevel, remainingXpForNextLevel);
    }
}

public class LevelInfo
{
    public int Level { get; }
    public float ProgressWithinLevel { get; }
    public float RemainingXpForNextLevel { get; }

    public LevelInfo(int level, float progressWithinLevel, float remainingXpForNextLevel)
    {
        Level = level;
        ProgressWithinLevel = progressWithinLevel;
        RemainingXpForNextLevel = remainingXpForNextLevel;
    }
}