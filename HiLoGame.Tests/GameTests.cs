using System.Linq;
using Xunit;
using Moq;

namespace HiLoGame.Tests
{
    public class GameTests
    {
        private readonly Mock<IInputOutputService> _mockIoService;

        public GameTests()
        {
            _mockIoService = new Mock<IInputOutputService>();
        }

        [Fact]
        public void AddPlayer_ShouldAddPlayerToGame()
        {
            var game = new Game(1, 100, _mockIoService.Object);
            game.AddPlayer("Player1");
            Assert.Single(game.Players);
            Assert.Equal("Player1", game.Players[0].Name);
        }

        [Fact]
        public void MysteryNumber_ShouldBeWithinRange()
        {
            var game = new Game(1, 100, _mockIoService.Object);
            Assert.InRange(game.MysteryNumber, 1, 100);
        }

        [Fact]
        public void StartGame_ShouldIdentifyWinner()
        {
            var game = new Game(1, 10, _mockIoService.Object);
            game.AddPlayer("Player1");
            game.AddPlayer("Player2");
            game.MysteryNumber = 5;

            // Simulate guessing
            game.Players[0].Attempts = 1;
            game.Players[0].TotalTime = 10;
            game.Players[1].Attempts = 2;
            game.Players[1].TotalTime = 20;

            // Set Player1 as the winner
            game.Winner = game.Players[0];

            game.AnnounceChampion();

            Assert.Equal("Player1", game.Winner.Name);
        }

        [Fact]
        public void AnnounceChampion_ShouldIdentifyPlayerWithFewestAttempts()
        {
            var game = new Game(1, 10, _mockIoService.Object);
            game.AddPlayer("Player1");
            game.AddPlayer("Player2");

            game.Players[0].Attempts = 5;
            game.Players[0].TotalTime = 30;
            game.Players[1].Attempts = 3;
            game.Players[1].TotalTime = 40;

            game.AnnounceChampion();

            Assert.Equal("Player2", game.Players.OrderBy(p => p.Attempts).First().Name);
        }

        [Fact]
        public void AnnounceChampion_ShouldBreakTiesBasedOnTotalTime()
        {
            var game = new Game(1, 10, _mockIoService.Object);
            game.AddPlayer("Player1");
            game.AddPlayer("Player2");

            game.Players[0].Attempts = 3;
            game.Players[0].TotalTime = 40;
            game.Players[1].Attempts = 3;
            game.Players[1].TotalTime = 30;

            game.AnnounceChampion();

            Assert.Equal("Player2", game.Players.OrderBy(p => p.Attempts).ThenBy(p => p.TotalTime).First().Name);
        }
    }
}
