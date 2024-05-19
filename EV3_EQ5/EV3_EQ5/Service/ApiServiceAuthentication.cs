using EV3_EQ5.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Firebase.Database;

namespace EV3_EQ5.Service
{
    public class ApiServiceAuthentication
    {
        public static async Task<bool> Login(UserAuthentication oUsuario)
        {
            try
            {

                HttpClient client = new HttpClient();
                var body = JsonConvert.SerializeObject(oUsuario);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(AppSettings.ApiAuthentication("LOGIN"), content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    ResponseAuthentication oResponse = JsonConvert.DeserializeObject<ResponseAuthentication>(jsonResult);
                    AppSettings.oAuthentication = oResponse;
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

        public static async Task<bool> RegistrarUsuario(Usuario oUsuario)
        {
            bool respuesta = true;
            try
            {
                UserAuthentication oUser = new UserAuthentication()
                {

                    email = oUsuario.Email,
                    edad = oUsuario.Edad,
                    password = oUsuario.Clave,
                    returnSecureToken = true
                };

                HttpClient client = new HttpClient();
                var body = JsonConvert.SerializeObject(oUser);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(AppSettings.ApiAuthentication("SIGNIN"), content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    ResponseAuthentication oResponse = JsonConvert.DeserializeObject<ResponseAuthentication>(jsonResult);
                    if (oResponse != null)
                    {
                        respuesta = await ApiServiceFirebase.RegistrarUsuario(oUsuario, oResponse);
                    }
                    else
                    {
                        respuesta = false;
                    }
                }
                else
                {
                    respuesta = false;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                respuesta = false;
            }

            return respuesta;
        }

        /* public static async Task<bool> RegistrarUsuario(Usuario oUsuario)
         {
             bool respuesta = true;
             try
             {
                 UserAuthentication oUser = new UserAuthentication()
                 {
                     nombres = oUsuario.Nombres,
                     email = oUsuario.Email,
                     password = oUsuario.Clave,
                     returnSecureToken = true
                 };

                 HttpClient client = new HttpClient();
                 var body = JsonConvert.SerializeObject(oUser);
                 var content = new StringContent(body, Encoding.UTF8, "application/json");
                 var response = await client.PostAsync(AppSettings.ApiAuthentication("SIGNIN"), content);
                 if (response.StatusCode.Equals(HttpStatusCode.OK))
                 {
                     var jsonResult = await response.Content.ReadAsStringAsync();
                     ResponseAuthentication oResponse = JsonConvert.DeserializeObject<ResponseAuthentication>(jsonResult);
                     if (oResponse != null)
                     {
                         // Guardar el usuario en Firebase Authentication
                         respuesta = await ApiServiceFirebase.RegistrarUsuario(oUsuario, oResponse);

                         if (respuesta)
                         {
                             // Guardar el usuario en Firebase Realtime Database
                             var firebaseClient = new FirebaseClient("https://piaprogra89-default-rtdb.firebaseio.com/");
                             var jsonUsuario = JsonConvert.SerializeObject(new Usuario
                             {
                                 Nombres = oUsuario.Nombres,
                                 Email = oUsuario.Email,
                                 Clave = oUsuario.Clave
                             });
                             await firebaseClient
                                 .Child("usuarios")
                                 .PostAsync(jsonUsuario);
                         }
                     }
                     else
                     {
                         respuesta = false;
                     }
                 }
                 else
                 {
                     respuesta = false;
                 }
             }
             catch (Exception ex)
             {
                 string t = ex.Message;
                 respuesta = false;
             }

             return respuesta;
         } */

        public static Usuario UsuarioAutenticado { get; set; }

        public static async Task<bool> VerificarUsuarioExistente(string nombres, string matricula, string email)
        {
            try
            {
                var firebaseClient = new FirebaseClient("https://piaprogra89-default-rtdb.firebaseio.com/");

                // Realizar la consulta a Firebase Realtime Database
                var usuarios = await firebaseClient
                    .Child("usuarios")
                    .OnceAsync<Usuario>();

                // Verificar si existe algún usuario con el mismo nombre completo y matrícula
                bool usuarioExistente = usuarios.Any(u =>
                    u.Object.Nombres == nombres &&
                    u.Object.Email == matricula &&
                    u.Object.Matricula == matricula);

                return usuarioExistente;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al verificar usuario existente: " + ex.Message);
                return false;
            }
        }



    }
}