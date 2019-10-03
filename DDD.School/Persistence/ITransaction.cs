using System;

namespace DDD.School.Persistence
{
    public interface ITransaction : IDisposable //TODO: leaky abstraction
    {
        void Commit();

        void Rollback();
    }
}