using Newtonsoft.Json;
using ShopUI.DataSource;
using ShopUI.Models;
using System.Collections.Generic;
using System.IO;

namespace ShopUI.DataAccess
{
    public class CustomerDataAccess : IDataAccess<Customer>
    {
        
        public JsonDataSource _dataSource { get; set; }


        public CustomerDataAccess(JsonDataSource data)
        {
            this._dataSource = data;
        }

        public List<Customer> GetAll()
        {
            List<Customer> customer = JsonConvert.DeserializeObject<List<Customer>>(_dataSource.GetCustomers());
            if (customer == null)
            {
                customer = new List<Customer>();
            }
            return customer;
        }

        public void SerializeItems(List<Customer> customer)
        {
            string json = JsonConvert.SerializeObject(customer, Formatting.Indented);
            File.WriteAllText(@"C:\Users\David!\source\repos\WebShopRazorPagesUppgift\Shop\wwwroot\data\Customer.json", json);
        }

        public Customer GetById(int id)
        {
            foreach (Customer product in GetAll())
            {
                if (product._id == id)
                {
                    return product;
                }
            }
            return null;
        }
    }
}
