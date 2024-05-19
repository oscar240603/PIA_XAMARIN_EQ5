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
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void registroButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Registro());
        }

        private async void ingresarButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPassword.Text) || string.IsNullOrWhiteSpace(TxtCorreo.Text))
            {
                await DisplayAlert("Oops", "Ingrese todos los datos", "Aceptar");
                return;
            }

            UserAuthentication oUsuario = new UserAuthentication()
            {
                email = TxtCorreo.Text,
                password = TxtPassword.Text,
                returnSecureToken = true
            };

            bool respuesta = await ApiServiceAuthentication.Login(oUsuario);
            if (respuesta)
            {
                Usuario usuario = await ApiServiceFirebase.ObtenerUsuario();
                VariablesGlobales.NombreUsuario = usuario.Nombres;
                VariablesGlobales.EdadUsuario = usuario.Edad;
                VariablesGlobales.Matricula = usuario.Matricula;
                VariablesGlobales.Correo = usuario.Email;

                Application.Current.MainPage = new NavigationPage(new Inicio());
            }
            else
            {
                await DisplayAlert("Oops", "Usuario no encontrado", "OK");
            }
        }
    }
}