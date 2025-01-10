
using ClinicalUtp.Controllers;
using ClinicalUtp.Models;
using ClinicalUtp.viewModels;
using ClinicalUtp.views;

namespace ClinicalUtp
{
    public partial class MainPage : ContentPage
    {
    
        public MainPage()
        {
            InitializeComponent();

            var httpClient = new HttpClient();
            var loginServices = new LoginServices(httpClient);
        
            // Pasa ambos servicios al constructor de LoginViewModel
            this.BindingContext = new LoginViewModel(loginServices);

           
        }

        private async void OnLabelTapped(object sender, EventArgs e)
        {
            // Navegar a Page2
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
