using DDD.School.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.School.Persistence.SQL
{
    public class SchoolUnitOfWork : BaseUnitOfWork<SchoolDbContext>, ISchoolUnitOfWork
    {
        public ICoursesRepository CoursesRepository { get; }
        public IStudentsRepository StudentsRepository { get; }
        public IMessagesRepository MessagesRepository { get; }

        private readonly IEventSerializer _eventSerializer;

        public SchoolUnitOfWork(SchoolDbContext dbContext,
            ICoursesRepository coursesRepository,
            IStudentsRepository studentsRepository,
            IMessagesRepository messagesRepository,
            IEventSerializer eventSerializer) : base(dbContext)
        {
            CoursesRepository = coursesRepository ?? throw new ArgumentNullException(nameof(coursesRepository));
            StudentsRepository = studentsRepository ?? throw new ArgumentNullException(nameof(studentsRepository));
            MessagesRepository = messagesRepository ?? throw new ArgumentNullException(nameof(messagesRepository));
            _eventSerializer = eventSerializer ?? throw new ArgumentNullException(nameof(eventSerializer));
        }

        protected override Task BeforeCommitAsync(CancellationToken cancellationToken)
        {
            var entities = this.DbContext.ChangeTracker.Entries()
                                         .Where(e => e.Entity is IHasEvents c && c.Events.Any())
                                         .Select(e => e.Entity as IHasEvents)
                                         .ToArray();
            foreach (var entity in entities)
            {
                var messages = entity.Events.Select(e => Message.FromDomainEvent(e, _eventSerializer))
                                            .ToArray();
                entity.ClearEvents();

                this.DbContext.AddRange(messages);
            }

            return Task.CompletedTask;
        }
    }
}