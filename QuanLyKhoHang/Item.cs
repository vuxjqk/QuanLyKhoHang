using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKhoHang
{
    class Item
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Amount
        {
            get { return UnitPrice * Quantity; }
        }

        public Item(int ProductID, string ProductName, decimal UnitPrice, int Quantity)
        {
            this.ProductID = ProductID;
            this.ProductName = ProductName;
            this.UnitPrice = UnitPrice;
            this.Quantity = Quantity;
        }
    }
}
