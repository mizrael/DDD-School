using System;
using DDD.School.Events;
using DDD.School.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DDD.School.Tests.Unit
{

    public class MessageTests
    {
        [Fact]
        public void FromDomainEvent_should_create_message()
        {
            var course = new Course(Guid.NewGuid(), "course");
            var student = new Student(Guid.NewGuid(), "firstname", "lastname");
            var @event = new StudentEnrolled(student, course);

            var eventSerializer = Substitute.For<IEventSerializer>();
            var expectedPayload = $"{student.Id}_{course.Id}";
            eventSerializer.Serialize(@event)
                .Returns(expectedPayload);

            var sut = Message.FromDomainEvent(@event, eventSerializer);
            sut.Should().NotBeNull();
            sut.Payload.Should().Be(expectedPayload);
            sut.ProcessedAt.Should().BeNull();
            sut.Type.Should().Be(typeof(StudentEnrolled).FullName);
        }
    }
}