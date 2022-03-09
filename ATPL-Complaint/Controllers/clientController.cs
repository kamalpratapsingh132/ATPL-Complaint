using ATPL_Complaint.Customclass;
using ATPL_Complaint.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ATPL_Complaint.Controllers
{
    public class clientController : Controller
    {
        // GET: client
        DBModel dbj = new DBModel();
        ObjectParameter objectparam = new ObjectParameter("accid", typeof(decimal));
        [HttpGet]
        public ActionResult clientlogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Clientlogin(client usermodel)
        {
            if (Convert.ToString(Session[ADSSAntiBot.SESSION_CAPTCHA]).Equals(usermodel.cap))
            {

                var userDetails = dbj.tbl_client.Where(model => model.contact_no == usermodel.contact_no && model.pwd == usermodel.pwd).FirstOrDefault();
                if (userDetails == null)
                {
                    ViewBag.Tempmessage = "INVALID USERNAME OR PASSWOWRD";
                    return View("adminlogin", usermodel);
                }
                else
                {
                    Session["id"] = userDetails.id;
                    Session["product_id"] = userDetails.company_id;
                    Session["Name"] = Convert.ToString(userDetails.company_name);
                    return RedirectToAction("status_ticket", "client");
                }
            }
            else
            {
                ViewBag.Message = "INVALID CAPCHA";
                return View();
            }
           
        }
        public ActionResult profile()
        {
            var Sid = Session["id"];
            if (Sid != null)
            {
                int userid = Convert.ToInt32(Session["id"].ToString());
                if (userid != 0)
                {
                    var user = dbj.tbl_client.Where(x => x.id == userid).FirstOrDefault();
                    return View(user);
                }
                else

                {
                    return RedirectToAction("clientlogin", "client");

                }
            }
            else
            {

                return RedirectToAction("clientlogin", "client");
            }
        }
        [HttpGet]
        public ActionResult support_ticket()
        {
            ViewBag.CategoryItems = dbj.tbl_ticket_for.Distinct().OrderBy(i => i.ticket_for).Select(i => new SelectListItem() { Text = i.ticket_for, Value = i.ticket_id.ToString() }).ToList();
            ViewBag.CategoryListItems = dbj.tbl_product.Distinct().OrderBy(i => i.product_name).Select(i => new SelectListItem() { Text = i.product_name, Value = i.product_id.ToString() }).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult support_ticket(support_ticket Model)
        {
            var Sid = Session["id"];
            if (Sid != null)
            {
                

                var x = dbj.Sp_add_ticket(Model.date, Model.select_product, Model.ticket_for, Model.message, objectparam);
                var chk = objectparam.Value;

                if (Convert.ToInt32(chk) == 0)
                {
                    ViewBag.Message = "ALREADY ASSIGNED SUPPORT TICKET";

                }
                else
                {
                    ViewBag.Message = "ASSIGNED SUPPORT TICKET";
                }
                ModelState.Clear();
                ViewBag.CategoryItems = dbj.tbl_ticket_for.Distinct().OrderBy(i => i.ticket_for).Select(i => new SelectListItem() { Text = i.ticket_for, Value = i.ticket_id.ToString() }).ToList();
                ViewBag.CategoryListItems = dbj.tbl_product.Distinct().OrderBy(i => i.product_name).Select(i => new SelectListItem() { Text = i.product_name, Value = i.product_id.ToString() }).ToList();
                return View();
            }
            else
            {

                return RedirectToAction("clientlogin", "client");
            }
        }

        public ActionResult clientproduct()
        {
            var Sid = Session["id"];
            if (Sid != null)
            {
                string userid = Session["product_id"].ToString();
                if (userid != "0")
                {
                    var user = dbj.tbl_assi_product.Where(x => x.select_client == userid).FirstOrDefault();
                    return View(user);
                }

                else

                {
                    return RedirectToAction("clientlogin", "client");
                }
            }
            else
            {

                return RedirectToAction("clientlogin", "client");
            }

        }

        public ActionResult status_ticket()
        {
            var res = dbj.tbl_Support_ticket.Where(x=>x.status!= "COMPLETED").ToList();
            ViewBag.support_ticket = res;


            var rev = dbj.tbl_Support_ticket.Where(x => x.status == "COMPLETED").ToList();
            ViewBag.complete_ticket = rev;

            //assi_product tbl = new assi_product();
            //var = DateTime.Today.AddMonths(-1);

            var rew = dbj.sp_New_check_AMC().ToList();
            ViewBag.upcomingamclist = rew;
            return View();
        }

        //public ActionResult completed_ticket()
        //{
        //    var res = dbj.tbl_Support_ticket.ToList();
        //    ViewBag.complete_ticket = res;
        //    return View();
        //}

        //public ActionResult upcoming_Amc()
        //{
        //    var res = dbj.tbl_assi_product.ToList();
        //    ViewBag.upcomingamclist = res;
        //    return View();
        //}

        [HttpGet]
        public ActionResult Forgetpwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult forgetpwd(resetpasswordModel usermodel)
        {
            var userDetails = dbj.tbl_client.Where(model => model.email_id == usermodel.email).FirstOrDefault();
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
                return RedirectToAction("sendotp", "client");
            }
        }

        [HttpGet]
        public ActionResult sendotp(resetpasswordModel user)
        {
            //string  useremail = dbj.tbl_client.Where(model => model.email_id == user.email).FirstOrDefault().ToString();
            //string emailid = useremail.ToString();
            string useremail = "kamalpratapsingh132@gmail.com";
            string sub = "Otp For Forget Password";
            Random rn = new Random();
            int otp = rn.Next(1000, 9999);
            string body = otp.ToString();
            mail(useremail, sub, body);
            return View("sendotp");
        }

        //[HttpPost]
        //public ActionResult sendotp()
        //{
         
        //}

        public void mail( string useremail, string Sub, string body)
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
        public ActionResult Resetpassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult resetpassword(resetpasswordModel model)
        {
            var Sid = Session["id"];
            if (Sid != null)
            {

                var oldemail = model.email;
               var newpass = model.newpassword;
               var tbl_pass = dbj.tbl_client.Where(x => x.email_id == oldemail).FirstOrDefault();
               if (newpass != null)
              {

                var dt = dbj.sp_Change_Pass(model.email,model.newpassword, objectparam);
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
                return View("clientlogin");
               
            }
            else
            {
                //TempData["Msg"] = "<script>alert('Not Logged In')</script>";
                return RedirectToAction("clientlogin", "client");
            }   
        }

    }
}