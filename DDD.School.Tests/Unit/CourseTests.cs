using System;
using FluentAssertions;
using Xunit;

namespace DDD.School.Tests.Unit
{
    public class CourseTests
    {
        [Fact]
        public void SetTitle_should_fail_if_value_invalid()
        {
            var sut = new Course(Guid.NewGuid(), "title");
            Assert.Throws<ArgumentNullException>(() => sut.SetTitle(null));
            Assert.Throws<ArgumentNullException>(() => sut.SetTitle(""));
            Assert.Throws<ArgumentNullException>(() => sut.SetTitle(" "));
        }

        [Fact]
        public void SetTitle_should_set_value_when_valid()
        {
            var expected = "new title";
            var sut = new Course(Guid.NewGuid(), "title");
            sut.SetTitle(expected);
            sut.Title.Should().Be(expected);
        }
    }
}