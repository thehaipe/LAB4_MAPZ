using System.Collections.Generic;

namespace LAB4_MAPZ.Models;

public class Forge : Building
{
    private const int OreConsumed = 3;

    public Forge()
    {
        Name                      = "Forge";
        ProductionIntervalSeconds = 4;
        Description               = $"-{OreConsumed} IronOre  →  +1 Iron / {ProductionIntervalSeconds}s";
    }

    protected override bool TryProduce(Dictionary<ResourceType, int> resources)
    {
        int ore = resources.GetValueOrDefault(ResourceType.IronOre);
        if (ore < OreConsumed) return false;

        resources[ResourceType.IronOre] = ore - OreConsumed;
        resources[ResourceType.Iron]    = resources.GetValueOrDefault(ResourceType.Iron) + Level;
        return true;
    }

    public override Building Clone() => new Forge() { Level = Level };
}
