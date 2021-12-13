using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebApplication2.Models
{
    public class Logger
    {
        private const string PATH = "Data//action_log.txt";
        public static void WriteActionLog(string action, int? id)
        {
            string message = "";
            if(id != null)
            {
                message = id + ":";
            }
            else
            {
                message += "anon:";
            }
            message += action + ":";
            DateTime dateTime = DateTime.Now;
            message += dateTime.ToString() + "\n";

            File.AppendAllText(PATH, message);
        } 
    }
}