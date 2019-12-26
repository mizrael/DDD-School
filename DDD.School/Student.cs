using DDD.School.Events;
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

        private readonly List<StudentCourseStatus> _courses = new List<StudentCourseStatus>();
        public IReadOnlyCollection<StudentCourseStatus> Courses => _courses.ToImmutableArray();

        public void Enroll(Course course)
        {
            if(null == course)
                throw new ArgumentNullException(nameof(course));

            var oldCourses = _courses.Where(c => c.CourseId == course.Id).ToArray();

            var isEmpty = !oldCourses.Any();
            var hasWithdrawn = !isEmpty && oldCourses.OrderByDescending(c => c.Date).First().Status == StudentCourseStatus.Statuses.Withdrawn;

            if (isEmpty || hasWithdrawn)
            {
                _courses.Add(new StudentCourseStatus(this, course, StudentCourseStatus.Statuses.Enrolled, DateTime.UtcNow));
                this.AddEvent(new StudentEnrolled(this, course));
            }
        }

        public void Withdraw(Course course)
        {
            if (null == course)
                throw new ArgumentNullException(nameof(course));

            var oldCourses = _courses.Where(c => c.CourseId == course.Id).ToArray();

            var isEmpty = !oldCourses.Any();

            var isCompleted = !isEmpty && oldCourses.OrderByDescending(c => c.Date).First().Status == StudentCourseStatus.Statuses.Completed;
            if(isCompleted)
                throw new ArgumentException($"student {this.Id} has completed course {course.Id} already");

            var isEnrolled = !isEmpty && oldCourses.OrderByDescending(c => c.Date).First().Status == StudentCourseStatus.Statuses.Enrolled;
            if (!isEnrolled)
                throw new ArgumentException($"student {this.Id} not enrolled in course {course.Id}");
            
            _courses.Add(new StudentCourseStatus(this, course, StudentCourseStatus.Statuses.Withdrawn, DateTime.UtcNow));
            this.AddEvent(new StudentWithdrawn(this, course));
        }

        public void Complete(Course course)
        {
            if (null == course)
                throw new ArgumentNullException(nameof(course));

            var oldCourses = _courses.Where(c => c.CourseId == course.Id).ToArray();
            if (!oldCourses.Any())
                throw new ArgumentException($"student {this.Id} not enrolled in course {course.Id}");
            if(oldCourses.Any(c => c.Status == StudentCourseStatus.Statuses.Withdrawn))
                throw new ArgumentException($"student {this.Id} has withdrawn from course {course.Id}");

            _courses.Add(new StudentCourseStatus(this, course, StudentCourseStatus.Statuses.Completed, DateTime.UtcNow));
            this.AddEvent(new CourseCompleted(this, course));
        }
    }
}