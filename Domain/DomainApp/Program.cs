using BracketService.Domain;
using BracketService.ValueObjects;
using BracketService.Domain.Enums;
using BracketService.Domain.Exceptions;

namespace DomainApp;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // Создание турнира и кронштейна с 4 участниками
            var tournamentId = Guid.NewGuid();
            var participantIds = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            Console.WriteLine("Creating bracket for tournament...");
            var bracket = Bracket.Generate(tournamentId, BracketType.SingleElimination, participantIds);
            
            Console.WriteLine($"✓ Bracket created:");
            Console.WriteLine($"  - Tournament ID: {bracket.TournamentId}");
            Console.WriteLine($"  - Type: {bracket.Type}");
            Console.WriteLine($"  - Total Rounds: {bracket.TotalRounds.Value}");
            Console.WriteLine($"  - Current Round: {bracket.CurrentRound.Value}");
            Console.WriteLine($"  - Status: {bracket.Status}");
            Console.WriteLine($"  - Created at: {bracket.CreatedAt:yyyy-MM-dd HH:mm:ss}\n");

            // Вывод информации о первом раунде
            PrintRoundInfo(bracket, 1);

            // Установка победителей в первом раунде
            Console.WriteLine("Setting match winners in Round 1...");
            var round1 = bracket.Rounds.First(r => r.RoundNumber == 1);
            var matches = round1.Matches.ToList();

            // Матч 1: Player 0 побеждает
            var match1 = matches[0];
            var player1Winner = match1.Player1Id;
            bracket.AdvanceWinner(match1.Id, player1Winner);
            Console.WriteLine($"✓ Match 1: {player1Winner} wins!");

            // Матч 2: Player 1 побеждает
            var match2 = matches[1];
            var player2Winner = match2.Player2Id;
            bracket.AdvanceWinner(match2.Id, player2Winner);
            Console.WriteLine($"✓ Match 2: {player2Winner} wins!\n");

            // Проверка автоматического создания второго раунда
            Console.WriteLine($"After Round 1 completion:");
            Console.WriteLine($"  - Current Round: {bracket.CurrentRound.Value}");
            Console.WriteLine($"  - Status: {bracket.Status}");
            Console.WriteLine($"  - Total Rounds in Bracket: {bracket.Rounds.Count}\n");

            // Вывод информации о втором раунде (финал)
            PrintRoundInfo(bracket, 2);

            // Финальный матч
            Console.WriteLine("Setting match winner in Round 2 (Finals)...");
            var round2 = bracket.Rounds.First(r => r.RoundNumber == 2);
            var finalMatch = round2.Matches.First();
            var finalWinner = finalMatch.Player1Id;
            bracket.AdvanceWinner(finalMatch.Id, finalWinner);
            Console.WriteLine($"✓ Finals: {finalWinner} is the tournament winner!\n");

            // Финальное состояние
            Console.WriteLine($"Tournament Completed:");
            Console.WriteLine($"  - Bracket Status: {bracket.Status}");
            Console.WriteLine($"  - Total Rounds Played: {bracket.Rounds.Count}");
            Console.WriteLine($"  - Champion: {finalWinner}\n");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void PrintRoundInfo(Bracket bracket, int roundNumber)
    {
        var round = bracket.Rounds.FirstOrDefault(r => r.RoundNumber == roundNumber);
        if (round == null)
        {
            Console.WriteLine($"Round {roundNumber} not available yet.\n");
            return;
        }

        Console.WriteLine($"Round {roundNumber} Matches:");
        var matchCount = 1;
        foreach (var match in round.Matches)
        {
            var winner = match.WinnerId?.ToString() ?? "TBD";
            Console.WriteLine($"  Match {matchCount}: {match.Player1Id} vs {match.Player2Id} - Winner: {winner} (Status: {match.Status})");
            matchCount++;
        }
        Console.WriteLine();
    }
    
}
