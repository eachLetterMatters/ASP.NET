﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Foodie
{
    public class Connection
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        }
    }

    public class Utils
    {
        public static bool IsValidExtension(string fileName)
        {
            bool isValid = false;
            string[] fileExtension = { ".jpg", ".png", ".jpeg" };
            for (int i = 0;  i < fileExtension.Length; i++) 
            {
                if (fileName.Contains(fileExtension[i]))
                {
                    isValid = true;
                    break;
                }
            }
            return isValid;
        }

        public static string GetImageUrl(Object url)
        {
            string returnedUrl = "";
            if (string.IsNullOrEmpty(url.ToString()) || url == DBNull.Value)
            {
                returnedUrl = "../Images/no_image.jpg";
            }
            else
            {
                returnedUrl = string.Format("../{0}", url);
            }
            return returnedUrl;
        }
    }
}