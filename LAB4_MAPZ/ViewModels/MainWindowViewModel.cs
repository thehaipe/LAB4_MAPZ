using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LAB4_MAPZ.Models;
using LAB4_MAPZ.Models.Facades;
using LAB4_MAPZ.Models.Factories;
using LAB4_MAPZ.Models.Structural;

namespace LAB4_MAPZ.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly GameFacade      _game = new();
    private readonly DispatcherTimer _timer;

    [ObservableProperty] private bool   _showMenu     = true;
    [ObservableProperty] private bool   _showGame     = false;
    [ObservableProperty] private bool   _showGameOver = false;
    [ObservableProperty] private string _levelName    = "";
    [ObservableProperty] private string _timeDisplay  = "00:00";
    [ObservableProperty] private string _medalDisplay = "";
    [ObservableProperty] private string _medalHint    = "";
    [ObservableProperty] private string _moneyDisplay = "Credits: 0";
    [ObservableProperty] private string _buildMessage = "";

    public ObservableCollection<AvailableBuildingVm> AvailableBuildings  { get; } = new();
    public ObservableCollection<string>              ActiveBuildingsList  { get; } = new();
    public ObservableCollection<ResourceGoalVm>      ResourceGoals        { get; } = new();

    // Пікер вугільних шахт: три прототипи у ComboBox
    public CoalMinePickerVm CoalMinePicker { get; }

    public MainWindowViewModel()
    {
        _timer         = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick   += OnTimerTick;
        CoalMinePicker = new CoalMinePickerVm(
            OnBuildBuilding,
            CanBuildBuilding,
            _game.CreateOffers);
    }

    [RelayCommand] private void StartEasy() => StartGame(new EasyGameFactory());
    [RelayCommand] private void StartHard() => StartGame(new HardGameFactory());

    [RelayCommand]
    private void Restart()
    {
        _timer.Stop();
        _game.Reset();
        TimeDisplay  = "00:00";
        MedalDisplay = "";
        MoneyDisplay = "Credits: 0";
        BuildMessage = "";
        ShowMenu     = true;
        ShowGame     = false;
        ShowGameOver = false;
    }

    private void StartGame(IBuildingFactory factory)
    {
        var level = _game.StartLevel(factory);

        LevelName = level.Name;
        MedalHint = $"Gold < {level.GoldTimeSeconds / 60}:00   |   " +
                    $"Silver < {level.SilverTimeSeconds / 60}:00   |   Copper — решта";
        BuildMessage = "";
        RefreshMoney();

        AvailableBuildings.Clear();
        foreach (var offer in _game.CreateOffers(level.AvailableToBuild))
            AvailableBuildings.Add(new AvailableBuildingVm(offer, OnBuildBuilding, CanBuildBuilding));

        ResourceGoals.Clear();
        foreach (var g in level.Goals)
            ResourceGoals.Add(new ResourceGoalVm(g.Key.ToString(), g.Value));

        RefreshActiveBuildings();

        ShowMenu     = false;
        ShowGame     = true;
        ShowGameOver = false;
        _timer.Start();
    }

    private void OnBuildBuilding(IBuildingOffer offer)
    {
        _game.TryBuild(offer, out var message);
        BuildMessage = message;
        RefreshMoney();
        RefreshActiveBuildings();
        RefreshBuildCommands();
    }

    private bool CanBuildBuilding(IBuildingOffer offer) => _game.CanBuild(offer);

    private void OnTimerTick(object? sender, EventArgs e)
    {
        _game.Tick();

        TimeDisplay = TimeSpan.FromSeconds(_game.ElapsedSeconds).ToString(@"mm\:ss");
        RefreshMoney();

        foreach (var rg in ResourceGoals)
        {
            if (Enum.TryParse<ResourceType>(rg.ResourceName, out var type))
            {
                _game.Resources.TryGetValue(type, out int amount);
                rg.Current = amount;
            }
        }

        RefreshActiveBuildings();
        RefreshBuildCommands();

        if (!_game.IsFinished) return;

        _timer.Stop();
        MedalDisplay = _game.GetMedal() switch
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
        foreach (var b in _game.ActiveBuildings)
        {
            string wear   = b.MaxDurability > 0 ? $"  [Dur: {b.Durability}/{b.MaxDurability}]" : "";
            string broken = b.IsBroken           ? "  *** BROKEN ***"                          : "";
            ActiveBuildingsList.Add($"{b}{wear}{broken}   —   {b.Description}");
        }
    }

    private void RefreshMoney() => MoneyDisplay = $"Credits: {_game.Money}";

    private void RefreshBuildCommands()
    {
        foreach (var building in AvailableBuildings)
            building.BuildCommand.NotifyCanExecuteChanged();

        CoalMinePicker.BuildCommand.NotifyCanExecuteChanged();
    }
}
