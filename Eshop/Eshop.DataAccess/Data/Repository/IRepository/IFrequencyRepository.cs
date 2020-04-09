using Eshop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.DataAccess.Data.Repository.IRepository
{
    public interface IFrequencyRepository : IRepository<Frequency>
    {
        void Update(Frequency frequency);
        IEnumerable<SelectListItem> GetFrequencyForDropDown();
    }
}
