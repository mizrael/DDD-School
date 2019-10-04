using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace DDD.School.Tests.Unit
{
    public class StudentTests
    {
        [Fact]
        public void SetFirstname_should_fail_if_value_invalid()
        {
            var sut = new Student(Guid.NewGuid(), "firstname", "lastname");
            Assert.Throws<ArgumentNullException>(() => sut.SetFirstname(null));
            Assert.Throws<ArgumentNullException>(() => sut.SetFirstname(""));
            Assert.Throws<ArgumentNullException>(() => sut.SetFirstname(" "));
        }

        [Fact]
        public void SetFirstname_should_set_value_when_valid()
        {
            var expected = "new name";
            var sut = new Student(Guid.NewGuid(), "firstname", "lastname");
            sut.SetFirstname(expected);
            sut.Firstname.Should().Be(expected);
        }

        [Fact]
        public void SetLastname_should_fail_if_value_invalid()
        {
            var sut = new Student(Guid.NewGuid(), "firstname", "lastname");
            Assert.Throws<ArgumentNullException>(() => sut.SetLastname(null));
            Assert.Throws<ArgumentNullException>(() => sut.SetLastname(""));
            Assert.Throws<ArgumentNullException>(() => sut.SetLastname(" "));
        }

        [Fact]
        public void SetLastname_should_set_value_when_valid()
        {
            var expected = "new name";
            var sut = new Student(Guid.NewGuid(), "firstname", "lastname");
            sut.SetLastname(expected);
            sut.Lastname.Should().Be(expected);
        }

        [Fact]
        public void Enroll_should_add_student_to_course()
        {
            var course = new Course(Guid.NewGuid(), "course");
            var sut = new Student(Guid.NewGuid(), "firstname", "lastname");

            sut.Courses.Should().BeEmpty();
            sut.Enroll(course);
            sut.Courses.Count.Should().Be(1);
            sut.Courses.Should().Contain(c => c.Status == StudentCourseStatus.Statuses.Enrolled && c.StudentId == sut.Id && c.CourseId == course.Id);
        }

        [Fact]
        public void Enroll_should_add_student_to_course_only_once()
        {
            var course = new Course(Guid.NewGuid(), "course");
            var sut = new Student(Guid.NewGuid(), "firstname", "lastname");
            
            sut.Enroll(course);
            sut.Courses.Count.Should().Be(1);

            sut.Enroll(course);
            sut.Courses.Count.Should().Be(1);
        }

        [Fact]
        public void Enroll_should_add_student_to_course_if_he_has_withdrawn()
        {
            var course = new Course(Guid.NewGuid(), "course");
            var sut = new Student(Guid.NewGuid(), "firstname", "lastname");

            sut.Enroll(course);
            sut.Withdraw(course);
            sut.Enroll(course);

            sut.Courses.ElementAt(0).Status.Should().Be(StudentCourseStatus.Statuses.Enrolled);
            sut.Courses.ElementAt(1).Status.Should().Be(StudentCourseStatus.Statuses.Withdrawn);
            sut.Courses.ElementAt(2).Status.Should().Be(StudentCourseStatus.Statuses.Enrolled);
        }

        [Fact]
        public void Withdraw_should_update_course_status()
        {
            var course = new Course(Guid.NewGuid(), "course");
            var sut = new Student(Guid.NewGuid(), "firstname", "lastname");

            sut.Enroll(course);
            sut.Withdraw(course);

            sut.Courses.ElementAt(0).Status.Should().Be(StudentCourseStatus.Statuses.Enrolled);
            sut.Courses.ElementAt(1).Status.Should().Be(StudentCourseStatus.Statuses.Withdrawn);
        }

        [Fact]
        public void Withdraw_should_throw_if_student_not_enrolled()
        {
            var course = new Course(Guid.NewGuid(), "course");
            var sut = new Student(Guid.NewGuid(), "firstname", "lastname");

            var ex = Assert.Throws<ArgumentException>(() => sut.Withdraw(course));
            ex.Message.Should().Be($"student {sut.Id} not enrolled in course {course.Id}");
        }

        [Fact]
        public void Withdraw_should_throw_if_student_has_completed_course()
        {
            var course = new Course(Guid.NewGuid(), "course");
            var sut = new Student(Guid.NewGuid(), "firstname", "lastname");

            sut.Enroll(course);
            sut.Complete(course);

            var ex = Assert.Throws<ArgumentException>(() => sut.Withdraw(course));
            ex.Message.Should().Be($"student {sut.Id} has completed course {course.Id} already");
        }

        [Fact]
        public void Complete_should_throw_if_student_not_enrolled()
        {
            var course = new Course(Guid.NewGuid(), "course");
            var sut = new Student(Guid.NewGuid(), "firstname", "lastname");
            
            var ex = Assert.Throws<ArgumentException>(() => sut.Complete(course));
            ex.Message.Should().Be($"student {sut.Id} not enrolled in course {course.Id}");
        }

        [Fact]
        public void Complete_should_throw_if_student_has_withdrawn()
        {
            var course = new Course(Guid.NewGuid(), "course");
            var sut = new Student(Guid.NewGuid(), "firstname", "lastname");

            sut.Enroll(course);
            sut.Withdraw(course);

            var ex = Assert.Throws<ArgumentException>(() => sut.Complete(course));
            ex.Message.Should().Be($"student {sut.Id} has withdrawn from course {course.Id}");
        }

        [Fact]
        public void Complete_should_succeed_when_student_is_enrolled()
        {
            var course = new Course(Guid.NewGuid(), "course");
            var sut = new Student(Guid.NewGuid(), "firstname", "lastname");

            sut.Enroll(course);
            sut.Complete(course);
            sut.Courses.ElementAt(0).Status.Should().Be(StudentCourseStatus.Statuses.Enrolled);
            sut.Courses.ElementAt(1).Status.Should().Be(StudentCourseStatus.Statuses.Completed);
        }
    }
}