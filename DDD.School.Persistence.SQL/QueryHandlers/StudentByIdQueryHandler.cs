using Dapper;
using DDD.School.Queries;
using MediatR;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.School.Persistence.SQL.QueryHandlers
{

    public class StudentByIdQueryHandler : IRequestHandler<StudentById, StudentDetails>
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        private const string query = @"SELECT TOP 1 [Id]
                                          ,[FirstName]
                                          ,[LastName]
                                      FROM [dbo].[Students]
                                      WHERE [Id] = @Id;";

        public StudentByIdQueryHandler(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider ?? throw new ArgumentNullException(nameof(connectionStringProvider));
        }

        public async Task<StudentDetails> Handle(StudentById request, CancellationToken cancellationToken)
        {
            if (null == request)
                throw new ArgumentNullException(nameof(request));

            await using var conn = new SqlConnection(_connectionStringProvider.ConnectionString);
            var result = await conn.QueryFirstOrDefaultAsync<StudentDetails>(query, new { Id = request.Id });
            return result;
        }
    }
}
