using System;

namespace DDD.School
{
    public class Course : BaseAggregateRoot<Guid>
    {
        public Course(Guid id, string title)
        {
            this.Id = id;
            SetTitle(title);
        }

        public string Title { get; private set; }

        public void SetTitle(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));
            Title = value;
        }
    }
}
