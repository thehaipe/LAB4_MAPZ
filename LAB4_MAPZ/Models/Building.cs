using System.Collections.Generic;

namespace LAB4_MAPZ.Models;

// Prototype: базовий клас-прототип; кожна підбудівля реалізує Clone()
public abstract class Building
{
    public string Name        { get; protected set; } = "";
    public string Description { get; protected set; } = "";
    public int    Level       { get; set; } = 1;
    public int    ProductionIntervalSeconds { get; protected set; } = 3;

    private int _tickCounter;

    public bool Tick(Dictionary<ResourceType, int> resources)
    {
        _tickCounter++;
        if (_tickCounter < ProductionIntervalSeconds) return false;
        _tickCounter = 0;
        return TryProduce(resources);
    }

    protected abstract bool TryProduce(Dictionary<ResourceType, int> resources);
    public abstract Building Clone();

    public override string ToString() => $"{Name} [Lv.{Level}]";
}
