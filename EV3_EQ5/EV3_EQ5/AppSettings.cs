using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EV3_EQ5.Models;

using Xamarin.Forms;

namespace EV3_EQ5
{
    public class AppSettings 
    {
        public static string ApiFirebase = "https://piaprogra89-default-rtdb.firebaseio.com/";
        private static string KeyAplication = "AIzaSyChmtCCs4SdTIEA8qB6PDr5sCTb2NcY_Ss";

        public static ResponseAuthentication oAuthentication { get; set; }
        private static string ApiUrlGoogleApis = "https://identitytoolkit.googleapis.com/v1/";

        public static string ApiAuthentication(string tipo)
        {
            if (tipo == "LOGIN")
                return ApiUrlGoogleApis + "accounts:signInWithPassword?key=" + KeyAplication;
            else if (tipo == "SIGNIN")
                return ApiUrlGoogleApis + "accounts:signUp?key=" + KeyAplication;
            else
                return "";
        }
    }
}