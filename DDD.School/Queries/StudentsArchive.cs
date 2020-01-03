using System;
using MediatR;

namespace DDD.School.Queries
{
    public class StudentsArchive : IRequest<PagedCollection<StudentArchiveItem>>
    {
        public StudentsArchive(long page, long pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public long Page { get; }

        public long PageSize { get; }
    }
    
    public class StudentArchiveItem
    {
        public StudentArchiveItem(Guid id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }

}
