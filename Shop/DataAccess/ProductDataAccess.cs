﻿using Newtonsoft.Json;
using ShopUI.DataSource;
using ShopUI.Models;
using System.Collections.Generic;
using System.IO;

namespace ShopUI.DataAccess
{
    public class ProductDataAccess : IDataAccess<Product>
    {
        public JsonDataSource _dataSource { get; set; }
        

        public ProductDataAccess(JsonDataSource data)
        {
            _dataSource = data;
        }

        public List<Product> GetAll()
        {
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(_dataSource.GetProducts());          
            return products;
        }

        public void Serialize(List<Product> products)
        {
            string json = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(@"C:\Users\David!\source\repos\WebShopRazorPagesUppgift\Shop\wwwroot\data\Product.json", json);
        }

        public Product GetById(int id)
        {
            foreach (Product product in GetAll())
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
