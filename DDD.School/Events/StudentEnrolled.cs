using System;

namespace DDD.School.Events
{
    public class StudentEnrolled : IDomainEvent
    {
        public StudentEnrolled(Student student, Course course)
        {
            this.Student = student ?? throw new ArgumentNullException(nameof(student));
            this.Course = course ?? throw new ArgumentNullException(nameof(course));
        }
        public Student Student { get; }
        public Course Course { get; }
    }
}
