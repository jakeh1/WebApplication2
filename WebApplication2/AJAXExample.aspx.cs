using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2.Views
{
    public partial class AJAXExample : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if (Button1.Text == "stop")
            {
                Random randomNumGen = new Random();
                int randomNum = randomNumGen.Next(1, 100);
                this.Label2.Text = randomNum.ToString();
            }
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
            if (Button1.Text == "start")
            {
                Button1.Text = "stop";
            }
            else
            {
                Button1.Text = "start";
            }
        }
    }
}