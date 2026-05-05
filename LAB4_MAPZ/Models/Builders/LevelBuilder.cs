using System.Collections.Generic;

namespace LAB4_MAPZ.Models.Builders;

public class LevelBuilder : ILevelBuilder
{
    private GameLevel _level = new();

    public ILevelBuilder SetName(string name)                          { _level.Name             = name;      return this; }
    public ILevelBuilder SetGoals(Dictionary<ResourceType, int> goals) { _level.Goals            = goals;     return this; }
    public ILevelBuilder SetInitialBuildings(List<Building> buildings)  { _level.InitialBuildings = buildings; return this; }
    public ILevelBuilder SetAvailableBuildings(List<Building> buildings){ _level.AvailableToBuild = buildings; return this; }

    public ILevelBuilder SetMedalTimes(int goldSeconds, int silverSeconds)
    {
        _level.GoldTimeSeconds   = goldSeconds;
        _level.SilverTimeSeconds = silverSeconds;
        return this;
    }

    public ILevelBuilder SetStartingMoney(int money)
    {
        _level.StartingMoney = money;
        return this;
    }

    public GameLevel Build()
    {
        var result = _level;
        _level = new GameLevel();   // скидаємо, щоб builder можна було використати повторно
        return result;
    }
}
