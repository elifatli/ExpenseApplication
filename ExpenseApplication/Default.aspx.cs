using ExpenseApplication.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ExpenseApplication
{
    public partial class _Default : Page
    {
        public AspNetUser CurrentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentUser = DBHelper.GetCurrentUserByID(Context.User.Identity.GetUserId());
        }
    }
}