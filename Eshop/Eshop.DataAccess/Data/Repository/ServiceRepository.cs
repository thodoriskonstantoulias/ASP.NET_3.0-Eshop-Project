using Eshop.DataAccess.Data.Repository.IRepository;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eshop.DataAccess.Data.Repository
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Service service)
        {
            var objFromDb = _context.Services.FirstOrDefault(s => s.Id == service.Id);
            objFromDb.Name = service.Name;
            objFromDb.Price = service.Price;
            objFromDb.LongDesc = service.LongDesc;
            objFromDb.ImageUrl = service.ImageUrl;
            objFromDb.CategoryId = service.CategoryId;
            objFromDb.FrequencyId = service.FrequencyId;

            _context.SaveChanges();
        }
    }
}
