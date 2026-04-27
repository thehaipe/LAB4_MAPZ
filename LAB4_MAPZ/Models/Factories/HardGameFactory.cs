using System.Collections.Generic;

namespace LAB4_MAPZ.Models.Factories;

// Важка складність: тільки одна Копальня на старті; більші цілі, потрібне вугілля
public class HardGameFactory : IBuildingFactory
{
    public string DifficultyName => "Hard";

    public List<Building> CreateInitialBuildings() => new()
    {
        new Mine()   // лише базова копальня
    };

    public List<Building> CreateAvailableBuildings() => new()
    {
        new Mine(),
        new Mine(ResourceType.Coal, amountPerCycle: 2),
        new Forge(),
        new MetallurgicalPlant()
    };

    public Dictionary<ResourceType, int> CreateGoals() => new()
    {
        { ResourceType.IronOre, 50 },
        { ResourceType.Iron,    25 },
        { ResourceType.Steel,   10 },
        { ResourceType.Coal,    15 }
    };

    public int GetGoldTimeSeconds()   => 60;
    public int GetSilverTimeSeconds() => 120;
}
