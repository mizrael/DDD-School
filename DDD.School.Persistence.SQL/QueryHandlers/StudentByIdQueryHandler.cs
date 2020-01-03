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
                                      WHERE [Id] = @Id;
                                        SELECT C.Id, C.Title
                                        FROM
                                        (SELECT Row_Number() OVER(PARTITION by CourseId ORDER BY CreatedAt DESC) AS ri, CourseId, [Status], CreatedAt
                                        FROM StudentCourses SC
                                        WHERE StudentId = @Id) T 
                                        JOIN Courses C ON C.Id = T.CourseId
                                        WHERE T.Status = 1 AND ri = 1";

        public StudentByIdQueryHandler(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider ?? throw new ArgumentNullException(nameof(connectionStringProvider));
        }

        public async Task<StudentDetails> Handle(StudentById request, CancellationToken cancellationToken)
        {
            if (null == request)
                throw new ArgumentNullException(nameof(request));

            await using var conn = new SqlConnection(_connectionStringProvider.ConnectionString);

            var results = await conn.QueryMultipleAsync(query, new { Id = request.Id });
            
            var studentRow = await results.ReadSingleOrDefaultAsync();
            var courses = await results.ReadAsync<CourseArchiveItem>();

            return new StudentDetails(studentRow.Id, studentRow.FirstName, studentRow.LastName, courses);
        }
    }
}
