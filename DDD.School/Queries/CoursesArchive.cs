using System;
using MediatR;

namespace DDD.School.Queries
{
    public class CoursesArchive : IRequest<PagedCollection<CourseArchiveItem>>
    {
        public CoursesArchive(long page, long pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public long Page { get; }

        public long PageSize { get; }
    }

    public class CourseArchiveItem
    {
        public CourseArchiveItem(Guid id, string title)
        {
            Id = id;
            Title = title;
        }

        public Guid Id { get; }
        public string Title { get; }
    }
}
