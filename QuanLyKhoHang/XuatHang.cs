using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKhoHang
{
    class XuatHang
    {
        public static List<Item> items = new List<Item>();

        public static void AddItem(int ProductID, string ProductName, decimal UnitPrice, int Quantity)
        {
            Item item = items.Find(i => i.ProductID == ProductID);
            if (item == null)
            {
                item = new Item(ProductID, ProductName, UnitPrice, Quantity);
                items.Add(item);
            }
            else
                item.Quantity = Quantity;
        }

        public static void RemoveItem(int ProductID)
        {
            Item item = items.Find(i => i.ProductID == ProductID);
            if (item != null)
                items.Remove(item);
        }

        public static int TotalQuantity()
        {
            return items.Count != 0 ? items.Sum(i => i.Quantity) : 0;
        }

        public static decimal TotalAmount()
        {
            return items.Count != 0 ? items.Sum(i => i.Amount) : 0;
        }
    }
}
