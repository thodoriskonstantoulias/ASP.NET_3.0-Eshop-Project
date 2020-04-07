using Eshop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.DataAccess.Data.Repository.IRepository
{
    interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<SelectListItem> GetCategoryForDropDown();
        void Update(Category category);
    }
}
