using System;
using System.Collections.Generic;
using BracketService.Models;

namespace BracketService.Services
{
    public interface IBracketGenerator
    {
        BracketStructure GenerateSingleElimination(int teamCount);
    }

    public class BracketGenerator : IBracketGenerator
    {
        public BracketStructure GenerateSingleElimination(int teamCount)
        {
            int actualSize = 1;
            while (actualSize < teamCount)
            {
                actualSize *= 2;
            }

            var structure = new BracketStructure();
            int roundCount = (int)Math.Log2(actualSize);

            int matchId = 1;
            for (int round = 1; round <= roundCount; round++)
            {
                int matchesInRound = (int)Math.Pow(2, roundCount - round);
                var roundObj = new Round
                {
                    RoundNumber = round,
                    Matches = new List<Match>()
                };

                for (int i = 0; i < matchesInRound; i++)
                {
                    var match = new Match
                    {
                        MatchId = $"m{matchId++}",
                        Team1Id = null,
                        Team2Id = null,
                        WinnerId = null,
                        Status = "pending"
                    };

                    if (round < roundCount)
                    {
                        int nextMatchIndex = i / 2;
                        match.NextMatchId = $"m{(int)(Math.Pow(2, roundCount - round - 1) + nextMatchIndex)}";
                    }

                    roundObj.Matches.Add(match);
                }

                structure.Rounds.Add(roundObj);
            }

            return structure;
        }
    }
}