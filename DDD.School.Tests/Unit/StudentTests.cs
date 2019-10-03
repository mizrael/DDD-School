using System;
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
        public void Withdraw_should_update_course_status()
        {
            var course = new Course(Guid.NewGuid(), "course");
            var sut = new Student(Guid.NewGuid(), "firstname", "lastname");

            sut.Enroll(course);
            sut.Withdraw(course);

            sut.Courses.Count.Should().Be(1);
            sut.Courses.Should().Contain(c => c.Status == StudentCourseStatus.Statuses.Withdrawn && c.StudentId == sut.Id && c.CourseId == course.Id);
        }

        [Fact]
        public void Withdraw_should_throw_if_student_not_enrolled()
        {
            var course = new Course(Guid.NewGuid(), "course");
            var sut = new Student(Guid.NewGuid(), "firstname", "lastname");

            var ex = Assert.Throws<ArgumentException>(() => sut.Withdraw(course));
            ex.Message.Should().Contain(course.Id.ToString());
        }

    }
}