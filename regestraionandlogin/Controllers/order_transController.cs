using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using regestraionandlogin.Models;
using System.Data.Entity;
using System.Linq;
using System.Timers;
using System.Net;

namespace regestraionandlogin.Controllers
{
    public class order_transController : Controller
    {
        // GET: order_trans
       
        MyDatabaseEntities3 db = new MyDatabaseEntities3();
        [HttpGet]
        public ActionResult Index(int? id)
        {


            var search = db.Orders.AsQueryable();
            if (!String.IsNullOrEmpty(id.ToString()))
            {
                search = search.Where(o => o.Order_id == id);
            }

            return View(search);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult Add_order()
        {

            ViewBag.Stock_id = new SelectList(db.Stocks, "Stock_id", "StockName", "price");
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add_order([Bind(Include = "Order_id,StockID,TransDate,Quantity ")] Order order, [Bind(Include = "price ,StockName")] Stock stock, [Bind(Include = "wallet")] User usr
           )
        {

            var Total = order.Quantity * stock.price;
            string message = "";
            bool buy = true;
            bool sell = false;

            if (ModelState.IsValid)
            {
                if (Total <= usr.wallet)
                {
                    if (order.Type == buy)
                    {
                        usr.wallet -= Total;
                    }
                    else
                    {
                        usr.wallet += Total;
                    }
                    db.Orders.Add(order);
                    db.SaveChanges();
                    return RedirectToAction("index");
                }
            }
            else
            {
                message = "model is invalid";
            }
            ViewBag.message = message;
            ViewBag.Stock_id = new SelectList(db.Stocks, "Stock_id", "StockName", "price", order.StockID);
            return View(order);

        }




        public ActionResult Update_order(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.StockID = new SelectList(db.Stocks, "Id", "Name", order.StockID);
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update_order([Bind(Include = "Order_id,Type,StockID,Useid,TransDate,Quantity ")] Order order)
        {
            DateTime now = DateTime.Now;
            string message = "";

            if (ModelState.IsValid)
            {
                if (order.TransDate == DateTime.MinValue)
                {

                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    message = "can't update your order";

                }
            }
            ViewBag.message = message;
            ViewBag.Stock_id = new SelectList(db.Stocks, "Stock_id", "StockName", "price", order.StockID);
            return View(order);
        }
    }
}