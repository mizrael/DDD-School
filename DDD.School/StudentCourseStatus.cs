using System;

namespace DDD.School
{
    public class StudentCourseStatus : ValueObject<StudentCourseStatus>
    {
        private StudentCourseStatus() { }

        public StudentCourseStatus(Student student, Course course, Statuses status, DateTime date)
        {
            if(null == student)
                throw new ArgumentNullException(nameof(student));
            if (null == course)
                throw new ArgumentNullException(nameof(course));

            this.StudentId = student.Id;
            this.CourseId = course.Id;
            this.Status = status;
            Date = date;
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

        public Guid StudentId { get; private set; }
        public Guid CourseId { get; private set; }
        public Statuses Status { get; private set; }
        public DateTime Date { get; private set; }

        public enum Statuses
        {
            Enrolled = 1,
            Withdrawn,
            Completed
        }
    }
}