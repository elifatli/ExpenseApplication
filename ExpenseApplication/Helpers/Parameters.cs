using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseApplication.Helpers
{
    public class Parameters
    {
        //int bir değer vermezsen direkt sıfırdan başlatır!
        public enum ExpenseStatusEnum
        {
            PendingManagerApproval=1,
            Rejected=2,
            Repair=3,
            PendingAccountantPayment=4,
            Paid=5
        }

        public enum AccessEnum
        {
            CanInsert,
            CanApprove,
            CanPay
        }

        public enum RoleEnum
        {
            Staff=2,
            Manager=3,
            Accountant=4
        }

        public readonly static string LoginPageURL = "~/Account/Login.aspx";
        public readonly static string HomePageURL = "~/Default.aspx";

        public readonly static int GridviewSumTextIndex = 3;
        public readonly static int GridviewSumValueIndex = 4;
    }
}