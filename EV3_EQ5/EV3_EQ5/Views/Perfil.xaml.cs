using EV3_EQ5.Models;
using EV3_EQ5.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EV3_EQ5.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EV3_EQ5.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Perfil : ContentPage
    {

        ApiServiceFirebase apiService;

        public Perfil()
        {
            InitializeComponent();
            CargarDatosUsuario();
        }

        private async void CargarDatosUsuario()
        {
            try
            {
                txtNombre.Text = VariablesGlobales.NombreUsuario;
                txtEdad.Text = VariablesGlobales.EdadUsuario;
                txtMatricula.Text = VariablesGlobales.Matricula;
                txtEmail.Text = VariablesGlobales.Correo;

            }
            catch (Exception ex)
            {
                // Manejar la excepción
                await DisplayAlert("Error", $"Error al cargar datos del usuario: {ex.Message}", "OK");
            }
        }
    }



}
