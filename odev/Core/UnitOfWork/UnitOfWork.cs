using Data.Context;
using Data.IUnitOfWork;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private bool _disposed;
        public UnitOfWork(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("The UnitOfWork is already disposed and cannot be used anymore.");
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            _disposed = true;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void TrackerClear()
        {
            _context.ChangeTracker.Clear();
        }
    }
}