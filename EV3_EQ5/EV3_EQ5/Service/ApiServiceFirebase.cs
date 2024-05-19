using EV3_EQ5.Models;
using EV3_EQ5;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;


using Xamarin.Forms;

namespace EV3_EQ5.Service
{
    public class ApiServiceFirebase
    {

        static FirebaseClient firebase = new FirebaseClient("https://piaprogra89-default-rtdb.firebaseio.com/");

        public static async Task<Usuario> ObtenerUsuario(string userId)
        {
            try
            {

                var usuarioRef = firebase.Child("usuarios").Child(userId);


                var usuarioSnapshot = await usuarioRef.OnceSingleAsync<Usuario>();

                if (usuarioSnapshot != null)
                {

                    return usuarioSnapshot;
                }
                else
                {

                    return null;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al obtener usuario: {ex.Message}");
                return null;
            }
        }

        public static async Task<bool> RegistrarUsuario(Usuario oUsuario, ResponseAuthentication oResponse)
        {
            try
            {

                HttpClient client = new HttpClient();
                var body = JsonConvert.SerializeObject(oUsuario);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var formatoapi = string.Concat(AppSettings.ApiFirebase, "{0}/{1}.json?auth={2}");

                var response = await client.PutAsync(
                    string.Format(formatoapi, "usuarios", oResponse.LocalId, oResponse.IdToken),
                    content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }

        }

        public static async Task<Usuario> ObtenerUsuario()
         {
             Usuario oObject = new Usuario();
             try
             {
                 HttpClient client = new HttpClient();
                 string apiformat = string.Concat(AppSettings.ApiFirebase, "usuarios/{0}.json?auth={1}");
                 var response = await client.GetAsync(string.Format(apiformat, AppSettings.oAuthentication.LocalId, AppSettings.oAuthentication.IdToken));
                 if (response.StatusCode.Equals(HttpStatusCode.OK))
                 {
                     var jsonstring = await response.Content.ReadAsStringAsync();

                     if (jsonstring != "null")
                     {
                         oObject = JsonConvert.DeserializeObject<Usuario>(jsonstring);
                     }
                     return oObject;
                 }
                 else
                 {
                     return oObject;
                 }
             }
             catch (Exception ex)
             {
                 string t = ex.Message;
                 return oObject;
             }

         }




        public static async Task<bool> GuardarCambiosUsuario(Usuario oUsuario)
        {
            Usuario oObject = new Usuario();
            try
            {
                HttpClient client = new HttpClient();
                var body = JsonConvert.SerializeObject(oUsuario);
                var content = new StringContent(body, Encoding.UTF8, "application/json");


                string apiformat = string.Concat(AppSettings.ApiFirebase, "usuarios/{0}.json?auth={1}");
                var response = await client.PutAsync(string.Format(apiformat, AppSettings.oAuthentication.LocalId, AppSettings.oAuthentication.IdToken), content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }

        }

        public static bool RegistrarUsuario(Usuario usuario)
        {
            throw new NotImplementedException();
        }
    }
}