<%@ WebHandler Language="C#" Class="captcha" %>

using System;
using System.Web;
using System.Drawing;
using System.Web.SessionState;
using ATPL_Complaint.Customclass;
public class captcha : IHttpHandler, IRequiresSessionState
{

   
    public void ProcessRequest (HttpContext context) 
    {
        context.Response.ContentType = "image/jpeg";
        ADSSAntiBot captcha = new ADSSAntiBot();
        string str = captcha.DrawNumbers(6);
        if (context.Session[ ADSSAntiBot.SESSION_CAPTCHA] == null) context.Session.Add(ADSSAntiBot.SESSION_CAPTCHA, str);
        else
        {
            context.Session[ ADSSAntiBot.SESSION_CAPTCHA] = str;
        }
        Bitmap bmp = captcha.Result;
        bmp.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
    } 
    public bool IsReusable {
        get {
            return true;
        }
    }

}