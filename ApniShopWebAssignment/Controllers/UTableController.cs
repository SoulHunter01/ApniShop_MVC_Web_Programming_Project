using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApniShopWebAssignment.Models;

namespace ApniShopWebAssignment.Controllers
{
    public class UTableController : Controller
    {
        public static bool isLoggedIndex=false;

        [HttpGet]
        public ActionResult Registration()
        {
            UTable tablemodel = new UTable();
            return View(tablemodel);
        }
        // GET: Table
        [HttpPost]
        public ActionResult Registration(UTable tableModel)
        {
            string filename = Path.GetFileNameWithoutExtension(tableModel.ImageFile.FileName);
            string extension = Path.GetExtension(tableModel.ImageFile.FileName);
            filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
            tableModel.UProfilePicturePath = "~/Image/" + filename;
            filename = Path.Combine(Server.MapPath("~/Image/"), filename);
            tableModel.ImageFile.SaveAs(filename);
            using (LoginRegisterDatabaseEntities2 NEWDBMODEL = new LoginRegisterDatabaseEntities2())
            {
                if (ModelState.IsValid)
                {
                    if (NEWDBMODEL.UTables.Any(x => x.Uname == tableModel.Uname))
                    {
                        ViewBag.ErrorMessage = "UserName Already Exists";
                        return View("Registration", new UTable());
                    }
                    NEWDBMODEL.UTables.Add(tableModel);
                    NEWDBMODEL.SaveChanges();
                }
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful";
            return View("Registration", new UTable());
        }
        [HttpGet]
        public ActionResult Login()
        {
            UTable tablemodel = new UTable();
            return View(tablemodel);
        }
        [HttpPost]
        public ActionResult Login(UTable tableModel)
        {
            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                if( (DBMODEL.UTables.Any(x=>x.Uname==tableModel.Uname)) && (DBMODEL.UTables.Any(x => x.UPassword == tableModel.UPassword)))
                {
                    ApniShopWebAssignment.Models.UTable check = DBMODEL.UTables.Single(x => x.Uname == tableModel.Uname);
                    if (tableModel.UIsAdmin==true)
                    {
                     
                        if(check.UIsAdmin==tableModel.UIsAdmin)
                        {
                            ModelState.Clear();
                            return RedirectToActionPermanent("AdminIndex");
                        }
                        else
                        {
                            UTable newentry = new UTable();
                            ViewBag.ErrorMessage = "YOU ARE NOT AN ADMIN!!!";
                            return View();

                        }
                    }
                    else
                    {
                        ModelState.Clear();
                        isLoggedIndex = true;
                        ApniShopWebAssignment.Models.UTable entry = DBMODEL.UTables.Single(x => x.Uname == tableModel.Uname);
                        Session["DATA"] = entry;
                        return RedirectToActionPermanent("Index", entry);
                    }

                    
                }
                else
                {
                    ModelState.Clear();
                    ViewBag.ErrorMessage = "Login Credentials Are Incorrect";
                    return View();
                }
        
            }
        
        }

        public ActionResult Index()
        {
            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                var entry = DBMODEL.PTables.Where(x => x.popularity >= 5).ToList();
                return View(entry);
            }
        }

        public ActionResult AdminIndex()
        {
            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                var products = DBMODEL.UPTables.SqlQuery("SELECT TOP(1000) [UPID],[UPNAME],[UPImage],[UPCategory],[UPRating], [UPPrice],[UPWish],[UPQuantity] FROM [dbo].[UPTable] ").ToList<UPTable>();
                return View(products);
            }
        }

        public ActionResult ForgotPassword()
        {
            UTable tablemodel = new UTable();
            return View(tablemodel);
        }
        [HttpPost]
        public ActionResult ForgotPassword(UTable tableModel)
        {
            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                if ((DBMODEL.UTables.Any(x => x.Uname == tableModel.Uname)))
                {
                    ApniShopWebAssignment.Models.UTable entry = DBMODEL.UTables.Single(x => x.Uname == tableModel.Uname);
                    return View("ShowUserName",entry);
             
                }
                else
                {
                    ModelState.Clear();
                    ViewBag.ErrorMessage = "UserName Does Not Exist";
                    return View();
                }

            }

        }
        public ActionResult ProfilePage(UTable tablemodel)
        {
            if (isLoggedIndex)
            {
                var data = Session["DATA"] as UTable;
                return View(data); 
            }
            else
            {
                return View(tablemodel);
            }
        }

        public ActionResult EditProfile()
        {
            var user = Session["DATA"] as UTable;

            return View(user);
        }

        [HttpPost]
        public ActionResult EditProfile(UTable tableModel)
        {
            string filename = Path.GetFileNameWithoutExtension(tableModel.UProfilePicturePath);
            string extension = Path.GetExtension(tableModel.ImageFile.FileName);
            filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
            tableModel.UProfilePicturePath = "~/Image/" + filename;
            filename = Path.Combine(Server.MapPath("~/Image/"), filename);
            tableModel.ImageFile.SaveAs(filename);

            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                var entry = Session["DATA"] as UTable;

                DBMODEL.UTables.Remove(DBMODEL.UTables.Find(entry.UID));
                DBMODEL.SaveChanges();

                DBMODEL.UTables.Add(tableModel);
                DBMODEL.SaveChanges();
            }
            ModelState.Clear();
            return View("ProfilePage", tableModel);
        }

        public ActionResult AddProductToProductDatabase(int id,string id2)
        {
            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                ApniShopWebAssignment.Models.UPTable check = DBMODEL.UPTables.Single(x => x.UPNAME == id2);
                PTable newentry = new PTable();
                newentry.PName = check.UPNAME;
                newentry.PQuantity = check.UPQuantity;
                newentry.PPrice = check.UPPrice;
                newentry.PRating = check.UPRating;
                newentry.PWish = check.UPWish;
                newentry.PImage = check.UPImage;
                newentry.ImageFile = check.ImageFile;
                newentry.PCategory = check.UPCategory;
                DBMODEL.PTables.Add(newentry);
                _ = DBMODEL.SaveChanges();
                var removeid = DBMODEL.UPTables.Find(id);
                DBMODEL.UPTables.Remove(removeid);
                _ = DBMODEL.SaveChanges();
                return View();
            }
        }
        public ActionResult RemoveProductFromTempDatabase(int id)
        {
            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                var removeid = DBMODEL.UPTables.Find(id);
                DBMODEL.UPTables.Remove(removeid);
                _ = DBMODEL.SaveChanges();
                return View();
            }
        }
        public ActionResult userOrders(int id)
        {
            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                  var entry = DBMODEL.OTables.Where(x => x.CustomerID == id).ToList();

                return View(entry);
            }
        }

        public ActionResult GiveRating(int id)
        {
            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                var current = DBMODEL.OTables.Find(id);
                return View(current);
            }
        }

        [HttpPost]
        public ActionResult GiveRating(OTable tableModel)
        {
            using (LoginRegisterDatabaseEntities2 DBMODEL = new LoginRegisterDatabaseEntities2())
            {
                DBMODEL.OTables.Remove(DBMODEL.OTables.Find(tableModel.OrderID));
                DBMODEL.OTables.Add(tableModel);
                var entry = DBMODEL.OTables.Where(x => x.ProductID == tableModel.ProductID).ToList();
                DBMODEL.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Rating Updated";
            return View(tableModel);
        }

    }


}