using EV3_EQ5.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace EV3_EQ5.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModificarSala : ContentPage
    {
        List<numitem> numero;
        FirebaseClient firebase;
        

        public ModificarSala()
        {
            InitializeComponent();
            InitApp();
            firebase = new FirebaseClient("https://piaprogra89-default-rtdb.firebaseio.com/");
            InitializeDateTimePickers();


        }




        private void InitApp()
        {
            numero = new List<numitem>();
            numero.Add(new numitem { num = 1, Name = "1" });
            numero.Add(new numitem { num = 2, Name = "2" });
            numero.Add(new numitem { num = 3, Name = "3" });
            numero.Add(new numitem { num = 4, Name = "4" });
            numero.Add(new numitem { num = 5, Name = "5" });
            numero.Add(new numitem { num = 6, Name = "6" });

            foreach (var Num in numero)
            {
                pickermodifisala.Items.Add(Num.Name);
            }


        }

        private async void atrasModificar_Clicked(object sender, EventArgs e)
        {
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync(new ApartaSala());
        }

        /*private async void savemodifi_Clicked(object sender, EventArgs e){
        try
        {
            // Obtener el ID introducido en el Entry
            string idReservacion = TxtSala.Text;

            // Obtener los nuevos valores seleccionados en el Picker, DatePicker y TimePicker
            string nuevaSala = pickermodifisala.SelectedItem.ToString();
            DateTime nuevaFecha = datemodifiPicker.Date;
            TimeSpan nuevaHora = timemodifiPicker.Time;

            // Actualizar los datos de la reservación en Firebase
            await firebase
                .Child("reservaciones")
                .Child(idReservacion)
                .PutAsync(new Reservacion
                {
                    Id = idReservacion,
                    Sala = nuevaSala,
                    Fecha = nuevaFecha.ToString("yyyy-MM-dd"),
                    Hora = nuevaHora.ToString(@"hh\:mm")
                });

            // Mostrar un mensaje de éxito
            await DisplayAlert("Éxito", "Los cambios en la reservación han sido guardados correctamente", "Aceptar");
        }
        catch (Exception ex)
        {
            // Manejo de errores aquí...
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }*/

        private void InitializeDateTimePickers()
        {
            datemodifiPicker.MinimumDate = DateTime.Today.AddDays(-1);

            timemodifiPicker.Time = new TimeSpan(7, 0, 0); // Hora mínima: 7am
            timemodifiPicker.Format = "hh:mm tt"; // Formato de 12 horas
            timemodifiPicker.PropertyChanged += (sender, e) =>
            {
                if (timemodifiPicker.Time < new TimeSpan(7, 0, 0)) // Si la hora es antes de las 7am
                {
                    timemodifiPicker.Time = new TimeSpan(7, 0, 0); // Establecer la hora a las 7am
                }
                else if (timemodifiPicker.Time > new TimeSpan(20, 0, 0)) // Si la hora es después de las 8pm
                {
                    timemodifiPicker.Time = new TimeSpan(20, 0, 0); // Establecer la hora a las 8pm
                }
            };
        }


        /* private async void savemodifi_Clicked(object sender, EventArgs e)
         {
             try
             {
                 // Obtener el ID introducido en el Entry
                 string idReservacion = TxtSala.Text;

                 // Verificar si el ID de la reservación existe
                 var reservacionExistente = await firebase
                     .Child("reservaciones")
                     .Child(idReservacion)
                     .OnceSingleAsync<Reservacion>();

                 if (reservacionExistente == null)
                 {
                     await DisplayAlert("Error", "El ID de la reservación no existe", "Aceptar");
                     return;
                 }

                 // Obtener los nuevos valores seleccionados en el Picker, DatePicker y TimePicker
                 string nuevaSala = pickermodifisala.SelectedItem.ToString();
                 DateTime nuevaFecha = datemodifiPicker.Date;
                 TimeSpan nuevaHora = timemodifiPicker.Time;

                 if (nuevaHora < new TimeSpan(7, 0, 0) || nuevaHora > new TimeSpan(20, 0, 0))
                 {
                     await DisplayAlert("Error", "La hora seleccionada debe estar entre las 7am y las 8pm", "Aceptar");
                     return;
                 }

                 // Verificar si ya existe una reservación para la sala, fecha y hora seleccionadas
                 var reservaciones = await firebase
                     .Child("reservaciones")
                     .OrderBy("Fecha")
                     .EqualTo(nuevaFecha.ToString("yyyy-MM-dd"))
                     .OnceAsync<Reservacion>();

                 bool salaYaReservada = reservaciones.Any(r =>
                     r.Key != idReservacion && // Excluye la reserva que se está modificando
                     r.Object.Sala == nuevaSala &&
                     r.Object.Hora == nuevaHora.ToString(@"hh\:mm"));

                 if (salaYaReservada)
                 {
                     await DisplayAlert("Error", "La sala seleccionada ya está reservada para esta fecha y hora", "Aceptar");
                     return;
                 }

                 // Actualizar los datos de la reservación en Firebase
                 await firebase
                     .Child("reservaciones")
                     .Child(idReservacion)
                     .PutAsync(new Reservacion
                     {
                         Id = idReservacion,
                         Sala = nuevaSala,
                         Fecha = nuevaFecha.ToString("yyyy-MM-dd"),
                         Hora = nuevaHora.ToString(@"hh\:mm")
                     });

                 // Mostrar un mensaje de éxito
                 await DisplayAlert("Éxito", "Los cambios en la reservación han sido guardados correctamente", "Aceptar");
             }
             catch (Exception ex)
             {
                 // Manejo de errores aquí...
                 await DisplayAlert("Error", ex.Message, "Aceptar");
             }
         }*/

        private async void savemodifi_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Obtener el ID introducido en el Entry
                string idReservacion = TxtSala.Text;

                // Verificar si el ID de la reservación existe
                var reservacionExistente = await firebase
                    .Child("reservaciones")
                    .Child(idReservacion)
                    .OnceSingleAsync<Reservacion>();

                if (reservacionExistente == null)
                {
                    await DisplayAlert("Error", "El ID de la reservación no existe", "Aceptar");
                    return;
                }

                // Obtener los nuevos valores seleccionados en el Picker, DatePicker y TimePicker
                string nuevaSala = pickermodifisala.SelectedItem.ToString();
                DateTime nuevaFecha = datemodifiPicker.Date;

                // Establece los minutos y segundos del timePicker a cero
                TimeSpan nuevaHora = new TimeSpan(timemodifiPicker.Time.Hours, 0, 0);

                if (nuevaHora < new TimeSpan(7, 0, 0) || nuevaHora > new TimeSpan(20, 0, 0))
                {
                    await DisplayAlert("Error", "La hora seleccionada debe estar entre las 7am y las 8pm", "Aceptar");
                    return;
                }

                // Verificar si ya existe una reservación para la sala, fecha y hora seleccionadas
                var reservaciones = await firebase
                    .Child("reservaciones")
                    .OrderBy("Fecha")
                    .EqualTo(nuevaFecha.ToString("yyyy-MM-dd"))
                    .OnceAsync<Reservacion>();

                bool salaYaReservada = reservaciones.Any(r =>
                    r.Key != idReservacion && // Excluye la reserva que se está modificando
                    r.Object.Sala == nuevaSala &&
                    r.Object.Hora == nuevaHora.ToString(@"hh\:mm"));

                if (salaYaReservada)
                {
                    await DisplayAlert("Error", "La sala seleccionada ya está reservada para esta fecha y hora", "Aceptar");
                    return;
                }

                // Actualizar los datos de la reservación en Firebase
                await firebase
                    .Child("reservaciones")
                    .Child(idReservacion)
                    .PutAsync(new Reservacion
                    {
                        Id = idReservacion,
                        Sala = nuevaSala,
                        Fecha = nuevaFecha.ToString("yyyy-MM-dd"),
                        Hora = nuevaHora.ToString(@"hh\:mm")
                    });

                // Mostrar un mensaje de éxito
                await DisplayAlert("Éxito", "Los cambios en la reservación han sido guardados correctamente", "Aceptar");
            }
            catch (Exception ex)
            {
                // Manejo de errores aquí...
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }





    }

}