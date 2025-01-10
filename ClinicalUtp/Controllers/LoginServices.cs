using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ClinicalUtp.Models;

namespace ClinicalUtp.Controllers
{
    public class LoginServices
    {
        private readonly HttpClient _httpClient;
        public UserSessionService UserSession { get; private set; }
        public LoginServices (HttpClient httpClient)
        {
            _httpClient = HttpClientProvider.Client;
            UserSession = new UserSessionService();

        }

        public async Task<object> LoginAdminAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("Admin/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<object>(); // Ajusta el tipo de retorno según tu necesidad
            }

            return null; // Manejar el error o el mensaje de error
        }

        public async Task<int?> LoginUserAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("Paciente/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

                if (result?.UserId != null) // Verificación simplificada
                {
                    Preferences.Set("IdPaciente", (int)result.UserId); // Guarda el UserId en las preferencias
                    return result.UserId;
                }
            }

            return null; // Manejar el error o el mensaje de error aquí si es necesario
        }

        public async Task<int?> LoginDoctorAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("Doctor/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

                if (result?.UserId != null) // Verificación simplificada
                {
                    Preferences.Set("IdPaciente", (int)result.UserId); // Guarda el UserId en las preferencias
                    return result.UserId;
                }
            }

            return null; // Manejar el error o el mensaje de error aquí si es necesario
        }



        public class UserSessionService
        {
            public int? UserId { get; private set; } // Propiedad para almacenar el UserId del usuario logeado

            public void Login(int userId)
            {
                UserId = userId; // Almacena el UserId
            }

            public void Logout()
            {
                UserId = null; // Limpia el UserId al cerrar sesión
            }
        }
    }
}
