using EV3_EQ5.Models;
using EV3_EQ5.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EV3_EQ5.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registro : ContentPage
    {
        

        public Registro()
        {
            InitializeComponent();
        }

        /*private async void ButtonRegistrar_Clicked(object sender, EventArgs e)
        {
            
            string correo = TxtEmail.Text;

            
            if (correo.EndsWith("@gmail.com"))
            {
                
                Usuario oUsuario = new Usuario()
                {
                    Nombres = TxtName.Text,
                    Edad = TxtEdad.Text,
                    Matricula = TxtMatricula.Text,
                    Email = TxtEmail.Text,
                    Clave = TxtPassword.Text
                };

                
                bool respuesta = await ApiServiceAuthentication.RegistrarUsuario(oUsuario);

                
                if (respuesta)
                {
                    await DisplayAlert("Correcto", "Usuario registrado", "Aceptar");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo registrar", "Aceptar");
                }

            }
            else
            {
                
                await DisplayAlert("Error", "El correo electrónico debe ser de dominio @gmail.com", "Aceptar");
            }
           
        }*/

        /*private async void ButtonRegistrar_Clicked(object sender, EventArgs e)
        {
            try
            {
                string correo = TxtEmail.Text;

                if (!correo.EndsWith("@gmail.com"))
                {
                    await DisplayAlert("Error", "El correo electrónico debe ser de dominio @gmail.com", "Aceptar");
                    return;
                }

                // Verificar si el usuario ya está registrado
                bool usuarioExistente = await ApiServiceAuthentication.VerificarUsuarioExistente(TxtName.Text, TxtMatricula.Text);

                if (usuarioExistente)
                {
                    await DisplayAlert("Error", "El usuario ya está registrado", "Aceptar");
                    return;
                }

                // Crear el objeto Usuario
                Usuario oUsuario = new Usuario()
                {
                    Nombres = TxtName.Text,
                    Edad = TxtEdad.Text,
                    Matricula = TxtMatricula.Text,
                    Email = TxtEmail.Text,
                    Clave = TxtPassword.Text
                };

                // Registrar el usuario
                bool respuesta = await ApiServiceAuthentication.RegistrarUsuario(oUsuario);

                if (respuesta)
                {
                    await DisplayAlert("Correcto", "Usuario registrado", "Aceptar");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo registrar", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }*/

        private async void ButtonRegistrar_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Verificar si todos los campos obligatorios están llenos
                if (string.IsNullOrWhiteSpace(TxtName.Text) ||
                    string.IsNullOrWhiteSpace(TxtEdad.Text) ||
                    string.IsNullOrWhiteSpace(TxtMatricula.Text) ||
                    string.IsNullOrWhiteSpace(TxtEmail.Text) ||
                    string.IsNullOrWhiteSpace(TxtPassword.Text))
                {
                    await DisplayAlert("Error", "Por favor, llene todos los campos", "Aceptar");
                    return;
                }

                string correo = TxtEmail.Text;

                if (!correo.EndsWith("@gmail.com"))
                {
                    await DisplayAlert("Error", "El correo electrónico debe ser de dominio @gmail.com", "Aceptar");
                    return;
                }

                // Verificar si el usuario ya está registrado
                bool usuarioExistente = await ApiServiceAuthentication.VerificarUsuarioExistente(TxtName.Text, TxtEmail.Text, TxtMatricula.Text);

                if (usuarioExistente)
                {
                    await DisplayAlert("Error", "El usuario ya está registrado", "Aceptar");
                    return;
                }

                // Crear el objeto Usuario
                Usuario oUsuario = new Usuario()
                {
                    Nombres = TxtName.Text,
                    Edad = TxtEdad.Text,
                    Matricula = TxtMatricula.Text,
                    Email = TxtEmail.Text,
                    Clave = TxtPassword.Text
                };

                // Registrar el usuario
                bool respuesta = await ApiServiceAuthentication.RegistrarUsuario(oUsuario);

                if (respuesta)
                {
                    await DisplayAlert("Correcto", "Usuario registrado", "Aceptar");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo registrar", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }




    }
}