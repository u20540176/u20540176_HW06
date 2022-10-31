using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using u20540176_HW06.Models;

namespace u20540176_HW06.Controllers
{
    public class OrdersController : Controller
    {
        private BikeStoresEntities db = new BikeStoresEntities();
        // GET: Orders
        public ActionResult Index(DateTime? search, int? i)
        {
            //var orderItems = db.order_items.Include(oo => oo.product).Include(oo => oo.order).ToList();
            List<order_items> products = db.order_items.ToList();
            // return View(orderItems);
            return View(db.order_items.Where(x => x.order.order_date == search || search == null).ToList().ToPagedList(i ?? 1, 10));



        }

        public string GetEmployeeOrderData()
        {
            List<EmpOrder> obj = db.orders.Select(o => new EmpOrder
            {
                OrderID = o.order_id,
                Amount = (double)o.order_items.Sum(s => s.list_price * s.quantity),
                StaffID = o.staff_id,
            }).ToList();

            List<object> list = new List<object>();
            foreach (staff staff in db.staffs.ToList())
            {
                double amount = obj.Where(o => o.StaffID == staff.staff_id).Sum(o => o.Amount);
                list.Add(new { Amount = Math.Round(amount), Name = staff.first_name + " " + staff.last_name });
            }

            return JsonConvert.SerializeObject(list);
        }
    }
}
    