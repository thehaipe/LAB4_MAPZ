using LAB4_MAPZ.Models.Factories;

namespace LAB4_MAPZ.Models.Builders;

public class LevelDirector
{
    private readonly ILevelBuilder _builder;

    public LevelDirector(ILevelBuilder builder) => _builder = builder;

    public GameLevel BuildFromFactory(IBuildingFactory factory) =>
        _builder
            .SetName($"Level 1  —  {factory.DifficultyName}")
            .SetGoals(factory.CreateGoals())
            .SetInitialBuildings(factory.CreateInitialBuildings())
            .SetAvailableBuildings(factory.CreateAvailableBuildings())
            .SetMedalTimes(factory.GetGoldTimeSeconds(), factory.GetSilverTimeSeconds())
            .Build();
}
