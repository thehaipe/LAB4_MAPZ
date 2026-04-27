using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LAB4_MAPZ.Models;
using LAB4_MAPZ.Models.Builders;
using LAB4_MAPZ.Models.Factories;

namespace LAB4_MAPZ.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly GameSession     _session = GameSession.Instance;
    private readonly DispatcherTimer _timer;

    [ObservableProperty] private bool   _showMenu     = true;
    [ObservableProperty] private bool   _showGame     = false;
    [ObservableProperty] private bool   _showGameOver = false;
    [ObservableProperty] private string _levelName    = "";
    [ObservableProperty] private string _timeDisplay  = "00:00";
    [ObservableProperty] private string _medalDisplay = "";
    [ObservableProperty] private string _medalHint    = "";

    public ObservableCollection<AvailableBuildingVm> AvailableBuildings  { get; } = new();
    public ObservableCollection<string>              ActiveBuildingsList  { get; } = new();
    public ObservableCollection<ResourceGoalVm>      ResourceGoals        { get; } = new();

    public MainWindowViewModel()
    {
        _timer      = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += OnTimerTick;
    }

    [RelayCommand] private void StartEasy() => StartGame(new EasyGameFactory());
    [RelayCommand] private void StartHard() => StartGame(new HardGameFactory());

    [RelayCommand]
    private void Restart()
    {
        _timer.Stop();
        _session.Reset();
        TimeDisplay  = "00:00";
        MedalDisplay = "";
        ShowMenu     = true;
        ShowGame     = false;
        ShowGameOver = false;
    }

    private void StartGame(IBuildingFactory factory)
    {
        var level = new LevelDirector(new LevelBuilder()).BuildFromFactory(factory);
        _session.StartLevel(level);

        LevelName = level.Name;
        MedalHint = $"Gold < {level.GoldTimeSeconds / 60}:00   |   " +
                    $"Silver < {level.SilverTimeSeconds / 60}:00   |   Copper — решта";

        AvailableBuildings.Clear();
        foreach (var b in level.AvailableToBuild)
            AvailableBuildings.Add(new AvailableBuildingVm(b, OnBuildBuilding));

        ResourceGoals.Clear();
        foreach (var g in level.Goals)
            ResourceGoals.Add(new ResourceGoalVm(g.Key.ToString(), g.Value));

        RefreshActiveBuildings();

        ShowMenu     = false;
        ShowGame     = true;
        ShowGameOver = false;
        _timer.Start();
    }

    private void OnBuildBuilding(Building prototype)
    {
        _session.AddBuilding(prototype);
        RefreshActiveBuildings();
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        _session.Tick();

        TimeDisplay = TimeSpan.FromSeconds(_session.ElapsedSeconds).ToString(@"mm\:ss");

        foreach (var rg in ResourceGoals)
        {
            if (Enum.TryParse<ResourceType>(rg.ResourceName, out var type))
            {
                _session.Resources.TryGetValue(type, out int amount);
                rg.Current = amount;
            }
        }

        if (!_session.IsFinished) return;

        _timer.Stop();
        MedalDisplay = _session.GetMedal() switch
        {
            "Gold"   => $"GOLD MEDAL!   ({TimeDisplay})",
            "Silver" => $"SILVER MEDAL  ({TimeDisplay})",
            _        => $"COPPER MEDAL  ({TimeDisplay})"
        };
        ShowGame     = false;
        ShowGameOver = true;
    }

    private void RefreshActiveBuildings()
    {
        ActiveBuildingsList.Clear();
        foreach (var b in _session.ActiveBuildings)
            ActiveBuildingsList.Add($"{b}   —   {b.Description}");
    }
}
