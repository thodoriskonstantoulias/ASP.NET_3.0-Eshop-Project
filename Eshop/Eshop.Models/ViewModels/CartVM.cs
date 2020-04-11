using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Models.ViewModels
{
    public class CartVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IList<Service> ServiceList { get; set; }
    }
}
