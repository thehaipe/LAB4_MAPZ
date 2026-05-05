using System.Collections.Generic;

namespace LAB4_MAPZ.Models;

// ─── Продукт Builder'а ───────────────────────────────────────────────────────
// GameLevel збирається по кроку через LevelBuilder + LevelDirector.
// Тут немає логіки — тільки дані рівня.
// ────────────────────────────────────────────────────────────────────────────
public class GameLevel
{
    public string                     Name             { get; set; } = "";
    public Dictionary<ResourceType, int> Goals         { get; set; } = new();
    public List<Building>             InitialBuildings  { get; set; } = new();
    public List<Building>             AvailableToBuild  { get; set; } = new();

    // Межі часу для медалей (у секундах)
    public int GoldTimeSeconds   { get; set; }
    public int SilverTimeSeconds { get; set; }
    public int StartingMoney     { get; set; }
}
