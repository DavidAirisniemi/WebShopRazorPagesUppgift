using System.Collections.Generic;

namespace ShopUI.Models
{
    public class ShoppingCart
    {
        public int _id { get; set; }
        public List<Product> _products { get; set; }

        public ShoppingCart(int id)
        {
            _id = id;
            if (_products == null)
            {
                _products = new List<Product>();
            }
            
        }
        public void AddProductToCart(Product product)
        {
            _products.Add(product);
        }
    }
}
