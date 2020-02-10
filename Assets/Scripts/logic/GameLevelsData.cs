using UnityEngine;

[CreateAssetMenu(fileName = "GameLevelsData", menuName = "Game/GameLevelsData", order = 1)]
public class GameLevelsData : ScriptableObject
{
    public LevelData levels;
}