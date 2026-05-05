using System.Collections.Generic;

namespace LAB4_MAPZ.Models.Factories;

// Abstract Factory: інтерфейс для сімейства об'єктів одної складності
public interface IBuildingFactory
{
    string DifficultyName { get; }
    List<Building>                CreateInitialBuildings();
    List<Building>                CreateAvailableBuildings();
    Dictionary<ResourceType, int> CreateGoals();
    int GetGoldTimeSeconds();
    int GetSilverTimeSeconds();
    int GetStartingMoney();
}
