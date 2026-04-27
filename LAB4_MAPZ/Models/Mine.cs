using System.Collections.Generic;

namespace LAB4_MAPZ.Models;

public class Mine : Building
{
    private readonly ResourceType _produces;
    private readonly int          _amountPerCycle;

    public Mine(ResourceType produces = ResourceType.IronOre, int amountPerCycle = 2)
    {
        _produces       = produces;
        _amountPerCycle = amountPerCycle;

        Name                      = $"{produces} Mine";
        ProductionIntervalSeconds = 3;
        Description               = $"+{_amountPerCycle} {produces} / {ProductionIntervalSeconds}s";
    }

    protected override bool TryProduce(Dictionary<ResourceType, int> resources)
    {
        resources[_produces] = resources.GetValueOrDefault(_produces) + _amountPerCycle * Level;
        return true;
    }

    public override Building Clone() => new Mine(_produces, _amountPerCycle) { Level = Level };
}
