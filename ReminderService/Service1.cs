using ExpenseApplication;
using ExpenseApplication.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ReminderService
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer createOrderTimer;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            createOrderTimer = new System.Timers.Timer();
            createOrderTimer.Elapsed += new System.Timers.ElapsedEventHandler(GetMail);
            createOrderTimer.Interval = 300000;
            createOrderTimer.Enabled = true;
            createOrderTimer.AutoReset = true;
            createOrderTimer.Start();
        }

        public void GetMail(object sender, System.Timers.ElapsedEventArgs args)
        {
            var expenseForm= DBHelper.GetExpensesForReminderMail();
            foreach (var item in expenseForm)
            {
               MailHelper.SendReminderMailForManager(item);
                DBHelper.SetModifiedBySytem(item.ID);
            }

           

        }
    }
}
