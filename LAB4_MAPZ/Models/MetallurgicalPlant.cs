using System.Collections.Generic;

namespace LAB4_MAPZ.Models;

public class MetallurgicalPlant : Building
{
    private const int IronConsumed = 2;
    private const int CoalConsumed = 1;

    public MetallurgicalPlant()
    {
        Name                      = "Metallurgical Plant";
        ProductionIntervalSeconds = 5;
        Description               = $"-{IronConsumed} Iron -{CoalConsumed} Coal  →  +1 Steel / {ProductionIntervalSeconds}s";
    }

    protected override bool TryProduce(Dictionary<ResourceType, int> resources)
    {
        int iron = resources.GetValueOrDefault(ResourceType.Iron);
        int coal = resources.GetValueOrDefault(ResourceType.Coal);
        if (iron < IronConsumed || coal < CoalConsumed) return false;

        resources[ResourceType.Iron]  = iron - IronConsumed;
        resources[ResourceType.Coal]  = coal - CoalConsumed;
        resources[ResourceType.Steel] = resources.GetValueOrDefault(ResourceType.Steel) + Level;
        return true;
    }

    public override Building Clone() => new MetallurgicalPlant() { Level = Level };
}
