using EV3_EQ5.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EV3_EQ5.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Inicio : ContentPage
    {
        public class ImageInformation
        {
            public ImageSource _Image { get; set; }

        }

        private ObservableCollection<ImageInformation> imageCollection;

        public ObservableCollection<ImageInformation> ImageCollection
        {
            get { return imageCollection; }
            set
            {
                imageCollection = value;
                OnPropertyChanged();

            }
        }

        public Inicio()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = this;
            ImageCollection = new ObservableCollection<ImageInformation>
            {
                new ImageInformation {_Image = "Sala1.jpg"},
                new ImageInformation {_Image = "Sala2.jpg"},
                new ImageInformation {_Image = "Sala3.jpg"}
            };
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private async void btnAparta_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ApartaSala());
        }

        private async void atrasInicio_Clicked(object sender, EventArgs e)
        {
            VariablesGlobales.NombreUsuario = null;
            VariablesGlobales.EdadUsuario = null;
            VariablesGlobales.Matricula = null;
            VariablesGlobales.Correo = null;

            await Application.Current.MainPage.Navigation.PushModalAsync(new Login());
        }

        private async void perfil_Clicked(object sender, EventArgs e)
        {
           await Application.Current.MainPage.Navigation.PushAsync(new Perfil());
            
        }

        private async void btnreglamento_Clicked(object sender, EventArgs e)
        {
           await Application.Current.MainPage.Navigation.PushAsync(new Reglamento());
        }
    }
}