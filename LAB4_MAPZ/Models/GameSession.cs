using System.Collections.Generic;
using System.Linq;
using LAB4_MAPZ.Models.Economy;

namespace LAB4_MAPZ.Models;


public class GameSession
{
    private static GameSession? _instance;
    private static readonly object _lock = new();

    public static GameSession Instance
    {
        get { lock (_lock) { return _instance ??= new GameSession(); } }
    }

    private GameSession() { }

    public GameLevel?                    CurrentLevel    { get; private set; }
    public Dictionary<ResourceType, int> Resources       { get; private set; } = new();
    public List<Building>                ActiveBuildings { get; private set; } = new();
    public int                           ElapsedSeconds  { get; private set; }
    public int                           Money           { get; private set; }
    public bool                          IsRunning       { get; private set; }
    public bool                          IsFinished      { get; private set; }

    public void StartLevel(GameLevel level)
    {
        CurrentLevel    = level;
        Resources       = new Dictionary<ResourceType, int>();
        ElapsedSeconds  = 0;
        Money           = level.StartingMoney;
        IsRunning       = true;
        IsFinished      = false;
        ActiveBuildings = level.InitialBuildings.Select(b => b.Clone()).ToList();
    }

    public void Reset()
    {
        CurrentLevel    = null;
        Resources       = new Dictionary<ResourceType, int>();
        ActiveBuildings = new List<Building>();
        ElapsedSeconds  = 0;
        Money           = 0;
        IsRunning       = false;
        IsFinished      = false;
    }

    public void Tick()
    {
        if (!IsRunning) return;

        var incomeCalculator = new ResourceIncomeCalculator();
        ElapsedSeconds++;
        foreach (var building in ActiveBuildings)
        {
            // Доходи рахуються за дельтою ресурсів після конкретної будівлі:
            // так переробка ресурсів не дає гроші за вже витрачену сировину.
            var beforeProduction = Resources.ToDictionary(pair => pair.Key, pair => pair.Value);
            if (building.Tick(Resources))
                Money += incomeCalculator.CalculateIncome(beforeProduction, Resources);
        }

        if (GoalsAchieved())
        {
            IsRunning  = false;
            IsFinished = true;
        }
    }

    public void AddBuilding(Building prototype) =>
        ActiveBuildings.Add(prototype.Clone());

    public bool SpendMoney(int amount)
    {
        if (Money < amount) return false;

        Money -= amount;
        return true;
    }

    public void AddMoney(int amount) => Money += amount;

    public bool GoalsAchieved()
    {
        if (CurrentLevel == null) return false;
        return CurrentLevel.Goals.All(g => Resources.GetValueOrDefault(g.Key) >= g.Value);
    }

    public string GetMedal()
    {
        if (CurrentLevel == null) return "";
        if (ElapsedSeconds <= CurrentLevel.GoldTimeSeconds)   return "Gold";
        if (ElapsedSeconds <= CurrentLevel.SilverTimeSeconds) return "Silver";
        return "Copper";
    }
}
