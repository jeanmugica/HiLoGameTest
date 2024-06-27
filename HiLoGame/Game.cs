using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Game
{
    public int Min { get; set; }
    public int Max { get; set; }
    public int MysteryNumber { get; set; }
    public List<Player> Players { get; set; }
    public int CurrentPlayerIndex { get; set; }
    public Player Winner { get; set; }

    private IInputOutputService _ioService;
    public void InjectIOService(IInputOutputService ioService)
    {
        _ioService = ioService;
    }

    [JsonConstructor]
    public Game(int min, int max, List<Player> players, int currentPlayerIndex, Player winner, int mysteryNumber)
    {
        Min = min;
        Max = max;
        Players = players ?? new List<Player>();
        CurrentPlayerIndex = currentPlayerIndex;
        Winner = winner;
        MysteryNumber = mysteryNumber;
    }

    public Game(int min, int max, IInputOutputService ioService)
    {
        Min = min;
        Max = max;
        MysteryNumber = new Random().Next(min, max + 1);
        Players = new List<Player>();
        CurrentPlayerIndex = 0;
        Winner = null;
        _ioService = ioService;
    }

    public void AddPlayer(string playerName)
    {
        Players.Add(new Player(playerName));
    }

    public void Start()
    {
        bool isGameOver = false;

        while (!isGameOver)
        {
            Player currentPlayer = Players[CurrentPlayerIndex];
            _ioService.WriteLine($"{currentPlayer.Name}'s turn:");
            DateTime startTime = DateTime.Now;

            int guess = _ioService.ReadValidNumber("Enter your guess: ");

            if (guess == MysteryNumber)
            {
                _ioService.WriteLine($"Congratulations {currentPlayer.Name}! You've guessed the mystery number {MysteryNumber}.");
                isGameOver = true;
                Winner = currentPlayer;
            }
            else
            {
                _ioService.WriteLine(guess < MysteryNumber ? "HI" : "LO");
            }

            DateTime endTime = DateTime.Now;
            currentPlayer.Attempts++;
            currentPlayer.TotalTime += (endTime - startTime).TotalSeconds;

            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
            SaveGameState();
        }

        DisplayScores();
        AnnounceChampion();
        File.Delete("gamestate.json");
    }

    private void DisplayScores()
    {
        _ioService.WriteLine("\nGame Over! Here are the scores:");
        foreach (var player in Players)
        {
            _ioService.WriteLine($"{player.Name}: {player.Attempts} attempts, Total Time: {player.TotalTime:F2} seconds");
        }
    }

    public void AnnounceChampion()
    {
        Player champion = Players.OrderBy(p => p.Attempts).ThenBy(p => p.TotalTime).First();
        _ioService.WriteLine($"\nChampion of the game is {champion.Name} with {champion.Attempts} attempts!");
    }

    private void SaveGameState()
    {
        try
        {
            string json = JsonSerializer.Serialize(this);
            File.WriteAllText("gamestate.json", json);
        }
        catch (Exception ex)
        {
            _ioService.WriteLine($"Error saving game state: {ex.Message}");
        }
    }
}
