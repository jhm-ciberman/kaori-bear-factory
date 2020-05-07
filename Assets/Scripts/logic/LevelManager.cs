using UnityEngine;

public static class LevelManager
{
    private static GameLevelsData _levelsData;

    public static void SetLevelsList(GameLevelsData levelsData)
    {
        LevelManager._levelsData = levelsData;
    }

    public static int GetLevelIndex(LevelData level)
    {
        var levels = LevelManager._levelsData.levels;

        for (var i = 0; i < levels.Length; i++)
        {
            if (levels[i] == level) return i;
        }

        return -1;
    }

    public static LevelData GetNextLevel(LevelData level)
    {
        var levels = LevelManager._levelsData.levels;
        int index = LevelManager.GetLevelIndex(level);
        return index + 1 > levels.Length ? null : levels[index + 1];
    }

    public static void Win(LevelData level)
    {
        PlayerPrefs.SetInt("Level_" + level.name, 1);
        PlayerPrefs.Save();
    }

    public static int GetFailedAttempts(LevelData level)
    {
        return PlayerPrefs.GetInt("ConsecutiveFails_Level_" + level.name, 0);
    }

    public static void SetFailedAttempts(LevelData level, int number)
    {
        PlayerPrefs.SetInt("ConsecutiveFails_Level_" + level.name, number);
    }

    public static void Fail(LevelData level)
    {
        LevelManager.SetFailedAttempts(level, LevelManager.GetFailedAttempts(level) + 1);
        PlayerPrefs.Save();
    }
}