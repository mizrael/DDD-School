using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DDD.School
{
    public class PagedCollection<T>
    {
        public static readonly PagedCollection<T> Empty = new PagedCollection<T>(Enumerable.Empty<T>(), 0L, 0L, 0L);

        public PagedCollection(IEnumerable<T> items, long page, long pageSize, long totalItemsCount)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            this.Items = items.ToImmutableArray();
            this.Page = page;
            this.PageSize = pageSize;
            this.TotalItemsCount = totalItemsCount;
            this.TotalPagesCount = pageSize != 0L ? (long)Math.Ceiling(totalItemsCount / (Decimal)pageSize) : 0L;
        }

        public long Page { get; }

        public long PageSize { get; }

        public long TotalPagesCount { get; }

        public long TotalItemsCount { get; }

        public IReadOnlyCollection<T> Items { get; }
    }
}