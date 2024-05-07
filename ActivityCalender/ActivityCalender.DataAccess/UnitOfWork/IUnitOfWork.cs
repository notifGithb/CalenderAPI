using ActivityCalender.DataAccess.Etkinlikler;

namespace ActivityCalender.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IEtkinlikRepository EtkinlikRepository { get; }
        Task SaveChangesAsync();

    }
}
