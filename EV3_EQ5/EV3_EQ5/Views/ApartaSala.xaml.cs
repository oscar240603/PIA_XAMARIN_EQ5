using EV3_EQ5.Models;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EV3_EQ5.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ApartaSala : ContentPage
	{
        List<numitem> numero;
        FirebaseClient firebase;

        public ApartaSala ()
		{
			InitializeComponent ();
			InitApp ();
            InitializeDateTimePickers();
            NavigationPage.SetHasNavigationBar(this, false);
            firebase = new FirebaseClient("https://piaprogra89-default-rtdb.firebaseio.com/");
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
                pickersala.Items.Add(Num.Name);
            }


        }


        private void pickersala_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }




        /*private async void save_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Obtiene los valores seleccionados del Picker, DatePicker y TimePicker
                string salaSeleccionada = pickersala.SelectedItem.ToString();
                DateTime fechaSeleccionada = datePicker.Date;
                TimeSpan horaSeleccionada = timePicker.Time;

                // Verificar si ya existe una reservación para la sala, fecha y hora seleccionadas
                var reservaciones = await firebase
                    .Child("reservaciones")
                    .OrderBy("Fecha")
                    .EqualTo(fechaSeleccionada.ToString("yyyy-MM-dd"))
                    .OnceAsync<Reservacion>();

                bool salaYaReservada = reservaciones.Any(r =>
                    r.Object.Sala == salaSeleccionada &&
                    r.Object.Hora == horaSeleccionada.ToString(@"hh\:mm"));

                if (salaYaReservada)
                {
                    await DisplayAlert("Error", "La sala seleccionada ya está reservada para esta fecha y hora", "Aceptar");
                    return;
                }

                // Si la sala no está reservada, se crea una nueva reservación
                var reserva = await firebase.Child("reservaciones").PostAsync(new
                {
                    Sala = salaSeleccionada,
                    Fecha = fechaSeleccionada.ToString("yyyy-MM-dd"),
                    Hora = horaSeleccionada.ToString(@"hh\:mm")
                });

                // Obtiene el ID único generado por Firebase para la reserva
                string reservaId = reserva.Key;

                // Muestra un mensaje de éxito con el ID de la reservación
                await DisplayAlert("Éxito", $"Reservación guardada correctamente\nID de reservación: {reservaId}", "Aceptar");
            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si ocurre alguna excepción
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }*/

        private string GenerarIdUnico(string sala, DateTime fecha, TimeSpan hora)
        {
            // Concatena la sala, la fecha y la hora para formar una cadena
            string cadenaUnica = sala + fecha.ToString("yyyyMMdd") + hora.ToString(@"hh\:mm");

            // Calcula el hash de la cadena única
            using (var sha = new System.Security.Cryptography.SHA1Managed())
            {
                var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(cadenaUnica));
                // Convierte el hash a una cadena hexadecimal
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }

        private void InitializeDateTimePickers()
        {
            datePicker.MinimumDate = DateTime.Today;

            timePicker.Time = new TimeSpan(7, 0, 0); // Hora mínima: 7am
            timePicker.Format = "hh:mm tt"; // Formato de 12 horas
            timePicker.PropertyChanged += (sender, e) =>
            {
                if (timePicker.Time < new TimeSpan(7, 0, 0)) // Si la hora es antes de las 7am
                {
                    timePicker.Time = new TimeSpan(7, 0, 0); // Establecer la hora a las 7am
                }
                else if (timePicker.Time > new TimeSpan(20, 0, 0)) // Si la hora es después de las 8pm
                {
                    timePicker.Time = new TimeSpan(20, 0, 0); // Establecer la hora a las 8pm
                }
            };
        }

        /*private async void save_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Obtiene los valores seleccionados del Picker, DatePicker y TimePicker
                string salaSeleccionada = pickersala.SelectedItem.ToString();
                DateTime fechaSeleccionada = datePicker.Date;
                TimeSpan horaSeleccionada = timePicker.Time;

                if (horaSeleccionada < new TimeSpan(7, 0, 0) || horaSeleccionada > new TimeSpan(20, 0, 0))
                {
                    await DisplayAlert("Error", "La hora seleccionada debe estar entre las 7am y las 8pm", "Aceptar");
                    return;
                }

                // Verificar si ya existe una reservación para la sala, fecha y hora seleccionadas
                var reservaciones = await firebase
                    .Child("reservaciones")
                    .OrderBy("Fecha")
                    .EqualTo(fechaSeleccionada.ToString("yyyy-MM-dd"))
                    .OnceAsync<Reservacion>();

                bool salaYaReservada = reservaciones.Any(r =>
                    r.Object.Sala == salaSeleccionada &&
                    r.Object.Hora == horaSeleccionada.ToString(@"hh\:mm"));

                if (salaYaReservada)
                {
                    await DisplayAlert("Error", "La sala seleccionada ya está reservada para esta fecha y hora", "Aceptar");
                    return;
                }

                // Combina la fecha y la hora en un solo valor para usarlo como ID
                string idUnico = salaSeleccionada.ToString() + fechaSeleccionada.ToString("yyyyMMdd") + horaSeleccionada.ToString(@"hh\:mm");

                // Crea una referencia directamente al nodo de la reserva usando el ID único
                var reservaRef = firebase.Child("reservaciones").Child(idUnico);

                // Verifica si ya existe una reserva con el mismo ID único
                var reservaExistente = await reservaRef.OnceSingleAsync<Reservacion>();
                if (reservaExistente != null)
                {
                    await DisplayAlert("Error", "La sala seleccionada ya está reservada para esta fecha y hora", "Aceptar");
                    return;
                }

                // Si no hay una reserva existente, crea una nueva
                await reservaRef.PutAsync(new
                {
                    Sala = salaSeleccionada,
                    Fecha = fechaSeleccionada.ToString("yyyy-MM-dd"),
                    Hora = horaSeleccionada.ToString(@"hh\:mm")
                });

                // Muestra un mensaje de éxito con el ID de la reservación
                await DisplayAlert("Éxito", $"Reservación guardada correctamente\n" + $"Guarda el ID para futuras modificaciones de la reservacion\n" +
                    $"ID de reservación: {idUnico}", "Aceptar");

            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si ocurre alguna excepción
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }*/

        /*private async void save_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Obtiene los valores seleccionados del Picker, DatePicker y TimePicker
                string salaSeleccionada = pickersala.SelectedItem.ToString();
                DateTime fechaSeleccionada = datePicker.Date;

                // Establece los minutos y segundos del timePicker a cero
                TimeSpan horaSeleccionada = new TimeSpan(timePicker.Time.Hours, 0, 0);

                if (horaSeleccionada < new TimeSpan(7, 0, 0) || horaSeleccionada > new TimeSpan(20, 0, 0))
                {
                    await DisplayAlert("Error", "La hora seleccionada debe estar entre las 7am y las 8pm", "Aceptar");
                    return;
                }

                // Verificar si ya existe una reservación para la sala, fecha y hora seleccionadas
                var reservaciones = await firebase
                    .Child("reservaciones")
                    .OrderBy("Fecha")
                    .EqualTo(fechaSeleccionada.ToString("yyyy-MM-dd"))
                    .OnceAsync<Reservacion>();

                bool salaYaReservada = reservaciones.Any(r =>
                    r.Object.Sala == salaSeleccionada &&
                    r.Object.Hora == horaSeleccionada.ToString(@"hh\:mm"));

                if (salaYaReservada)
                {
                    await DisplayAlert("Error", "La sala seleccionada ya está reservada para esta fecha y hora", "Aceptar");
                    return;
                }

                // Combina la fecha y la hora en un solo valor para usarlo como ID
                string idUnico = salaSeleccionada.ToString() + fechaSeleccionada.ToString("yyyyMMdd") + horaSeleccionada.ToString(@"hh\:mm");

                // Crea una referencia directamente al nodo de la reserva usando el ID único
                var reservaRef = firebase.Child("reservaciones").Child(idUnico);

                // Verifica si ya existe una reserva con el mismo ID único
                var reservaExistente = await reservaRef.OnceSingleAsync<Reservacion>();
                if (reservaExistente != null)
                {
                    await DisplayAlert("Error", "La sala seleccionada ya está reservada para esta fecha y hora", "Aceptar");
                    return;
                }

                // Si no hay una reserva existente, crea una nueva
                await reservaRef.PutAsync(new
                {
                    Sala = salaSeleccionada,
                    Fecha = fechaSeleccionada.ToString("yyyy-MM-dd"),
                    Hora = horaSeleccionada.ToString(@"hh\:mm")
                });

                // Muestra un mensaje de éxito con el ID de la reservación
                await DisplayAlert("Éxito", $"Reservación guardada correctamente\n" + $"Guarda el ID para futuras modificaciones de la reservación\n" +
                    $"ID de reservación: {idUnico}", "Aceptar");

            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si ocurre alguna excepción
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }*/

        private async void save_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Verificar si se ha seleccionado una sala
                if (pickersala.SelectedIndex == -1)
                {
                    await DisplayAlert("Error", "Por favor, seleccione una sala", "Aceptar");
                    return;
                }

                // Obtiene los valores seleccionados del Picker, DatePicker y TimePicker
                string salaSeleccionada = pickersala.SelectedItem.ToString();
                DateTime fechaSeleccionada = datePicker.Date;

                // Establece los minutos y segundos del timePicker a cero
                TimeSpan horaSeleccionada = new TimeSpan(timePicker.Time.Hours, 0, 0);

                if (horaSeleccionada < new TimeSpan(7, 0, 0) || horaSeleccionada > new TimeSpan(20, 0, 0))
                {
                    await DisplayAlert("Error", "La hora seleccionada debe estar entre las 7am y las 8pm", "Aceptar");
                    return;
                }

                // Verificar si ya existe una reservación para la sala, fecha y hora seleccionadas
                var reservaciones = await firebase
                    .Child("reservaciones")
                    .OrderBy("Fecha")
                    .EqualTo(fechaSeleccionada.ToString("yyyy-MM-dd"))
                    .OnceAsync<Reservacion>();

                bool salaYaReservada = reservaciones.Any(r =>
                    r.Object.Sala == salaSeleccionada &&
                    r.Object.Hora == horaSeleccionada.ToString(@"hh\:mm"));

                if (salaYaReservada)
                {
                    await DisplayAlert("Error", "La sala seleccionada ya está reservada para esta fecha y hora", "Aceptar");
                    return;
                }

                // Combina la fecha y la hora en un solo valor para usarlo como ID
                string idUnico = salaSeleccionada.ToString() + fechaSeleccionada.ToString("yyyyMMdd") + horaSeleccionada.ToString(@"hh\:mm");

                // Crea una referencia directamente al nodo de la reserva usando el ID único
                var reservaRef = firebase.Child("reservaciones").Child(idUnico);

                // Verifica si ya existe una reserva con el mismo ID único
                var reservaExistente = await reservaRef.OnceSingleAsync<Reservacion>();
                if (reservaExistente != null)
                {
                    await DisplayAlert("Error", "La sala seleccionada ya está reservada para esta fecha y hora", "Aceptar");
                    return;
                }

                // Si no hay una reserva existente, crea una nueva
                await reservaRef.PutAsync(new
                {
                    Sala = salaSeleccionada,
                    Fecha = fechaSeleccionada.ToString("yyyy-MM-dd"),
                    Hora = horaSeleccionada.ToString(@"hh\:mm")
                });

                // Muestra un mensaje de éxito con el ID de la reservación
                await DisplayAlert("Éxito", $"Reservación guardada correctamente\n" + $"Guarda el ID para futuras modificaciones de la reservación\n" +
                    $"ID de reservación: {idUnico}", "Aceptar");

            }
            catch (Exception ex)
            {
                // Muestra un mensaje de error si ocurre alguna excepción
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }





        private async void consuSala_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new Consultarsala());
        }

        private async void atrasAparta_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new Inicio());
        }

        private async void modifiSala_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ModificarSala());
        }
    }
}