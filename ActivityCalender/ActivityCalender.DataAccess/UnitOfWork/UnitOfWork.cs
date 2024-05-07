using ActivityCalender.DataAccess.Etkinlikler;

namespace ActivityCalender.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ActivityCalenderContext _context;

        public UnitOfWork(ActivityCalenderContext context)
        {
            _context = context;
            EtkinlikRepository = new EtkinlikRepository(_context);
        }

        public IEtkinlikRepository EtkinlikRepository { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
