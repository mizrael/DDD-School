using System;

namespace DDD.School
{
    public class StudentCourseStatus : ValueObject<StudentCourseStatus>
    {
        public StudentCourseStatus(Student student, Course course, Statuses status)
        {
            if(null == student)
                throw new ArgumentNullException(nameof(student));
            if (null == course)
                throw new ArgumentNullException(nameof(course));

            this.StudentId = student.Id;
            this.CourseId = course.Id;
            this.Status = status;
        }

        protected override bool EqualsCore(StudentCourseStatus other)
        {
            return other != null &&
                   StudentId == other.StudentId &&
                   CourseId == other.CourseId &&
                   Status == other.Status;
        }

        protected override int GetHashCodeCore()
        {
            return HashCode.Combine(StudentId, CourseId, Status);
        }

        public Guid StudentId { get; }
        public Guid CourseId { get; }
        public Statuses Status { get; }
        public enum Statuses
        {
            None,
            Enrolled,
            Withdrawn,
            Completed
        }
    }
}