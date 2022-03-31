using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Infrastructure
{
    using System.Data;
    using Models;

    public static class OrderService
    {
        /// <summary>
        /// Returns a List of Orders for the company id provided.
        /// </summary>
        /// <param name="CompanyId">Id for looking up all company orders.</param>
        /// <returns>List of Orders for the provided company.</returns>
        public static List<Order> GetOrdersForCompany(int CompanyId)
        {            
            string sql =
                "SELECT c.name CompanyName, o.description Description, o.order_id OrderId FROM company c INNER JOIN [order] o on c.company_id=o.company_id Where c.company_id = @cId;" +
                "SELECT op.price OrderPrice, op.order_id OrderId, op.product_id ProductId, op.quantity Qty, p.name ProductName, p.price ProductPrice FROM[Order] o Join orderproduct op on op.order_id = o.order_id INNER JOIN product p on op.product_id = p.product_id Where o.company_id = @cId; ";

            DataSet results = Database.ExecuteReader(sql,CommandType.Text, new System.Data.SqlClient.SqlParameter("@cId",CompanyId));
            DataTable dtOrders = results.Tables[0];
            DataTable dtProducts = results.Tables[1];

            List<Order> orders = new List<Order>();

            foreach (DataRow dr in dtOrders.Rows)
            {
                List<OrderProduct> orderProducts = new List<OrderProduct>();
                DataRow[] products = dtProducts.Select("OrderId = " + dr["OrderId"] + "");
                decimal orderPrice = 0;

                foreach (DataRow product in products)
                {
                    OrderProduct orderProduct = new OrderProduct()
                    {
                        OrderId = (int)product["OrderId"],
                        ProductId = (int)product["ProductId"],
                        Price = (decimal)product["OrderPrice"],
                        Quantity = (int)product["Qty"],
                        Product = new Product()
                        {
                            Name = (string)product["ProductName"],
                            Price = (decimal)product["ProductPrice"]
                        }
                    };

                    orderPrice += orderProduct.Price * orderProduct.Quantity;

                    orderProducts.Add(orderProduct);
                }

                Order newOrder = new Order()
                {
                    CompanyName = (string)dr["CompanyName"],
                    Description = (string)dr["Description"],
                    OrderId = (int)dr["OrderId"],
                    OrderProducts = orderProducts,
                    OrderTotal = orderPrice
                };

                orders.Add(newOrder);
            }

            return orders;
        }
    }
}