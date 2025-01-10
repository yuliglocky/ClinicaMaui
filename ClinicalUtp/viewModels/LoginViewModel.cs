using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ClinicalUtp.Models;
using ClinicalUtp.Controllers;
using System.Collections;
using System.Windows.Input;
using ClinicalUtp.views;
using static ClinicalUtp.Controllers.LoginServices;
using ClinicalUtp.views.UsersViews;

namespace ClinicalUtp.viewModels
{

    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly LoginServices _loginServices;
       // Un servicio para manejar la sesión del usuario

       

        public LoginViewModel(LoginServices loginServices)
        {
           
            _loginServices = loginServices;
            LoginCommand = new Command(async () => await Login());
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Comando de inicio de sesión
        public async Task Login()
        {
            var loginDto = new LoginDto { Email = Email, Password = Password };

            try
            {
                // Intenta iniciar sesión como Admin
                var adminResult = await _loginServices.LoginAdminAsync(loginDto);
                if (adminResult != null)
                {
                    // Si es Admin, navega a la página de perfil del admin
                    await Application.Current.MainPage.Navigation.PushAsync(new AdminProfilePage());
                    return;
                }

                // Si no es Admin, intenta iniciar sesión como Doctor
                var doctorResult = await _loginServices.LoginDoctorAsync(loginDto);
                if (doctorResult != null)
                {
                    Preferences.Set("IdPaciente", doctorResult ?? 0); // Guarda userResult o 0 si es null

                    // Navega a la página de perfil con el UserId (0 si es null)
                    await Application.Current.MainPage.Navigation.PushAsync(new HomePage(doctorResult ?? 0));
                    return;
                }

                var userResult = await _loginServices.LoginUserAsync(loginDto);

                if (userResult != null)
                {
                    Preferences.Set("IdPaciente", userResult ?? 0); // Guarda userResult o 0 si es null

                    // Navega a la página de perfil con el UserId (0 si es null)
                    await Application.Current.MainPage.Navigation.PushAsync(new HomePageUsers(userResult ?? 0));
                    return;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Login fallido. Verifica tus credenciales.", "OK");
                }

                // Si ninguno de los logins tiene éxito, muestra un mensaje de error
                await Application.Current.MainPage.DisplayAlert("Error", "Usuario no encontrado o contraseña incorrecta", "OK");
            }
            catch (Exception ex)
            {
                // Manejar error, como mostrar un mensaje al usuario
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }
    }
}


