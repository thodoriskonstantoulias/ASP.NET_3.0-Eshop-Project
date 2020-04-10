using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Category> CategoryList { get; set; }
        public IEnumerable<Service> ServiceList { get; set; }

    }
}
