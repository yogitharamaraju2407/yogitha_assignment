using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace In_class_10_Entity_Framework
{
    class Order
    {
        public int OrderId { get; set; }
        public string CustName { get; set; }
        public List<OrderDetail> ProductList { get; set; }
    }



    class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ListPrice { get; set; }
        public string SupplierName { get; set; }
        public List<OrderDetail> OrderList { get; set; }
    }



    class OrderDetail
    {
        public int id { get; set; }
        public Order SaleOrderDetail { get; set; }
        public Product SaleProductDetail { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }

    class OrderDetailsContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=InClass;Trusted_Connection=True;MultipleActiveResultSets=true";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }




    class Program
    {
        static void Main(string[] args)
        {
            using (OrderDetailsContext context = new OrderDetailsContext())
            {
                context.Database.EnsureCreated();



                Order order1 = new Order { CustName = "yogitha" };
                Order order2 = new Order { CustName = "ramaraju" };
              
                Product product1 = new Product
                {
                    ProductName = "chocolate",
                    ListPrice = 20,
                    SupplierName = "MTR"
                };
                Product product2 = new Product
                {
                    ProductName = "Biscuit",
                    ListPrice = 55,
                    SupplierName = "MTR"
                };
            

                OrderDetail orderDetail1 = new OrderDetail
                {
                    SaleOrderDetail = order1,
                    SaleProductDetail = product1,
                    Quantity = 5,
                    OrderDate = DateTime.Now
                };
                OrderDetail orderDetail2 = new OrderDetail
                {
                    SaleOrderDetail = order2,
                    SaleProductDetail = product1,
                    Quantity = 10,
                    OrderDate = DateTime.Now
                };
     
                context.Orders.Add(order1);
                context.Orders.Add(order2);
              
                context.Products.Add(product1);
                context.Products.Add(product2);
                
                context.OrderDetails.Add(orderDetail1);
                context.OrderDetails.Add(orderDetail2);
                

                context.SaveChanges();

                IQueryable<OrderDetail> soldproductorders = context.OrderDetails
                .Include(c => c.SaleProductDetail).Where(c => c.SaleProductDetail == product1);


                List<OrderDetail> SelectedSoldProductOrders = soldproductorders.ToList();

                OrderDetail maxsoldproductorders = context.OrderDetails
                .Include(c => c.SaleProductDetail).Where(c => c.SaleProductDetail == product1).OrderByDescending(x => x.Quantity).FirstOrDefault();


            }




        }
    }
}
