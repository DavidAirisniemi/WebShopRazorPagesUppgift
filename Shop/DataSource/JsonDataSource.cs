using System.IO;

namespace ShopUI.DataSource
{
    public class JsonDataSource
    {
        public string GetProducts() {
            string path = @"C:\Users\David!\source\repos\WebShopRazorPagesUppgift\Shop\wwwroot\data\Product.json";

            string jsonResponse = File.ReadAllText(path);
            return jsonResponse;
        }
        public string GetCustomers()
        {
            string path = @"C:\Users\David!\source\repos\WebShopRazorPagesUppgift\Shop\wwwroot\data\Customer.json";

            string jsonResponse = File.ReadAllText(path);
            return jsonResponse;
        }

    }
}
