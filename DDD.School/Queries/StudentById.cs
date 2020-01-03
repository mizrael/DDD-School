using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MediatR;

namespace DDD.School.Queries
{
    public class StudentById : IRequest<StudentDetails>
    {
        public StudentById(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
         
    public class StudentDetails
    {
        public StudentDetails(Guid id, string firstName, string lastName, IEnumerable<CourseArchiveItem> courses)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Courses = (courses ?? Enumerable.Empty<CourseArchiveItem>()).ToImmutableArray();
        }

        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public IReadOnlyCollection<CourseArchiveItem> Courses { get; }
    }
}
