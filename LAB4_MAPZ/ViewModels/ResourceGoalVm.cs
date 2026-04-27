using CommunityToolkit.Mvvm.ComponentModel;

namespace LAB4_MAPZ.ViewModels;

public partial class ResourceGoalVm : ObservableObject
{
    public string ResourceName { get; }
    public int    Goal         { get; }

    // NotifyPropertyChangedFor — автоматично нотифікує Display і IsMet при зміні Current
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Display))]
    [NotifyPropertyChangedFor(nameof(IsMet))]
    private int _current;

    public ResourceGoalVm(string resourceName, int goal)
    {
        ResourceName = resourceName;
        Goal         = goal;
    }

    public string Display => $"{ResourceName}:  {Current} / {Goal}";
    public bool   IsMet   => Current >= Goal;
}
