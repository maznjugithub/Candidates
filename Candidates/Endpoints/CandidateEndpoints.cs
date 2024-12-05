
using Dapper;
using Candidates.Models;
using Candidates.Service;

namespace Candidates.Endpoints;
public static class CandidatesEndpoints
{
	public static async void MapCandidatesEndpoints(this IEndpointRouteBuilder builder)
	{
		var group = builder.MapGroup("Candidates");
		// Endpoint to fetch all candidates
		group.MapGet("", async (SqlconnectionFactory sqlConnectionFactory) =>
		{
			using var connection = sqlConnectionFactory.Create();
			const string sql = "SELECT CandidatesId, Email, FirstName, LastName, PhoneNumber, CallTimeInterval, LinkedInUrl, GitHubUrl, Comments FROM Candidates";
			var candidates = await connection.QueryAsync<Candidate>(sql);
			return Results.Ok(candidates);
		});

		// Endpoint to fetch a single candidate by ID
		group.MapGet("{id}", async (int id, SqlconnectionFactory sqlConnectionFactory) =>
		{
			using var connection = sqlConnectionFactory.Create();
			const string sql = """
                SELECT CandidatesId, Email, FirstName, LastName, PhoneNumber, CallTimeInterval, LinkedInUrl, GitHubUrl, Comments
                FROM Candidates
                WHERE CandidatesId = @CandidatesId
            """;
			var candidate = await connection.QuerySingleOrDefaultAsync<Candidate>(
				sql,
				new { CandidatesId = id }); // Pass the parameter here

			return candidate is not null ? Results.Ok(candidate) : Results.NotFound();
		});

		group.MapPost("", async (Candidate candidate, SqlconnectionFactory sqlConnectionFactory) =>
		{
			using var connection = sqlConnectionFactory.Create();
			const string sql = """
        INSERT INTO Candidates (Email, FirstName, LastName, PhoneNumber, CallTimeInterval, LinkedInUrl, GitHubUrl, Comments) 
        VALUES (@Email, @FirstName, @LastName, @PhoneNumber, @CallTimeInterval, @LinkedInUrl, @GitHubUrl, @Comments)
    """;
			await connection.ExecuteAsync(sql, candidate);
			return Results.Ok();
		});
		group.MapPut("{id}", async (int id, Candidate candidate, SqlconnectionFactory sqlConnectionFactory) =>
		{
			using var connection = sqlConnectionFactory.Create();
			const string sql = """
        UPDATE Candidates
        SET 
            Email = @Email,
            FirstName = @FirstName,
            LastName = @LastName,
            PhoneNumber = @PhoneNumber,
            CallTimeInterval = @CallTimeInterval,
            LinkedInUrl = @LinkedInUrl,
            GitHubUrl = @GitHubUrl,
            Comments = @Comments
        WHERE CandidatesId = @CandidatesId
    """;

			var parameters = new
			{
				candidate.Email,
				candidate.FirstName,
				candidate.LastName,
				candidate.PhoneNumber,
				candidate.CallTimeInterval,
				candidate.LinkedInUrl,
				candidate.GitHubUrl,
				candidate.Comments,
				CandidatesId = id
			};

			await connection.ExecuteAsync(sql, parameters);
			return Results.NoContent();
		});

		group.MapDelete("{id}", async (int id, SqlconnectionFactory sqlConnectionFactory) =>
		{
			using var connection = sqlConnectionFactory.Create();
			const string sql = "DELETE FROM Candidates WHERE CandidatesId = @CandidatesId";
			await connection.ExecuteAsync(sql, new { CandidatesId = id });
			return Results.NoContent();
		});

	}
}
