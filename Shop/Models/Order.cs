using System.Collections.Generic;

namespace ShopUI.Models
{
    public class Order
    {
        public int _id { get; set; }
        public bool _isPaid { get; set; }
        public List<Product> _products { get; set; }

        public Order(int id, List<Product> products)
        {
            _id = id;
            _products = products;
            _isPaid = false;
        }
    }   
}
