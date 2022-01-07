using ApniShopWebAssignment.Controllers;
using ApniShopWebAssignment.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;

namespace ApniShopWebAssignment.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Index()
        {
            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                var products = DBMODEL.PTables.SqlQuery("SELECT TOP(1000) [PID],[PName],[PImage],[PCategory],[PRating], [PPrice],[PWish],[PQuantity],[popularity] FROM [dbo].[PTable] ").ToList<PTable>();
                return View(products);
            }
        }




        // GET: ProductTable
        public ActionResult ViewProduct()
        {
            return View();
        }
        public ActionResult AddProduct()
        {
            PTable tableModel = new PTable();
            return View(tableModel);
        }

        [HttpPost]
        public ActionResult AddProduct(PTable tableModel)
        {
            string filename = Path.GetFileNameWithoutExtension(tableModel.ImageFile.FileName);
            string extension = Path.GetExtension(tableModel.ImageFile.FileName);
            filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
            tableModel.PImage = "~/Image/" + filename;
            filename = Path.Combine(Server.MapPath("~/Image/"), filename);
            tableModel.ImageFile.SaveAs(filename);
            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                if (DBMODEL.PTables.Any(x => x.PName == tableModel.PName))
                {
                    ViewBag.ErrorMessage = "Product Already Exists";
                    return View("AddProduct", new PTable());
                }

                DBMODEL.PTables.Add(tableModel);
                DBMODEL.SaveChanges();
            }

            ModelState.Clear();
            ViewBag.SuccessMessage = "Product Addition Successful";
            return View("AddProduct", new PTable());


        }
        public ActionResult AddProductToTempDatabase()
        {
            UPTable entry = new UPTable();
            return View(entry);
        }

        [HttpPost]
        public ActionResult AddProductToTempDatabase(UPTable tableModel)
        {
            string filename = Path.GetFileNameWithoutExtension(tableModel.ImageFile.FileName);
            string extension = Path.GetExtension(tableModel.ImageFile.FileName);
            filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
            tableModel.UPImage = "~/Image/" + filename;
            filename = Path.Combine(Server.MapPath("~/Image/"), filename);
            tableModel.ImageFile.SaveAs(filename);

            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                if (DBMODEL.UPTables.Any(x => x.UPNAME == tableModel.UPNAME))
                {
                    ViewBag.ErrorMessage = "Product Already Exists In Database";
                    return View("AddProductToTempDatabase", new UPTable());
                }

                DBMODEL.UPTables.Add(tableModel);
                DBMODEL.SaveChanges();
            }

            ModelState.Clear();
            ViewBag.SuccessMessage = "Product Addition Successful";
            return View("AddProductToTempDatabase", new UPTable());
        }








        public ActionResult OrderNow(int id)
        {
            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                Session["PQTY"] = id;
                UTable entry = Session["DATA"] as UTable;
                OTable neworder = new OTable();
                neworder.CustomerID = entry.UID;
                neworder.ShippingServiceName = "TCS";
                neworder.OrderStatus = "Generated";
                neworder.OrderCountry = "Pakistan";
                neworder.OrderDate = DateTime.Now.Date;
                neworder.ProductID = id;
                return View(neworder);
            }
        }
        public ActionResult SaveOrderDetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveOrderDetails(OTable oTable)
        {

            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                var id = Session["PQTY"];
                int id2 = (int)id;
                if (DBMODEL.PTables.Any(newentry => newentry.PID == id2))
                {
                    ApniShopWebAssignment.Models.PTable entry = DBMODEL.PTables.SingleOrDefault(x => x.PID == id2);
                    var removeid = DBMODEL.PTables.Find(entry.PID);
                    DBMODEL.PTables.Remove(removeid);
                    DBMODEL.SaveChanges();
                    entry.PQuantity = entry.PQuantity - 1;
                    entry.popularity = entry.popularity + 1;
                    DBMODEL.PTables.Add(entry);
                    DBMODEL.SaveChanges();
                }
                if (DBMODEL.OTables.Any(x => x.OrderID == oTable.OrderID))
                {
                    ViewBag.ErrorMessage = "Order Already Exists";
                    return View("SaveOrderDetails", new OTable());
                }
                DBMODEL.OTables.Add(oTable);
                DBMODEL.SaveChanges();

            }

            ModelState.Clear();
            ViewBag.SuccessMessage = "Order Has Been Generated ";
            return View("SaveOrderDetails", oTable);
        }

        public ActionResult ApproveProduct(UPTable tablemodel)
        {
            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                return View();
            }
        }

        public ActionResult WishList(int id)
        {
            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                if (DBMODEL.PTables.Any(newentry => newentry.PID == id))
                {
                    ApniShopWebAssignment.Models.PTable entry = DBMODEL.PTables.SingleOrDefault(x => x.PID == id);
                    var removeid = DBMODEL.PTables.Find(entry.PID);
                    DBMODEL.PTables.Remove(removeid);
                    DBMODEL.SaveChanges();
                    int val= (int)entry.PWish.GetValueOrDefault() + 1;
                    entry.PWish = val;
                    DBMODEL.PTables.Add(entry);
                    DBMODEL.SaveChanges();
                }
            }
            return View();

        }

        public ActionResult WishListRemove(int id)
        {
            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                if (DBMODEL.PTables.Any(newentry => newentry.PID == id))
                {
                    ApniShopWebAssignment.Models.PTable entry = DBMODEL.PTables.SingleOrDefault(x => x.PID == id);
                    var removeid = DBMODEL.PTables.Find(entry.PID);
                    DBMODEL.PTables.Remove(removeid);
                    DBMODEL.SaveChanges();
                    int val = (int)entry.PWish.GetValueOrDefault() - 1;
                    entry.PWish = val;
                    DBMODEL.PTables.Add(entry);
                    DBMODEL.SaveChanges();
                }
            }
            return View();
        }



    }
}