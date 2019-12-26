using System;

namespace DDD.School
{
    public class StudentCourseStatus : BaseEntity<Guid>
    {
        private StudentCourseStatus(Guid id, Guid studentId, Guid courseId, Statuses status, DateTime createdAt)
        {
            this.Id = id;
            this.StudentId = studentId;
            this.CourseId = courseId;
            this.Status = status;
            this.CreatedAt = createdAt;
        }

        public Guid StudentId { get; private set; }
        public Guid CourseId { get; private set; }
        public Statuses Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public enum Statuses
        {
            Enrolled = 1,
            Withdrawn,
            Completed
        }

        public static StudentCourseStatus Enrolled(Student student, Course course) => new StudentCourseStatus(Guid.NewGuid(), student.Id, course.Id, Statuses.Enrolled, DateTime.UtcNow);
        public static StudentCourseStatus Withdrawn(Student student, Course course) => new StudentCourseStatus(Guid.NewGuid(), student.Id, course.Id, Statuses.Withdrawn, DateTime.UtcNow);
        public static StudentCourseStatus Completed(Student student, Course course) => new StudentCourseStatus(Guid.NewGuid(), student.Id, course.Id, Statuses.Completed, DateTime.UtcNow);
    }
}