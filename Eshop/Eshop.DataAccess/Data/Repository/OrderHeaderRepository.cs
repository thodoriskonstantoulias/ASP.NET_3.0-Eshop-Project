using Eshop.DataAccess.Data.Repository.IRepository;
using Eshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eshop.DataAccess.Data.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void ChangeOrderStatus(int orderHeaderId, string status)
        {
            var orderHeader = _context.OrderHeaders.FirstOrDefault(o => o.Id == orderHeaderId);
            orderHeader.Status = status;

            _context.SaveChanges();
        }
    }
}
