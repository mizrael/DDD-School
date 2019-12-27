using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DDD.School.Persistence.SQL.Tests.Fixtures;
using DDD.School.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace DDD.School.Persistence.SQL.Tests.Integration
{

    internal class FakeEvent : IDomainEvent
    {
        public string Text;
    }
}
