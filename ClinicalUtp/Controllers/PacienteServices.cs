using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ClinicalUtp.Models;
using System.Text.Json;



namespace ClinicalUtp.Controllers
{

    public class PacienteServices
    {
        private readonly HttpClient _httpClient;


        public PacienteServices(HttpClient httpClient)
        {
            _httpClient = HttpClientProvider.Client;

        }

        public async Task<List<DoctorDto>> GetDoctorsBasicAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<DoctorDto>>("Doctor/Basic");
            return response ?? new List<DoctorDto>();
        }

        public async Task<List<DoctorDto>> GetDoctorsAsync()
        {
            var response = await _httpClient.GetAsync("Doctor");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<DoctorDto>>();
            }
            return null;
        }

        public async Task<List<ServicioDto>> GetServiciosAsync()
        {
            var servicios = await _httpClient.GetFromJsonAsync<List<ServicioDto>>("Servicio");
            return servicios;
        }
        public async Task<AppointmentDto> CreateAppointmentAsync(AppointmentDto appointmentCreateDto)
        {
            try
            {
                // Realiza la solicitud POST para crear una cita
                var response = await _httpClient.PostAsJsonAsync("Appointment", appointmentCreateDto);
                response.EnsureSuccessStatusCode();

                // Deserializar la respuesta en un AppointmentDto
                var appointment = await response.Content.ReadFromJsonAsync<AppointmentDto>();
                return appointment;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating appointment: {ex.Message}");
                return null;
            }
        }


        // Método para agregar una donación
        public async Task<DonationDto> AddDonationAsync(DonationDto donationDto)
        {
            var response = await _httpClient.PostAsJsonAsync("Donation", donationDto); // Cambia la URL si es necesario

            if (response.IsSuccessStatusCode)
            {
                // Si la respuesta es exitosa, deserializamos el contenido de la respuesta
                return await response.Content.ReadFromJsonAsync<DonationDto>();
            }
            else
            {
                // Si no es exitosa, lanzamos una excepción con el mensaje de error
                throw new Exception($"Error al agregar la donación: {response.ReasonPhrase}");
            }
        }


        //Para Traer usuarios donadores
        public async Task<List<UserDto>> GetAllUserDetailsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Paciente/usuarios");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var users = System.Text.Json.JsonSerializer.Deserialize<List<UserDto>>(content, new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (users != null)
                    {
                        // Guardar la lista completa en Preferences
                        SaveUsersToPreferences(users);
                        return users;
                    }
                }

                // Si la API falla, recuperar de Preferences
                return GetUsersFromPreferences();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los detalles de los usuarios: {ex.Message}");

                // Recuperar desde Preferences en caso de error
                return GetUsersFromPreferences();
            }
        }


        //guardar usuarios en preferences
        public void SaveUsersToPreferences(List<UserDto> users)
        {
            try
            {
                // Serializar la lista de usuarios a JSON
                var usersJson = System.Text.Json.JsonSerializer.Serialize(users);

                // Guardar en Preferences
                Preferences.Set("Usuarios", usersJson);

                Console.WriteLine("Usuarios guardados en Preferences.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar usuarios en Preferences: {ex.Message}");
            }
        }


        //para traer usuarios donadores
        public List<UserDto> GetUsersFromPreferences()
        {
            try
            {
                // Obtener la lista de usuarios en formato JSON
                var usersJson = Preferences.Get("Usuarios", string.Empty);

                // Deserializar si hay datos
                if (!string.IsNullOrEmpty(usersJson))
                {
                    return System.Text.Json.JsonSerializer.Deserialize<List<UserDto>>(usersJson) ?? new List<UserDto>();
                }

                return new List<UserDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener usuarios desde Preferences: {ex.Message}");
                return new List<UserDto>();
            }
        }


        //para registrar paciente

        public async Task<string> RegisterPacienteAsync(UserDto dto)
        {
            try
            {
                // Enviar el DTO al backend
                var response = await _httpClient.PostAsJsonAsync("Paciente/registerPaciente", dto);

                // Verificar si la respuesta es exitosa
                if (response.IsSuccessStatusCode)
                {
                    return "Registro exitoso.";
                }
                else
                {
                    // Obtener el código de estado y el mensaje de error
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return $"Error en el registro. Código de estado: {response.StatusCode}. Mensaje: {errorMessage}";
                }
            }
            catch (Exception ex)
            {
                // Manejo básico de excepciones
                return $"Error: {ex.Message}";
            }
        }

        //APOINTMENT PARA USER PACIENTE
        // Obtener citas con estado 0
        public async Task<List<AppointmentDto>> GetAppointmentsWithUserIdAndStatus0(int userId)
        {
            var url = $"Appointment/user/{userId}/status-0"; // Estado pendiente
            return await GetAppointmentsByUrl(url);
        }

        // Obtener citas con estado 1
        public async Task<List<AppointmentDto>> GetAppointmentsWithUserIdAndStatus1(int userId)
        {
            var url = $"Appointment/user/{userId}/status-1"; // aprobadas
            return await GetAppointmentsByUrl(url);
        }

        // Obtener citas con estado 2 apro
        public async Task<List<AppointmentDto>> GetAppointmentsWithUserIdAndStatus2(int userId)
        {
            var url = $"Appointment/user/{userId}/status-2";
            return await GetAppointmentsByUrl(url);
        }

        // Método común para realizar la solicitud HTTP
        private async Task<List<AppointmentDto>> GetAppointmentsByUrl(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<AppointmentDto>>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las citas: {ex.Message}");
            }

            return new List<AppointmentDto>(); // Retornar lista vacía en caso de error
        }


        // obtener las donaciones de mi usuario 
        public async Task<List<DonationDto>> GetDonation(int userId)
        {
            try
            {
                // Construye la URL para obtener las donaciones por usuario
                var url = $"Donation/{userId}"; // Asegúrate de que esta ruta coincida con tu API
                var response = await _httpClient.GetAsync(url);

                // Verifica si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Deserializa el contenido en una lista de DonationDto
                    return await response.Content.ReadFromJsonAsync<List<DonationDto>>() ?? new List<DonationDto>();
                }
                else
                {
                    // Maneja posibles errores de la API
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al obtener las donaciones: {response.StatusCode} - {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error en GetDonationsByUserIdAsync: {ex.Message}");
                return new List<DonationDto>(); // Retorna una lista vacía en caso de error
            }
        }
    }

   



}
