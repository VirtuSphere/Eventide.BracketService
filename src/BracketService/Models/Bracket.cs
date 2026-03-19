using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace BracketService.Models
{
    public class Bracket
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public Guid TournamentId { get; set; }
        
        [Required]
        public string Format { get; set; } = string.Empty;
        
        [Required]
        [Column(TypeName = "jsonb")]
        public string StructureJson { get; set; } = "{}";
        
        public DateTime CreatedAt { get; set; }
        
        [NotMapped]
        public BracketStructure Structure
        {
            get => JsonSerializer.Deserialize<BracketStructure>(StructureJson) ?? new BracketStructure();
            set => StructureJson = JsonSerializer.Serialize(value);
        }
    }

    public class BracketStructure
    {
        public List<Round> Rounds { get; set; } = new();
    }

    public class Round
    {
        public int RoundNumber { get; set; }
        public List<Match> Matches { get; set; } = new();
    }

    public class Match
    {
        public string MatchId { get; set; } = string.Empty;
        public Guid? Team1Id { get; set; }
        public Guid? Team2Id { get; set; }
        public string? NextMatchId { get; set; }
        public int? WinnerId { get; set; }
        public string Status { get; set; } = "pending";
    }
}