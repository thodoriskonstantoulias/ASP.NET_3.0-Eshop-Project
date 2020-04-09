using Eshop.DataAccess.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
            Frequency = new FrequencyRepository(_context);
            Service = new ServiceRepository(_context);
        }

        public ICategoryRepository Category { get; private set; }

        public IFrequencyRepository Frequency { get; private set; }

        public IServiceRepository Service { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save() 
        {
            _context.SaveChanges();
        }
    }
}
