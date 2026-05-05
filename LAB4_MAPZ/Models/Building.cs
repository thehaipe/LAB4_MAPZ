using System;
using System.Collections.Generic;

namespace LAB4_MAPZ.Models;

// Prototype: базовий клас-прототип; кожна підбудівля реалізує Clone()
public abstract class Building
{
    public string Name        { get; protected set; } = "";
    public string Description { get; protected set; } = "";
    public int    Level       { get; set; } = 1;
    public int    ProductionIntervalSeconds { get; protected set; } = 3;

    // Знос: -1 означає невразливу будівлю (Mine, Forge, Plant)
    public int  MaxDurability { get; protected set; } = -1;
    public int  Durability    { get; protected set; }
    public int  WearPerCycle  { get; protected set; } = 0;
    public bool IsBroken      => MaxDurability > 0 && Durability <= 0;

    private int _tickCounter;

    public bool Tick(Dictionary<ResourceType, int> resources)
    {
        if (IsBroken) return false;

        _tickCounter++;
        if (_tickCounter < ProductionIntervalSeconds) return false;
        _tickCounter = 0;

        bool produced = TryProduce(resources);

        if (produced && MaxDurability > 0)
            Durability = Math.Max(0, Durability - WearPerCycle);

        return produced;
    }

    protected abstract bool TryProduce(Dictionary<ResourceType, int> resources);
    public abstract Building Clone();

    public override string ToString() => $"{Name} [Lv.{Level}]";
}
