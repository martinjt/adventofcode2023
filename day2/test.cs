
using Xunit;

public class GameTest
{
    [Fact]
    public void BasicGameInitTest()
    {
        var gameInput = "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green";
        var game = Game.LoadGame(gameInput);

        Assert.Equal(1, game.GameNumber);
        Assert.Equal(3, game.TotalTurns);
        Assert.Equal(6, game.MaxColourRequirements["blue"]);
        Assert.Equal(4, game.MaxColourRequirements["red"]);
        Assert.Equal(2, game.MaxColourRequirements["green"]);

        Assert.True(game.CanHaveGame(
                [("blue", 1)]));
        Assert.True(game.CanHaveGame(
                [("blue", 6)]));

        Assert.False(game.CanHaveGame(
                [("blue", 7)]));

        Assert.True(game.CanHaveGame(
                [("blue", 6),
                ("red", 4),
                ("green", 2)]));
    }
}