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
    public class CoursesArchiveQueryHandler: IRequestHandler<CoursesArchive, PagedCollection<CourseArchiveItem>>
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        private const string query = @"SELECT [Id]
                                          ,[Title]
                                      FROM [dbo].[Courses]
                                      Order by [Title]
                                      OFFSET @offset Rows
                                      Fetch NEXT @pageSize ROWS ONLY;
                                      SELECT COUNT(1) as TotalCount FROM [dbo].[Courses];";

        public CoursesArchiveQueryHandler(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider ?? throw new ArgumentNullException(nameof(connectionStringProvider));
        }

        public async Task<PagedCollection<CourseArchiveItem>> Handle(CoursesArchive request, CancellationToken cancellationToken)
        {
            if (null == request)
                throw new ArgumentNullException(nameof(request));

            await using var conn = new SqlConnection(_connectionStringProvider.ConnectionString);
            var results = await conn.QueryMultipleAsync(query, new { offset = request.Page * request.PageSize, pageSize = request.PageSize });

            var items = (await results.ReadAsync()).Select(r => new CourseArchiveItem(r.Id, r.Title));
            var totalCount = await results.ReadSingleAsync<int>();

            return new PagedCollection<CourseArchiveItem>(items, request.Page, request.PageSize, totalCount);
        }
    }
}
