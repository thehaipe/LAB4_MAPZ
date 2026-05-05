using System;
using CommunityToolkit.Mvvm.Input;
using LAB4_MAPZ.Models.Structural;

namespace LAB4_MAPZ.ViewModels;

public class AvailableBuildingVm
{
    public string      Name        { get; }
    public string      Description { get; }
    public string      CostDisplay { get; }
    public IBuildingOffer Offer    { get; }
    public RelayCommand BuildCommand { get; }

    public AvailableBuildingVm(
        IBuildingOffer offer,
        Action<IBuildingOffer> onBuild,
        Func<IBuildingOffer, bool> canBuild)
    {
        Offer       = offer;
        Name        = offer.Name;
        Description = offer.Description;
        CostDisplay = $"Cost: {offer.Cost} credits";
        BuildCommand = new RelayCommand(
            () => onBuild(offer),
            () => canBuild(offer));
    }
}
