using Firebase.Database;
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
	public partial class Consultarsala : ContentPage
	{
        FirebaseClient firebase;
        public Consultarsala ()
		{
            InitializeComponent();
            firebase = new FirebaseClient("https://piaprogra89-default-rtdb.firebaseio.com/");
            MostrarSalasReservadas();
        }

        private async void atrasConsu_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ApartaSala());
        }

        private async void MostrarSalasReservadas()
        {
            try
            {
                // Recupera las salas reservadas desde Firebase
                var reservaciones = await firebase.Child("reservaciones").OnceAsync<Reservacion>();

                // Convierte las reservaciones en una lista de objetos anónimos para mostrar en el ListView
                var reservacionesList = reservaciones.Select(r => new
                {
                    Sala = $"Sala: {r.Object.Sala}",
                    Fecha = $"Fecha: {DateTime.Parse(r.Object.Fecha).ToShortDateString()}",
                    Hora = $"Hora: {DateTime.Parse(r.Object.Hora).ToShortTimeString()}"
                }).ToList();

                // Establece el origen de datos del ListView
                listView.ItemsSource = reservacionesList;
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }



    }
}