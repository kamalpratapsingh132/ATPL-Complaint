using ATPL_Complaint.Customclass;
using ATPL_Complaint.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ATPL_Complaint.Controllers
{
    public class adminController : Controller
    {
        DBModel dbj = new DBModel();
        ObjectParameter objectparam = new ObjectParameter("accid", typeof(decimal));

        // GET: admin
        public ActionResult adminlogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Adminlogin(admin usermodel)
        {
            if (Convert.ToString(Session[ADSSAntiBot.SESSION_CAPTCHA]).Equals(usermodel.cap))
            {
                var userDetails = dbj.tbl_add_admin.Where(model => model.contact_no == usermodel.contactno1 && model.pwd == usermodel.pwd).FirstOrDefault();
                if (userDetails == null)
                {
                    ViewBag.Tempmessage = "INVALID USERNAME OR PASSWORD";
                    return View("adminlogin", usermodel);
                }
                else
                {
                    Session["id"] = userDetails.id;
                    Session["emp_id"] = userDetails.emp_id;
                    Session["UserName"] = Convert.ToString(userDetails.emp_name);
                    return RedirectToAction("Addclient", "admin");
                }
            }
            else
            {
                ViewBag.Message = "INVALID CAPCHA";
                return View();
            }

        }
        [HttpGet]
        public ActionResult Addclient(int? id)
        {
            var Sid = Session["id"];
            if (Sid != null)
            {
                client tbl = new client();


                if (id != null)
                {
                    var res = dbj.tbl_client.ToList();
                    ViewBag.clientlist = res;
                    var data = dbj.tbl_client.Where(x => x.id == id).FirstOrDefault();
                    ViewBag.StateList = dbj.tbl_state.Distinct().OrderBy(i => i.state_id).Select(i => new SelectListItem() { Text = i.state, Value = i.state_id.ToString() }).ToList();
                    tbl.id = data.id;
                    tbl.company_id = data.company_id;
                    tbl.company_name = data.company_name;
                    tbl.contact_person = data.contact_person;
                    tbl.address = data.address;
                    tbl.state = data.state;
                    tbl.city = data.city;
                    tbl.pincode = data.pincode;
                    tbl.contact_no = data.contact_no;
                    tbl.email_id = data.email_id;
                    tbl.gst_no = data.gst_no;
                    tbl.pan_no = data.pan_no;


                    return View(tbl);
                }
                else
                {
                    ViewBag.StateList = dbj.tbl_state.Distinct().OrderBy(i => i.state_id).Select(i => new SelectListItem() { Text = i.state, Value = i.state_id.ToString() }).ToList();
                    var res = dbj.tbl_client.ToList();
                    ViewBag.clientlist = res;
                    return View(tbl);
                }
            }
            else
            {

                return RedirectToAction("adminlogin", "admin");
            }
        }

        [HttpPost]
        public ActionResult addclient(client Model)
        {
            ViewBag.StateList = dbj.tbl_state.Distinct().OrderBy(i => i.state_id).Select(i => new SelectListItem() { Text = i.state, Value = i.state_id.ToString() }).ToList();
            var res = dbj.tbl_client.ToList();
            ViewBag.clientlist = res;
            //if (ModelState.IsValid)
            //{

            if (Model.id == 0)
            {
                Random rand = new Random();
                string password = Convert.ToString(rand.Next(100000, 999999));
                var x = dbj.Sp_addclient(Model.company_id, Model.company_name, Model.contact_person, Model.address, Model.city, Model.state, Model.pincode, Model.contact_no, Model.email_id, password, Model.gst_no, Model.pan_no, objectparam);
                var chk = objectparam.Value;
                if (Convert.ToInt32(chk) == 0)
                {
                    ViewBag.Message = "CLIENT DATA ALREADY EXIST.";
                }
                else
                {
                    ViewBag.Message = "CLIENT DATA SAVE SUCCESSFULLY.";
                }

                ModelState.Clear();
            }
            else
            {

                var x = dbj.Sp_Update_client((Convert.ToInt32(Model.id)), Model.company_id, Model.company_name, Model.contact_person, Model.address, Model.city, Model.state, Model.pincode, Model.contact_no, Model.email_id, Model.pwd, Model.gst_no, Model.pan_no, objectparam);
                var chk = objectparam.Value;
                // var chk = Convert.ToInt32(accid.Value);
                if (Convert.ToInt32(chk) == 1)
                {

                    TempData["msg"] = " UPDATED SUCCESSFULLY !!!";

                }
                else
                {
                    TempData["msg"] = " NOT UPDATED SUCCESSFULLY !!!";
                }

                ModelState.Clear();

            }
            //}


            return View();
        }


        public ActionResult City_Bind(int State)
        {
            List<SelectListItem> City = new List<SelectListItem>();
            City = dbj.tbl_city.Where(i => i.state_id == State).OrderBy(i => i.state_id).Select(i => new SelectListItem() { Text = i.city, Value = i.city_id.ToString() }).Distinct().ToList();
            return Json(City, JsonRequestBehavior.AllowGet);
        }


        public ActionResult clientdelete(int id)
        {
            if (id != 0)
            {
                var dt_Delete = dbj.tbl_client.Where(x => x.id == id).FirstOrDefault();
                var dt = dbj.Sp_Delete_client(id, objectparam);
                var chk = objectparam.Value;
                // var chk = Convert.ToInt32(accid.Value);
                if (Convert.ToInt32(chk) == 1)
                {
                    TempData["mg"] = "DELETED SUCCESSFULLY !";
                }
                else
                {
                    TempData["mg"] = "SOMETHING WENT WRONG !";
                }
                return RedirectToAction("addclient");
            }
            else
            {
                return RedirectToAction("addclient");
            }
        }


        [HttpGet]
        public ActionResult Addproduct(int? id)
        {
            var Sid = Session["id"];
            if (Sid != null)
            {
                product tbl = new product();


                if (id != null)
                {
                    ViewBag.CategoryListItems = dbj.tbl_caty.Distinct().OrderBy(i => i.product_caty).Select(i => new SelectListItem() { Text = i.product_caty, Value = i.caty_id.ToString() }).ToList();
                    var res = dbj.tbl_product.ToList();
                    ViewBag.productlist = res;
                    var data = dbj.tbl_product.Where(x => x.id == id).FirstOrDefault();
                    tbl.id = data.id;
                    tbl.product_category = data.product_category;
                    tbl.product_name = data.product_name;
                    tbl.description = data.description;


                    return View(tbl);
                }
                else
                {
                    ViewBag.CategoryListItems = dbj.tbl_caty.Distinct().OrderBy(i => i.product_caty).Select(i => new SelectListItem() { Text = i.product_caty, Value = i.caty_id.ToString() }).ToList();
                    var res = dbj.tbl_product.ToList();
                    ViewBag.productlist = res;
                    return View(tbl);
                }
            }
            else
            {

                return RedirectToAction("adminlogin", "admin");
            }

        }
        [HttpPost]
        public ActionResult Addproduct(product Model)
        {

            var res = dbj.tbl_product.ToList();
            ViewBag.productlist = res;

            if (Model.id == 0)
            {

                var x = dbj.Sp_addproduct(Model.product_id, Model.product_category, Model.product_name, Model.description, objectparam);
                var chk = objectparam.Value;
                if (Convert.ToInt32(chk) == 0)
                {
                    ViewBag.Mess = "ALREADY ADD PRODUCT.";

                }
                else
                {
                    ViewBag.Mess = "PRODUCT ADD SUCCESSFULLY.";

                }
                ModelState.Clear();
            }
            else
            {

                var x = dbj.Sp_Update_product((Convert.ToInt32(Model.id)), Model.product_id, Model.product_category, Model.product_name, Model.description, objectparam);
                var chk = objectparam.Value;
                // var chk = Convert.ToInt32(accid.Value);
                if (Convert.ToInt32(chk) == 1)
                {

                    TempData["msg"] = " UPDATED SUCCESSFULLY !!!";

                }
                else
                {
                    TempData["msg"] = " NOT UPDATED SUCCESSFULLY !!!";
                }

                ModelState.Clear();

            }
            ViewBag.CategoryListItems = dbj.tbl_caty.Distinct().OrderBy(i => i.product_caty).Select(i => new SelectListItem() { Text = i.product_caty, Value = i.caty_id.ToString() }).ToList();
            return View("Addproduct");
        }

        public ActionResult productdelete(int id)
        {
            if (id != 0)
            {
                var dt_Delete = dbj.tbl_product.Where(x => x.id == id).FirstOrDefault();
                var dt = dbj.Sp_Delete_product(id, objectparam);
                var chk = objectparam.Value;
                // var chk = Convert.ToInt32(accid.Value);
                if (Convert.ToInt32(chk) == 1)
                {
                    TempData["mg"] = "DELETED SUCCESSFULLY !";
                }
                else
                {
                    TempData["mg"] = "SOMETHING WENT WRONG !";
                }

                return RedirectToAction("addproduct");
            }
            else
            {
                return RedirectToAction("addproduct");
            }
        }


        //[HttpGet]
        //public ActionResult Addemp(int? id)
        //{
        //    var Sid = Session["id"];
        //    if (Sid != null)
        //    {
        //        emp tbl = new emp();


        //        if (id != null)
        //        {
        //            ViewBag.CategoryList = dbj.tbl_designation.Distinct().OrderBy(i => i.designation_name).Select(i => new SelectListItem() { Text = i.designation_name, Value = i.designation_id.ToString() }).ToList();
        //            ViewBag.CategoryListItems = dbj.tbl_department.Distinct().OrderBy(i => i.dept_name).Select(i => new SelectListItem() { Text = i.dept_name, Value = i.dept_id.ToString() }).ToList();
        //            ViewBag.StateList = dbj.tbl_state.Distinct().OrderBy(i => i.state_id).Select(i => new SelectListItem() { Text = i.state, Value = i.state_id.ToString() }).ToList();
        //            var res = dbj.tbl_emp.ToList();
        //            ViewBag.emplist = res;
        //            var data = dbj.tbl_emp.Where(x => x.id == id).FirstOrDefault();
        //            tbl.id = data.id;
        //            tbl.emp_id = data.emp_id;
        //            tbl.emp_name = data.emp_name;
        //            tbl.contact_no = data.contact_no;
        //            tbl.email_id = data.email_id;
        //            tbl.pwd = data.pwd;
        //            tbl.address = data.address;
        //            tbl.city = data.city;
        //            tbl.state = data.state;
        //            tbl.pincode = data.desigination;
        //            tbl.desigination = data.desigination;
        //            tbl.department = data.department;


        //            return View(tbl);
        //        }
        //        else
        //        {
        //            ViewBag.CategoryList = dbj.tbl_designation.Distinct().OrderBy(i => i.designation_name).Select(i => new SelectListItem() { Text = i.designation_name, Value = i.designation_id.ToString() }).ToList();
        //            ViewBag.CategoryListItems = dbj.tbl_department.Distinct().OrderBy(i => i.dept_name).Select(i => new SelectListItem() { Text = i.dept_name, Value = i.dept_id.ToString() }).ToList();
        //            ViewBag.StateList = dbj.tbl_state.Distinct().OrderBy(i => i.state_id).Select(i => new SelectListItem() { Text = i.state, Value = i.state_id.ToString() }).ToList();
        //            var res = dbj.tbl_emp.ToList();
        //            ViewBag.emplist = res;
        //            return View(tbl);
        //        }
        //    }
        //    else
        //    {

        //        return RedirectToAction("adminlogin", "admin");
        //    }
        //}
        //[HttpPost]
        //public ActionResult Addemp(emp Model)
        //{

        //    var res = dbj.tbl_emp.ToList();
        //    ViewBag.emplist = res;

        //    if (Model.id == 0)

        //    {

        //        var x = dbj.Sp_addemp(Model.emp_id, Model.emp_name, Model.contact_no, Model.email_id, Model.pwd, Model.address, Model.city, Model.state, Model.pincode, Model.desigination, Model.department, objectparam);
        //        var chk = objectparam.Value;
        //        if (Convert.ToInt32(chk) == 0)
        //        {
        //            ViewBag.Message = "EMPLOYEE DATA ALREADY EXIST.";
        //        }
        //        else
        //        {
        //            ViewBag.Message = "EMPLOYEE DATA SAVE SUCCESSFULLY.";
        //        }
        //    }
        //    else
        //    {

        //        var x = dbj.Sp_Update_emp((Convert.ToInt32(Model.id)), Model.emp_id, Model.emp_name, Model.contact_no, Model.email_id, Model.pwd, Model.address, Model.city, Model.state, Model.pincode, Model.desigination, Model.department, objectparam);
        //        var chk = objectparam.Value;
        //        // var chk = Convert.ToInt32(accid.Value);
        //        if (Convert.ToInt32(chk) == 1)
        //        {

        //            TempData["msg"] = " UPDATED SUCCESSFULLY !!!";

        //        }
        //        else
        //        {
        //            TempData["msg"] = " NOT UPDATED SUCCESSFULLY !!!";
        //        }

        //        ModelState.Clear();

        //    }
        //    ViewBag.CategoryList = dbj.tbl_designation.Distinct().OrderBy(i => i.designation_name).Select(i => new SelectListItem() { Text = i.designation_name, Value = i.designation_id.ToString() }).ToList();
        //    ViewBag.CategoryListItems = dbj.tbl_department.Distinct().OrderBy(i => i.dept_name).Select(i => new SelectListItem() { Text = i.dept_name, Value = i.dept_id.ToString() }).ToList();
        //    ViewBag.StateList = dbj.tbl_state.Distinct().OrderBy(i => i.state_id).Select(i => new SelectListItem() { Text = i.state, Value = i.state_id.ToString() }).ToList();
        //    return View("Addemp");
        //}

        //public ActionResult empdelete(int id)
        //{
        //    if (id != 0)
        //    {
        //        var dt_Delete = dbj.tbl_emp.Where(x => x.id == id).FirstOrDefault();
        //        var dt = dbj.Sp_Delete_emp(id, objectparam);
        //        var chk = objectparam.Value;
        //        // var chk = Convert.ToInt32(accid.Value);
        //        if (Convert.ToInt32(chk) == 1)
        //        {
        //            TempData["mg"] = "DELETED SUCCESSFULLY !";
        //        }
        //        else
        //        {
        //            TempData["mg"] = "SOMETHING WENT WRONG !";
        //        }
        //        return RedirectToAction("Addemp");
        //    }
        //    else
        //    {
        //        return RedirectToAction("Addemp");
        //    }
        //}


        //[HttpGet]
        //public ActionResult Assiproduct(int? id)
        //{
        //    var Sid = Session["id"];
        //    if (Sid != null)
        //    {

        //        assi_product tbl = new assi_product();


        //        if (id != null)
        //        {

        //            ViewBag.CategoryListItems = dbj.tbl_product.Distinct().OrderBy(i => i.product_name).Select(i => new SelectListItem() { Text = i.product_name, Value = i.product_id.ToString() }).ToList();
        //            ViewBag.CategoryList = dbj.tbl_client.Distinct().OrderBy(i => i.company_name).Select(i => new SelectListItem() { Text = i.company_name, Value = i.company_id.ToString() }).ToList();
        //            ViewBag.Category = dbj.tbl_emp.Distinct().OrderBy(i => i.emp_name).Select(i => new SelectListItem() { Text = i.emp_name, Value = i.emp_id.ToString() }).ToList();
        //            var res = dbj.tbl_assi_product.ToList();
        //            ViewBag.assiproductlist = res;
        //            var data = dbj.tbl_assi_product.Where(x => x.id == id).FirstOrDefault();
        //            tbl.id = data.id;
        //            tbl.select_product = data.select_product;
        //            tbl.select_client = data.select_client;
        //            tbl.select_emp = data.select_emp;
        //            tbl.select_project_supp = data.select_project_supp;
        //            tbl.installation_date = data.installation_date;
        //            tbl.amc_date = data.amc_date;
        //            tbl.amount = data.amount;
        //            tbl.amc_per = data.amc_per;


        //            return View(tbl);
        //        }
        //        else
        //        {
        //            ViewBag.CategoryListItems = dbj.tbl_product.Distinct().OrderBy(i => i.product_name).Select(i => new SelectListItem() { Text = i.product_name, Value = i.product_id.ToString() }).ToList();
        //            ViewBag.CategoryList = dbj.tbl_client.Distinct().OrderBy(i => i.company_name).Select(i => new SelectListItem() { Text = i.company_name, Value = i.company_id.ToString() }).ToList();
        //            ViewBag.Category = dbj.tbl_emp.Distinct().OrderBy(i => i.emp_name).Select(i => new SelectListItem() { Text = i.emp_name, Value = i.emp_id.ToString() }).ToList();

        //            var res = dbj.tbl_assi_product.ToList();
        //            ViewBag.assiproductlist = res;
        //            return View(tbl);
        //        }
        //    }
        //    else
        //    {

        //        return RedirectToAction("adminlogin", "admin");
        //    }
        //}

        //[HttpPost]
        //public ActionResult Assiproduct(assi_product Model)
        //{

        //    ViewBag.CategoryListItems = dbj.tbl_product.Distinct().OrderBy(i => i.product_name).Select(i => new SelectListItem() { Text = i.product_name, Value = i.product_id.ToString() }).ToList();
        //    ViewBag.CategoryList = dbj.tbl_client.Distinct().OrderBy(i => i.company_name).Select(i => new SelectListItem() { Text = i.company_name, Value = i.company_id.ToString() }).ToList();
        //    ViewBag.Category = dbj.tbl_emp.Distinct().OrderBy(i => i.emp_name).Select(i => new SelectListItem() { Text = i.emp_name, Value = i.emp_id.ToString() }).ToList();

        //    var res = dbj.tbl_assi_product.ToList();
        //    ViewBag.assiproductlist = res;

        //    if (Model.id == 0)
        //    {
        //        var x = dbj.Sp_assiproduct(Model.select_client_id, Model.select_product, Model.select_client, Model.select_emp, Model.select_project_supp, Model.installation_date, Model.amc_date, Model.amount, Model.amc_per, objectparam);
        //        var chk = objectparam.Value;

        //        if (Convert.ToInt32(chk) == 0)
        //        {
        //            ViewBag.Message = "ALREADY ASSIGNED";
        //        }
        //        else
        //        {
        //            ViewBag.Message = "ASSIGNED PRODUCT";
        //        }

        //        ModelState.Clear();
        //    }
        //    else
        //    {
        //        var x = dbj.Sp_Update_assi_product((Convert.ToInt32(Model.id)), Model.select_client_id, Model.select_product, Model.select_client, Model.select_emp, Model.select_project_supp, Model.installation_date, Model.amc_date, Model.amount, Model.amc_per, objectparam);
        //        var chk = objectparam.Value;
        //        // var chk = Convert.ToInt32(accid.Value);
        //        if (Convert.ToInt32(chk) == 1)
        //        {

        //            TempData["msg"] = " UPDATED SUCCESSFULLY !!!";

        //        }
        //        else
        //        {
        //            TempData["msg"] = " NOT UPDATED SUCCESSFULLY !!!";
        //        }

        //        ModelState.Clear();
        //    }


        //    return View("Assiproduct");
        //}


        //public ActionResult assi_productdelete(int id)
        //{
        //    if (id != 0)
        //    {
        //        var dt_Delete = dbj.tbl_assi_product.Where(x => x.id == id).FirstOrDefault();
        //        var dt = dbj.Sp_Delete_assi_product(id, objectparam);
        //        var ch = objectparam.Value;
        //        // var chk = Convert.ToInt32(accid.Value);
        //        if (Convert.ToInt32(ch) == 1)
        //        {
        //            TempData["mg"] = "DELETED SUCCESSFULLY !";
        //        }
        //        else
        //        {
        //            TempData["mg"] = "SOMETHING WENT WRONG !";
        //        }
        //        return RedirectToAction("Assiproduct");
        //    }
        //    else
        //    {
        //        return RedirectToAction("Assiproduct");
        //    }
        //}

        public ActionResult ticket_status_check()
        {
            var res = dbj.tbl_Support_ticket.ToList();
            ViewBag.ticket_check = res;
            return View();
        }

        [HttpGet]
        public ActionResult ticket_status_update(int id)
        {
            support_ticket tbl = new support_ticket();

            ViewBag.CategoryItems = dbj.tbl_ticket_for.Distinct().OrderBy(i => i.ticket_for).Select(i => new SelectListItem() { Text = i.ticket_for, Value = i.ticket_id.ToString() }).ToList();
            ViewBag.CategoryListItems = dbj.tbl_product.Distinct().OrderBy(i => i.product_name).Select(i => new SelectListItem() { Text = i.product_name, Value = i.product_id.ToString() }).ToList();
            var data = dbj.tbl_Support_ticket.Where(x => x.id == id).FirstOrDefault();
            tbl.id = data.id;
            tbl.date = data.date;
            tbl.ticket_for = data.ticket_for;
            tbl.message = data.message;
            return View(tbl);

        }

        [HttpPost]
        public ActionResult ticket_status_update(support_ticket Model)
        {
            var x = dbj.Sp_Update_ticket_status((Convert.ToInt32(Model.id)), Model.message, objectparam);
            var chk = objectparam.Value;
            // var chk = Convert.ToInt32(accid.Value);
            if (Convert.ToInt32(chk) == 1)
            {

                TempData["msg"] = " UPDATED SUCCESSFULLY !!!";

            }
            else
            {
                TempData["msg"] = " NOT UPDATED SUCCESSFULLY !!!";
            }

            ModelState.Clear();
            return View();
        }

        [HttpGet]
        public ActionResult Complete_Ticket(int id)
        {

            support_ticket tbl = new support_ticket();

            ViewBag.CategoryItems = dbj.tbl_ticket_for.Distinct().OrderBy(i => i.ticket_for).Select(i => new SelectListItem() { Text = i.ticket_for, Value = i.ticket_id.ToString() }).ToList();
            ViewBag.CategoryListItems = dbj.tbl_product.Distinct().OrderBy(i => i.product_name).Select(i => new SelectListItem() { Text = i.product_name, Value = i.product_id.ToString() }).ToList();
            var data = dbj.tbl_Support_ticket.Where(x => x.id == id).FirstOrDefault();
            tbl.id = data.id;
            tbl.date = data.date;
            tbl.ticket_for = data.ticket_for;
            tbl.message = data.message;
            return View(tbl);
        }

        [HttpPost]
        public ActionResult complete_Ticket(support_ticket Model)
        {
            var x = dbj.Sp_Complete_ticket((Convert.ToInt32(Model.id)), Model.message, objectparam);
            var chk = objectparam.Value;
            // var chk = Convert.ToInt32(accid.Value);
            if (Convert.ToInt32(chk) == 1)
            {

                TempData["msg"] = " COMPLETED SUCCESSFULLY !!!";

            }
            else
            {
                TempData["msg"] = " NOT COMPLETE SUCCESSFULLY !!!";
            }

            ModelState.Clear();
            return View();
        }


        public ActionResult ticket_check()
        {
            var Sid = Session["id"];
            if (Sid != null)
            {
                var res = dbj.tbl_Support_ticket.ToList();
                ViewBag.ticket_check = res;
                string userid = Session["emp_id"].ToString();
                if (userid != "0")
                {
                    var s = (from st in dbj.tbl_Support_ticket
                             join ap in dbj.tbl_assi_product on st.select_product equals ap.select_product
                             join aa in dbj.tbl_add_admin on ap.select_emp equals aa.emp_id
                             where aa.emp_id == userid
                             select new support_ticket()
                             {
                                 id = st.id,
                                 date = st.date,
                                 select_product = st.select_product,
                                 ticket_for = st.ticket_for,
                                 message = st.message,
                                 status = st.status,
                             }).FirstOrDefault();
                    return View(s);
                }
                
                else

                {
                    return RedirectToAction("adminlogin", "admin");
                }
            }
            else
            {

                return RedirectToAction("adminlogin", "admin");
            }
        }


        [HttpGet]
        public ActionResult Adforgetpwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult adforgetpwd(resetpasswordModel usermodel)
        {
            var userDetails = dbj.tbl_add_admin.Where(model => model.email_id == usermodel.email).FirstOrDefault();
            if (userDetails == null)
            {
                ViewBag.Tempmessage = "Email Id Not Register";
                return View("forgetpwd", usermodel);
            }
            else
            {
                Session["id"] = userDetails.id;
                Session["email_id"] = userDetails.email_id;
                //Session["product_id"] = userDetails.company_id;
                //Session["Name"] = Convert.ToString(userDetails.company_name);
                return RedirectToAction("adsendotp", "admin");
            }
        }

        [HttpGet]
        public ActionResult adsendotp(resetpasswordModel model)
        {
            
                //string  useremail = dbj.tbl_add_admin.Where(model => model.email_id == user.email).FirstOrDefault().ToString();
                //string emailid = useremail.ToString();
                string useremail = "kamalpratapsingh132@gmail.com";
                string sub = "Otp For Forget Password";
                Random rn = new Random();
                int otp = rn.Next(1000, 9999);
                string body = otp.ToString();
                mail(useremail, sub, body);
                return View("sendotp");
            
        }

        public void mail(string useremail, string Sub, string body)
        {
            try
            {

                MailMessage mail = new MailMessage("jamboosingh123@gmail.com", useremail);
                mail.Subject = Sub;
                string strtext1 = body;
                mail.Body = strtext1;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                NetworkCredential Credentials = new NetworkCredential("jamboosingh123@gmail.com", "mekamalsingh006");
                smtp.Credentials = Credentials;
                smtp.Send(mail);
            }

            catch (Exception ex)
            {
                //               
            }
        }


        [HttpGet]
        public ActionResult Admin_reset_pwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult admin_reset_pwd(resetpasswordModel model)
        {
            var Sid = Session["id"];
            if (Sid != null)
            {

                var oldemail = model.email;
                var newpass = model.newpassword;
                var tbl_pass = dbj.tbl_add_admin.Where(x => x.email_id == oldemail).FirstOrDefault();
                if (newpass != null)
                {

                    var dt = dbj.sp_Change_Pass(model.email, model.newpassword, objectparam);
                    var chk = Convert.ToInt32(objectparam.Value);
                    if (chk == 1)
                    {
                        ViewBag.Message = "Password Reset Successfully!";
                    }
                    else
                    {
                        ViewBag.Message = "Something went wrong!";
                    }
                }
                else
                {
                    ViewBag.Message = "Password does not Exist !";

                }
                ModelState.Clear();
                return View("adminlogin");

            }
            else
            {
                //TempData["Msg"] = "<script>alert('Not Logged In')</script>";
                return RedirectToAction("adminlogin", "admin");
            }
        }
    }
    
}