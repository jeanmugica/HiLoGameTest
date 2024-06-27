using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace HiLoGame
{
    class Program
    {
        static void Main(string[] args)
        {
            IInputOutputService ioService = new ConsoleInputOutputService();
            Console.WriteLine("Welcome to the Hi-Lo Game!");
            bool playAgain = true;

            while (playAgain)
            {
                Game game = InitializeGame(ioService);
                game.Start();

                playAgain = ioService.PromptUser("Do you want to play again? (yes/no): ");
                if (!playAgain)
                {
                    ioService.WriteLine("Thank you for playing! Goodbye.");
                }
            }
        }

        static Game InitializeGame(IInputOutputService ioService)
        {
            if (File.Exists("gamestate.json"))
            {
                if (ioService.PromptUser("Do you want to load the previous game state? (yes/no): "))
                {
                    return LoadGameState(ioService);
                }
            }

            return SetupNewGame(ioService);
        }

        static Game SetupNewGame(IInputOutputService ioService)
        {
            int min = ioService.ReadValidNumber("Enter the minimum number: ");
            int max = ioService.ReadValidNumber("Enter the maximum number: ");
            Game game = new Game(min, max, ioService);

            int playerCount = ioService.ReadValidNumber("Enter the number of players (must be at least 1): ", 1);

            for (int i = 1; i <= playerCount; i++)
            {
                ioService.Write($"Enter name for Player {i}: ");
                string playerName = ioService.ReadLine();
                game.AddPlayer(playerName);
            }

            return game;
        }

        static Game LoadGameState(IInputOutputService ioService)
        {
            try
            {
                string json = File.ReadAllText("gamestate.json");
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = true
                };
                Game game = JsonSerializer.Deserialize<Game>(json, options);
                game.InjectIOService(ioService);
                return game;
            }
            catch (Exception ex)
            {
                ioService.WriteLine($"Error loading game state: {ex.Message}");
                return SetupNewGame(ioService);
            }
        }
    }
}
