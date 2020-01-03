using System;
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
        public StudentDetails(Guid id, string firstName, string lastName)
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
