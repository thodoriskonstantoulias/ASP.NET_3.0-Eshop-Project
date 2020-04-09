using Eshop.DataAccess.Data.Repository.IRepository;
using Eshop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eshop.DataAccess.Data.Repository
{
    public class FrequencyRepository : Repository<Frequency>, IFrequencyRepository
    {
        private readonly ApplicationDbContext _context;

        public FrequencyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetFrequencyForDropDown()
        {
            return _context.Frequencies.Select(f => new SelectListItem
            {
                Text = f.Name,
                Value = f.Id.ToString()
            });
        }

        public void Update(Frequency frequency)
        {
            Frequency getFreq = _context.Frequencies.FirstOrDefault(f => f.Id == frequency.Id);
            getFreq.Name = frequency.Name;
            getFreq.FrequencyCount = frequency.FrequencyCount;

            _context.SaveChanges();
        }
    }
}
