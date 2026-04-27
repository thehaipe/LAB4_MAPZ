using System;
using CommunityToolkit.Mvvm.Input;
using LAB4_MAPZ.Models;

namespace LAB4_MAPZ.ViewModels;

public class AvailableBuildingVm
{
    public string      Name        { get; }
    public string      Description { get; }
    public Building    Prototype   { get; }
    public RelayCommand BuildCommand { get; }

    public AvailableBuildingVm(Building prototype, Action<Building> onBuild)
    {
        Prototype    = prototype;
        Name         = prototype.Name;
        Description  = prototype.Description;
        BuildCommand = new RelayCommand(() => onBuild(prototype));
    }
}
