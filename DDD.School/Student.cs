using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DDD.School
{
    public class Student : BaseEntity<Guid>, IAggregateRoot
    {
        public Student(Guid id, string firstname, string lastname)
        {
            this.Id = id;
            SetFirstname(firstname);
            SetLastname(lastname);
        }

        public string Firstname { get; private set; }

        public void SetFirstname(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));
            Firstname = value;
        }

        public string Lastname { get; private set; }
        
        public void SetLastname(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));
            Lastname = value;
        }

        private readonly HashSet<StudentCourseStatus> _courses = new HashSet<StudentCourseStatus>();
        public IReadOnlyCollection<StudentCourseStatus> Courses => _courses.ToImmutableArray();

        public void Enroll(Course course)
        {
            if(null == course)
                throw new ArgumentNullException(nameof(course));

            _courses.Add(new StudentCourseStatus(this, course, StudentCourseStatus.Statuses.Enrolled));
        }

        public void Withdraw(Course course)
        {
            if (null == course)
                throw new ArgumentNullException(nameof(course));
            if(_courses.All(c => c.CourseId != course.Id))
                throw new ArgumentException($"student {this.Id} not enrolled in course {course.Id}");

            _courses.RemoveWhere(c => c.CourseId == course.Id);
            _courses.Add(new StudentCourseStatus(this, course, StudentCourseStatus.Statuses.Withdrawn));
        }
    }
}