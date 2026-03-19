using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using BracketService.Data;
using BracketService.Models;
using BracketService.Services;
using Eventide.Contracts.Bracket;
using BracketServiceModels = BracketService.Models;

namespace BracketService.Services
{
    public class BracketGrpcService : Eventide.Contracts.Bracket.BracketService.BracketServiceBase
    {
        private readonly BracketDbContext _dbContext;
        private readonly IBracketGenerator _generator;
        private readonly ILogger<BracketGrpcService> _logger;

        public BracketGrpcService(
            BracketDbContext dbContext,
            IBracketGenerator generator,
            ILogger<BracketGrpcService> logger)
        {
            _dbContext = dbContext;
            _generator = generator;
            _logger = logger;
        }

        public override async Task<BracketResponse> GenerateEmptyBracket(
            GenerateBracketRequest request,
            ServerCallContext context)
        {
            try
            {
                _logger.LogInformation($"Generating bracket for tournament {request.TournamentId}");

                var existing = await _dbContext.Brackets
                    .FirstOrDefaultAsync(b => b.TournamentId == Guid.Parse(request.TournamentId));
                
                if (existing != null)
                {
                    return new BracketResponse { BracketId = existing.Id.ToString() };
                }

                var structure = _generator.GenerateSingleElimination(request.MaxTeams);
                
                var bracket = new BracketServiceModels.Bracket
                {
                    Id = Guid.NewGuid(),
                    TournamentId = Guid.Parse(request.TournamentId),
                    Format = request.Format.ToString(),
                    Structure = structure,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.Brackets.Add(bracket);
                await _dbContext.SaveChangesAsync();

                return new BracketResponse
                {
                    BracketId = bracket.Id.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating bracket");
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public override async Task<Eventide.Contracts.Bracket.Bracket> GetBracket(
            GetBracketRequest request,
            ServerCallContext context)
        {
            var bracket = await _dbContext.Brackets
                .FirstOrDefaultAsync(b => b.Id == Guid.Parse(request.BracketId));

            if (bracket == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Bracket not found"));
            }

            return new Eventide.Contracts.Bracket.Bracket
            {
                Id = bracket.Id.ToString(),
                TournamentId = bracket.TournamentId.ToString(),
                Format = Enum.Parse<TournamentFormat>(bracket.Format),
                StructureJson = bracket.StructureJson,
                CreatedAt = bracket.CreatedAt.ToString("o")
            };
        }
    }
}