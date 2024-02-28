using UnityEngine;

public class LevelCalculator : MonoBehaviour
{
    public LevelInfo CalculateLevelAndProgress(float cumulativeXp, float scalingFactor)
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