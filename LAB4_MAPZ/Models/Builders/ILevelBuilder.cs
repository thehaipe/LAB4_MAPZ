using System.Collections.Generic;

namespace LAB4_MAPZ.Models.Builders;

// Builder: інтерфейс покрокової побудови GameLevel
public interface ILevelBuilder
{
    ILevelBuilder SetName(string name);
    ILevelBuilder SetGoals(Dictionary<ResourceType, int> goals);
    ILevelBuilder SetInitialBuildings(List<Building> buildings);
    ILevelBuilder SetAvailableBuildings(List<Building> buildings);
    ILevelBuilder SetMedalTimes(int goldSeconds, int silverSeconds);
    ILevelBuilder SetStartingMoney(int money);
    GameLevel     Build();
}
