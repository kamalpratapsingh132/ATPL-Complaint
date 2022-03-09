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
    public class sadminController : Controller
    {
        DBModel dbj = new DBModel();
        ObjectParameter objectparam = new ObjectParameter("accid", typeof(decimal));
        // GET: sadmin

        [HttpGet]
        public ActionResult sadminlogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sadminlogin(sadmin usermodel)
        {
            if (Convert.ToString(Session[ADSSAntiBot.SESSION_CAPTCHA]).Equals(usermodel.cap))
            {
                var userDetails = dbj.tbl_sadmin.Where(model => model.contactno == usermodel.contactno && model.pwd == usermodel.pwd).FirstOrDefault();
                if (userDetails == null)
                {
                    ViewBag.Tempmessage = "invalid Username or Password.";
                    return View("sadminlogin", usermodel);
                }
                else

                    Session["id"] = userDetails.id;
                return RedirectToAction("addadmin", "sadmin");
            }
            else
            {
                ViewBag.Message = "Invalid Capcha";
                return View();
            }
        }
        [HttpGet]
        public ActionResult addadmin(int? id)
        {
            var Sid = Session["id"];
            if (Sid != null)
            {
                admin tbl = new admin();


                if (id != null)
                {
                    ViewBag.CategoryList = dbj.tbl_designation.Distinct().OrderBy(i => i.designation_name).Select(i => new SelectListItem() { Text = i.designation_name, Value = i.designation_id.ToString() }).ToList();
                    ViewBag.CategoryListItems = dbj.tbl_department.Distinct().OrderBy(i => i.dept_name).Select(i => new SelectListItem() { Text = i.dept_name, Value = i.dept_id.ToString() }).ToList();
                    ViewBag.StateList = dbj.tbl_state.Distinct().OrderBy(i => i.state_id).Select(i => new SelectListItem() { Text = i.state, Value = i.state_id.ToString() }).ToList();
                    var res = dbj.tbl_add_admin.ToList();
                    ViewBag.emplist = res;
                    var data = dbj.tbl_add_admin.Where(x => x.id == id).FirstOrDefault();
                    tbl.id = data.id;
                    tbl.emp_id = data.emp_id;
                    tbl.emp_name = data.emp_name;
                    tbl.contact_no = data.contact_no;
                    tbl.email_id = data.email_id;
                    tbl.pwd = data.pwd;
                    tbl.address = data.address;
                    tbl.city = data.city;
                    tbl.state = data.state;
                    tbl.pincode = data.desigination;
                    tbl.desigination = data.desigination;
                    tbl.department = data.department;


                    return View(tbl);
                }
                else
                {
                    ViewBag.CategoryList = dbj.tbl_designation.Distinct().OrderBy(i => i.designation_name).Select(i => new SelectListItem() { Text = i.designation_name, Value = i.designation_id.ToString() }).ToList();
                    ViewBag.CategoryListItems = dbj.tbl_department.Distinct().OrderBy(i => i.dept_name).Select(i => new SelectListItem() { Text = i.dept_name, Value = i.dept_id.ToString() }).ToList();
                    ViewBag.StateList = dbj.tbl_state.Distinct().OrderBy(i => i.state_id).Select(i => new SelectListItem() { Text = i.state, Value = i.state_id.ToString() }).ToList();
                    var res = dbj.tbl_add_admin.ToList();
                    ViewBag.emplist = res;
                    return View(tbl);
                }
            }
            else
            {

                return RedirectToAction("sadminlogin", "sadmin");
            }
        }

        [HttpPost]
        public ActionResult addadmin(admin Model)
        {
            var res = dbj.tbl_add_admin.ToList();
            ViewBag.emplist = res;

            if (Model.id == 0)

            {

                var x = dbj.Sp_add_admin(Model.emp_id, Model.emp_name, Model.contact_no, Model.email_id, Model.pwd, Model.address, Model.city, Model.state, Model.pincode, Model.desigination, Model.department, objectparam);
                var chk = objectparam.Value;
                if (Convert.ToInt32(chk) == 0)
                {
                    ViewBag.Message = "EMPLOYEE DATA ALREADY EXIST.";
                }
                else
                {
                    ViewBag.Message = "EMPLOYEE DATA SAVE SUCCESSFULLY.";
                }
            }
            else
            {

                var x = dbj.Sp_Update_admin((Convert.ToInt32(Model.id)), Model.emp_id, Model.emp_name, Model.contact_no, Model.email_id, Model.pwd, Model.address, Model.city, Model.state, Model.pincode, Model.desigination, Model.department, objectparam);
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
            ViewBag.CategoryList = dbj.tbl_designation.Distinct().OrderBy(i => i.designation_name).Select(i => new SelectListItem() { Text = i.designation_name, Value = i.designation_id.ToString() }).ToList();
            ViewBag.CategoryListItems = dbj.tbl_department.Distinct().OrderBy(i => i.dept_name).Select(i => new SelectListItem() { Text = i.dept_name, Value = i.dept_id.ToString() }).ToList();
            ViewBag.StateList = dbj.tbl_state.Distinct().OrderBy(i => i.state_id).Select(i => new SelectListItem() { Text = i.state, Value = i.state_id.ToString() }).ToList();
            return View("addadmin");
        }

        public ActionResult Delete_admin(int id)
        {
            if (id != 0)
            {
                var dt_Delete = dbj.tbl_add_admin.Where(x => x.id == id).FirstOrDefault();
                var dt = dbj.Sp_Delete_admin(id, objectparam);
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
                return RedirectToAction("addadmin");
            }
            else
            {
                return RedirectToAction("addadmin");
            }
        }

        public ActionResult City_Bind(int State)
        {
            List<SelectListItem> City = new List<SelectListItem>();
            City = dbj.tbl_city.Where(i => i.state_id == State).OrderBy(i => i.state_id).Select(i => new SelectListItem() { Text = i.city, Value = i.city_id.ToString() }).Distinct().ToList();
            return Json(City, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Assiproduct(int? id)
        {
            var Sid = Session["id"];
            if (Sid != null)
            {

                assi_product tbl = new assi_product();


                if (id != null)
                {

                    ViewBag.CategoryListItems = dbj.tbl_product.Distinct().OrderBy(i => i.product_name).Select(i => new SelectListItem() { Text = i.product_name, Value = i.product_id.ToString() }).ToList();
                    ViewBag.CategoryList = dbj.tbl_client.Distinct().OrderBy(i => i.company_name).Select(i => new SelectListItem() { Text = i.company_name, Value = i.company_id.ToString() }).ToList();
                    ViewBag.Category = dbj.tbl_add_admin.Distinct().OrderBy(i => i.emp_name).Select(i => new SelectListItem() { Text = i.emp_name, Value = i.emp_id.ToString() }).ToList();
                    var res = dbj.tbl_assi_product.ToList();
                    ViewBag.assiproductlist = res;
                    var data = dbj.tbl_assi_product.Where(x => x.id == id).FirstOrDefault();
                    tbl.id = data.id;
                    tbl.select_product = data.select_product;
                    tbl.select_client = data.select_client;
                    tbl.select_emp = data.select_emp;
                    tbl.select_project_supp = data.select_project_supp;
                    tbl.installation_date = data.installation_date;
                    tbl.amc_date = Convert.ToDateTime(data.amc_date).AddDays(-30).ToString();
                    tbl.amount = data.amount;
                    tbl.amc_per = data.amc_per;


                    return View(tbl);
                }
                else
                {
                    ViewBag.CategoryListItems = dbj.tbl_product.Distinct().OrderBy(i => i.product_name).Select(i => new SelectListItem() { Text = i.product_name, Value = i.product_id.ToString() }).ToList();
                    ViewBag.CategoryList = dbj.tbl_client.Distinct().OrderBy(i => i.company_name).Select(i => new SelectListItem() { Text = i.company_name, Value = i.company_id.ToString() }).ToList();
                    ViewBag.Category = dbj.tbl_add_admin.Distinct().OrderBy(i => i.emp_name).Select(i => new SelectListItem() { Text = i.emp_name, Value = i.emp_id.ToString() }).ToList();

                    var res = dbj.tbl_assi_product.ToList();
                    ViewBag.assiproductlist = res;
                    return View(tbl);
                }
            }
            else
            {

                return RedirectToAction("sadminlogin", "sadmin");
            }
        }

        [HttpPost]
        public ActionResult Assiproduct(assi_product Model)
        {

            ViewBag.CategoryListItems = dbj.tbl_product.Distinct().OrderBy(i => i.product_name).Select(i => new SelectListItem() { Text = i.product_name, Value = i.product_id.ToString() }).ToList();
            ViewBag.CategoryList = dbj.tbl_client.Distinct().OrderBy(i => i.company_name).Select(i => new SelectListItem() { Text = i.company_name, Value = i.company_id.ToString() }).ToList();
            ViewBag.Category = dbj.tbl_add_admin.Distinct().OrderBy(i => i.emp_name).Select(i => new SelectListItem() { Text = i.emp_name, Value = i.emp_id.ToString() }).ToList();

            var res = dbj.tbl_assi_product.ToList();
            ViewBag.assiproductlist = res;

            if (Model.id == 0)
            {
                var x = dbj.Sp_assiproduct(Model.select_client_id, Model.select_product, Model.select_client, Model.select_emp, Model.select_project_supp, Model.installation_date, Model.amc_date, Model.amount, Model.amc_per, objectparam);
                var chk = objectparam.Value;

                if (Convert.ToInt32(chk) == 0)
                {
                    ViewBag.Message = "ALREADY ASSIGNED";
                }
                else
                {
                    ViewBag.Message = "ASSIGNED PRODUCT";
                }

                ModelState.Clear();
            }
            else
            {
                var x = dbj.Sp_Update_assi_product((Convert.ToInt32(Model.id)), Model.select_client_id, Model.select_product, Model.select_client, Model.select_emp, Model.select_project_supp, Model.installation_date, Model.amc_date, Model.amount, Model.amc_per, objectparam);
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


            return View("Assiproduct");
        }


        public ActionResult assi_productdelete(int id)
        {
            if (id != 0)
            {
                var dt_Delete = dbj.tbl_assi_product.Where(x => x.id == id).FirstOrDefault();
                var dt = dbj.Sp_Delete_assi_product(id, objectparam);
                var ch = objectparam.Value;
                // var chk = Convert.ToInt32(accid.Value);
                if (Convert.ToInt32(ch) == 1)
                {
                    TempData["mg"] = "DELETED SUCCESSFULLY !";
                }
                else
                {
                    TempData["mg"] = "SOMETHING WENT WRONG !";
                }
                return RedirectToAction("Assiproduct");
            }
            else
            {
                return RedirectToAction("Assiproduct");
            }
        }
        public ActionResult clientlist()
        {
            var Sid = Session["id"];
            if (Sid != null)
            {
                var res = dbj.tbl_client.ToList();
                ViewBag.clientlist = res;
                return View();
            }
            else
            {

                return RedirectToAction("sadminlogin", "sadmin");
            }
        }


        public ActionResult productlist()
        {
            var Sid = Session["id"];
            if (Sid != null)
            {
                var res = dbj.tbl_product.ToList();
                ViewBag.productlist = res;
                return View();
            }
            else
            {

                return RedirectToAction("sadminlogin", "sadmin");
            }
        }

        public ActionResult employeelist()
        {
            var Sid = Session["id"];
            if (Sid != null)
            {
                var res = dbj.tbl_add_admin.ToList();
                ViewBag.emplist = res;
                return View();
            }
            else
            {

                return RedirectToAction("sadminlogin", "sadmin");
            }
        }

        public ActionResult Assi_productlist()
        {
            var Sid = Session["id"];
            if (Sid != null)
            {

                var res = dbj.tbl_assi_product.ToList();
                ViewBag.assiproductlist = res;
                return View();
            }
            else
            {

                return RedirectToAction("sadminlogin", "sadmin");
            }
        }

        [HttpGet]
        public ActionResult Sad_forgetpwd()
        {
            return View();
        }


        [HttpPost]
        public ActionResult sad_forgetpwd(resetpasswordModel usermodel)
        {

            var userDetails = dbj.tbl_sadmin.Where(model => model.email == usermodel.email).FirstOrDefault();
            if (userDetails == null)
            {
                ViewBag.Tempmessage = "Email Id Not Register";
                return View("forgetpwd", usermodel);
            }
            else
            {
                Session["id"] = userDetails.id;
                Session["email_id"] = userDetails.email;
                //Session["product_id"] = userDetails.company_id;
                //Session["Name"] = Convert.ToString(userDetails.company_name);
                return RedirectToAction("sadsendotp", "sadmin");
            }
        }



        public ActionResult sadsendotp()
        { 
            
            
            //string  useremail = dbj.tbl_sadmin.Where(model => model.email_id == user.email).FirstOrDefault().ToString();
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
        public ActionResult Sadmin_reset_pwd()
        {

           return View();
        }


        [HttpPost]
        public ActionResult sadmin_reset_pwd(resetpasswordModel model)
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
                return RedirectToAction("sadminlogin", "sadmin");
            }
        }
    }
}