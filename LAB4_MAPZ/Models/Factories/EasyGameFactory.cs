using System.Collections.Generic;

namespace LAB4_MAPZ.Models.Factories;

// Легка складність: гравець стартує з Копальнею + Кузнею; менші цілі
public class EasyGameFactory : IBuildingFactory
{
    public string DifficultyName => "Easy";

    public List<Building> CreateInitialBuildings() => new()
    {
        new Mine(),
        new Forge()
    };

    public List<Building> CreateAvailableBuildings() => new()
    {
        new Mine(),
        new Mine(ResourceType.Coal),
        new Forge(),
        new MetallurgicalPlant()
    };

    public Dictionary<ResourceType, int> CreateGoals() => new()
    {
        { ResourceType.IronOre, 20 },
        { ResourceType.Iron,    10 },
        { ResourceType.Steel,    3 }
    };

    public int GetGoldTimeSeconds()   => 60;   // < 1 хвилина  → золото
    public int GetSilverTimeSeconds() => 120;  // < 2 хвилини  → срібло
}
