using ExpenseApplication.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ExpenseApplication.Helpers.Parameters;

namespace ExpenseApplication.Base
{
    public class ExpenseApplicationBasePage : System.Web.UI.Page
    {
        protected AccessEnum AccessType { get; set; }
       
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            var user = DBHelper.GetCurrentUserByID(Context.User.Identity.GetUserId());
            if (user == null)
            {
                Response.Redirect(Parameters.LoginPageURL);
            }
            else
            {
                switch (AccessType)
                {
                    case AccessEnum.CanApprove:
                        {
                            if (!user.Role.CanApprove)
                            {
                                Response.Redirect(Parameters.HomePageURL);

                            }
                            break;
                        }
                    case AccessEnum.CanInsert:
                        {
                            if (!user.Role.CanInsert)
                            {
                                Response.Redirect(Parameters.HomePageURL);

                            }
                            break;
                        }
                    case AccessEnum.CanPay:
                        {
                            if (!user.Role.CanPay)
                            {
                                Response.Redirect(Parameters.HomePageURL);

                            }
                            break;
                        }
                    default: break;
                }
            }
        }
    }
}