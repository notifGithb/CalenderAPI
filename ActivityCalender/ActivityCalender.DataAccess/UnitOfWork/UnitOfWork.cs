//using ActivityCalender.DataAccess.Etkinlikler;
//using ActivityCalender.DataAccess.Kullanicilar;

//namespace ActivityCalender.DataAccess.UnitOfWork
//{
//    public class UnitOfWork : IUnitOfWork
//    {
//        private readonly ActivityCalenderContext _context;

//        public UnitOfWork(ActivityCalenderContext context)
//        {
//            _context = context;
//            EtkinlikRepository = new EtkinlikRepository(_context);
//            KullaniciEtkinlikRepositroy = new KullaniciEtkinlikRepositroy(_context);
//            KullaniciRepository = new KullaniciRepository(_context);
//        }

//        public IEtkinlikRepository EtkinlikRepository { get; private set; }

//        public IKullaniciEtkinlikRepositroy KullaniciEtkinlikRepositroy { get; private set; }

//        public IKullaniciRepository KullaniciRepository { get; private set; }

//        public void Dispose()
//        {
//            _context.Dispose();
//            //GC.SuppressFinalize(this);
//        }
//        public async Task DisposeAsync()
//        {
//            await _context.DisposeAsync();
//        }

//        public async Task SaveChangesAsync()
//        {
//            await _context.SaveChangesAsync();
//        }

//        async ValueTask IAsyncDisposable.DisposeAsync()
//        {
//            await _context.DisposeAsync();
//        }
//    }
//}
