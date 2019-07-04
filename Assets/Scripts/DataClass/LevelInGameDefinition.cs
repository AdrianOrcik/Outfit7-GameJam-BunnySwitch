using System;
using System.Collections.Generic;

[Serializable]
public class LevelInGameDefinition
{
    public string LevelName;
    public int xCount;
    public int yCount;
    public int MaxTileX;
    public int yTiles;
    public int RemainingNunu;
    public int GoalNunu;
    public int RemainingWaves;
    public List<int> EndPointsCapacity; //TODO: Implement
    public List<int> SpawnWaves;
}