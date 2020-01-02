using System;
using System.Threading;
using System.Threading.Tasks;
using DDD.School.Queries;
using MediatR;
using Dapper;
using System.Data.SqlClient;
using System.Linq;

namespace DDD.School.Persistence.SQL.QueryHandlers
{

    public class CourseByIdQueryHandler : IRequestHandler<CourseById, CourseDetails>
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        private const string query = @"SELECT TOP 1 [Id]
                                          ,[Title]
                                      FROM [dbo].[Courses]
                                      WHERE [Id] = @Id;";

        public CourseByIdQueryHandler(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider ?? throw new ArgumentNullException(nameof(connectionStringProvider));
        }

        public async Task<CourseDetails> Handle(CourseById request, CancellationToken cancellationToken)
        {
            if (null == request)
                throw new ArgumentNullException(nameof(request));

            await using var conn = new SqlConnection(_connectionStringProvider.ConnectionString);
            var result = await conn.QueryFirstAsync<CourseDetails>(query, new { Id = request.Id });
            return result;
        }
    }
}
