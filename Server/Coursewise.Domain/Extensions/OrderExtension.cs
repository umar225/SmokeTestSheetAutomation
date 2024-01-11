using Coursewise.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Extensions
{
    
    public static class OrderExtension
    {
        public static double GetProductsSum(this Order order)
        {
            double totalAmount = ItemsSum(order.Products.Select(x => x.TotalPrice));
            return totalAmount;
        }
        public static double ItemsSum(IEnumerable<double> items)
        {
            return items.Sum();
        }

        public static void Setup(this Product orderProduct, CourseDto product)
        {
            orderProduct.Name = product.Name;
            orderProduct.Price = product.Price;
            orderProduct.Provider = product.ProviderName;
        }
    }
}
