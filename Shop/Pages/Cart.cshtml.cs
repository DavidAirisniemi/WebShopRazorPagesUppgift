using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopUI.DataAccess;
using ShopUI.Models;

namespace ShopUI.Pages
{
    public class CartModel : PageModel
    {

        public readonly IDataAccess<Customer> _customerDataAccess;

        public CartModel(IDataAccess<Customer> customerDataAccess)
        {
            this._customerDataAccess = customerDataAccess;
        }

        public Customer _customer { get; set; }

        public IActionResult OnGet()
        {
            if(HttpContext.Session.GetInt32("LoginId") != 0 && HttpContext.Session.GetInt32("LoginId") != null)
            {
                _customer = _customerDataAccess.GetById((int)HttpContext.Session.GetInt32("LoginId"));
                return Page();
            }
            return RedirectToPage("/Index");
        }


        public IActionResult OnPostOrder()
        {
            OnGet();

            if(_customer._shoppingCart._products.Count == 0)
            {
                return Page();
            }

            //Ser dumt ut men är tvungen för att det ska fungera
            List<Product> _orderProducts = new List<Product>();
            _orderProducts.AddRange(_customer._shoppingCart._products);
            _customer._shoppingCart._products.Clear();
            _customer._orders.Add(new Order(_customer._orders.Count, _orderProducts));

            List<Customer> updateCList = _customerDataAccess.GetAll();
            updateCList[_customer._id - 1] = _customer;
            _customerDataAccess.SerializeItems(updateCList);
            return RedirectToPage("/Index");
        }





    }
}
