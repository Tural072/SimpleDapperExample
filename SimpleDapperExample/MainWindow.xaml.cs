using Dapper;
using SimpleDapperExample.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleDapperExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Product> GetAll()
        {
            List<Product> products = new List<Product>();
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString))
            {
                conn.Open();
                products = conn.Query<Product>("Select * from Products").ToList(); ;
            }
            return products;
        }

        private void InsertProduct(Product product)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString))
            {
                conn.Open();
                conn.Execute("Insert into Products(Name,Price) Values(@ProductName,@ProductPrice)", new { ProductName = product.Name, ProductPrice = product.Price });
            }
        }

        private void UpdateProduct( Product product)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString))
            {
                conn.Open();
                conn.Execute("Update Products set Name=@PName,Price=@PPrice where Id=@PId", new { PId = product.Id, PName = product.Name, PPrice = product.Price });
            }
        }
        private Product GetById(int id)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString))
            {
                conn.Open();
                var product = conn.QueryFirstOrDefault("Select * from Products where Id=@Id", new { Id = id }) ;
                return new Product { 
                Id=product.Id,
                Name=product.Name,
                Price=product.Price
                };
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            //InsertProduct(new Product { Name = "Asus Rog", Price = 4300 });

            myDataGrid.ItemsSource = GetAll();
            var product = GetById(1);
            product.Name ="ACER PRODUCT";
            product.Price = 430000;
            UpdateProduct(product);
            myDataGrid.ItemsSource = GetAll();
        }
    }
}
