using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "GameLevelsData", menuName = "Game/GameLevelsData", order = 1)]
public class GameLevelsData : ScriptableObject
{
    [ReorderableList] public LevelData[] levels = new LevelData[0];
}