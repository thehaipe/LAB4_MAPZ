using System.Collections.Generic;
using System.Linq;

namespace LAB4_MAPZ.Models;

// Singleton: лише один екземпляр на весь процес
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
    public bool                          IsRunning       { get; private set; }
    public bool                          IsFinished      { get; private set; }

    public void StartLevel(GameLevel level)
    {
        CurrentLevel    = level;
        Resources       = new Dictionary<ResourceType, int>();
        ElapsedSeconds  = 0;
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
        IsRunning       = false;
        IsFinished      = false;
    }

    public void Tick()
    {
        if (!IsRunning) return;

        ElapsedSeconds++;
        foreach (var building in ActiveBuildings)
            building.Tick(Resources);

        if (GoalsAchieved())
        {
            IsRunning  = false;
            IsFinished = true;
        }
    }

    public void AddBuilding(Building prototype) =>
        ActiveBuildings.Add(prototype.Clone());

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
