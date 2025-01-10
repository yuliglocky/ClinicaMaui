using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

using ClinicalUtp.Models;
using System.Text.Json;
using System.Diagnostics;
using System.Net;

namespace ClinicalUtp.Controllers
{
    public class AppointmentsServices
    {
        private readonly HttpClient _httpClient;

        // Constructor que recibe HttpClientProvider.Client
        public AppointmentsServices(HttpClient httpClient)
        {
            // Usa el HttpClient proporcionado por HttpClientProvider.Client
            _httpClient = HttpClientProvider.Client;
        }
        // Método para obtener las citas con estado 0
        public async Task<List<AppointmentDto>> GetAppointmentsWithStatus0()
        {
            var response = await _httpClient.GetAsync("Appointment/status-01");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AppointmentDto>>();
            }
            return new List<AppointmentDto>();
        }

        //darle medicina al usuario , cada vez que se atiende 
        public async Task<string> CreateAppointmentMedicineAsync(AppointmentxMedicinesDto appointmentMedicineDto)
        {
            try
            {
                // Realiza la solicitud POST para crear la cita y el medicamento
                var response = await _httpClient.PostAsJsonAsync("AppointmentMedicine", appointmentMedicineDto);
                response.EnsureSuccessStatusCode();

                // Lee la respuesta como una cadena (mensaje de éxito o error)
                var resultMessage = await response.Content.ReadAsStringAsync();
                return resultMessage;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating appointment and medicine: {ex.Message}");
                return "Error en la creación de la cita y medicamento.";
            }
        }


        // Métodos adicionales para otros estados si es necesario:
        public async Task<List<AppointmentDto>> GetAppointmentsWithStatus1()
        {
            var response = await _httpClient.GetAsync("Appointment/status-1");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AppointmentDto>>();
            }
            return new List<AppointmentDto>();
        }

        //citas finalizadas (para colocarlas algun dia, no son necesarias todavia )
        public async Task<List<AppointmentDto>> GetAppointmentsWithStatus2()
        {
            var response = await _httpClient.GetAsync("Appointment/status-2");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AppointmentDto>>();
            }
            return new List<AppointmentDto>();

        }

        //para traer appointment porid 
        public async Task<AppointmentDto> GetAppointmentDetailsAsync(int appointmentId)
        {
            var response = await _httpClient.GetAsync($"Appointment/{appointmentId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AppointmentDto>();
            }
            return null;
        }


        // anadir informacion medica 
        public async Task<string> AddMedicalInformationAsync(MedicalInformationDto medicalInfoDto)
        {
            try
            {
                // Realizar la solicitud POST para agregar la información médica
                var response = await _httpClient.PostAsJsonAsync("MedicalInformacion", medicalInfoDto);
                response.EnsureSuccessStatusCode();
                // Leer la respuesta como un mensaje
                var resultMessage = await response.Content.ReadAsStringAsync();
                return resultMessage;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return $"Error al agregar la información médica: {ex.Message}";
            }
        }
        // Método para actualizar el estado de una cita a 1 (Aprobada)
        public async Task<bool> ApproveAppointment(int id)
        {
            var response = await _httpClient.PutAsync($"Appointment/{id}/update-status-1", null);
            return response.IsSuccessStatusCode;
        }

        // Método para actualizar el estado de una cita a 2 (Rechazada)
        public async Task<bool> RejectAppointment(int id)
        {
            var response = await _httpClient.PutAsync($"Appointment/{id}/update-status-2", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> FinalizadaAppointment(int id)
        {
            var response = await _httpClient.PutAsync($"Appointment/{id}/update-status-3", null);
            return response.IsSuccessStatusCode;
        }


        // Obtener referencias por usuario
        public async Task<List<ReferencesDto>> GetReferencesByUserIdAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"Informes/GetReferences/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                // Parsear el JSON a JsonDocument para deserializar manualmente
                var jsonDocument = JsonDocument.Parse(json);
                var rootElement = jsonDocument.RootElement;

                // Crear una lista de referencias
                var references = new List<ReferencesDto>();

                // Recorrer cada elemento dentro del array JSON de referencias
                foreach (var referenceElement in rootElement.EnumerateArray())
                {
                    // Deserializar manualmente cada referencia
                    var reference = new ReferencesDto
                    {
                        DoctorId = referenceElement.GetProperty("doctorId").GetInt32(),
                        UserId = referenceElement.GetProperty("userId").GetInt32(),
                        ReferenceId = referenceElement.GetProperty("referenceId").GetInt32(),
                        DoctorName = referenceElement.GetProperty("doctorName").GetString(),
                        DoctorSpecialty = referenceElement.GetProperty("doctorSpecialty").GetString(),
                        Reason = referenceElement.GetProperty("reason").GetString(),
                        Diagnosis = referenceElement.GetProperty("diagnosis").GetString(),
                        DoctorSuggestions = referenceElement.GetProperty("doctorSuggestions").GetString(),
                        ReferenceDate = referenceElement.GetProperty("referenceDate").GetDateTime(),
                        FollowUpDate = referenceElement.GetProperty("followUpDate").GetDateTime()
                    };

                    // Agregar la referencia deserializada a la lista
                    references.Add(reference);
                }

                // Devolver la lista de referencias deserializadas
                return references;
            }

            // En caso de error, devolver una lista vacía
            return new List<ReferencesDto>();
        }

        // Método para enviar el PDF de una referencia específica
        public async Task<string> SendReferencePdfAsync(int userId, int referenceId)
        {
            try
            {
                // Construye la URL
                var url = $"Informes/SendReferencePdf/{userId}/{referenceId}";

                // Realiza la solicitud POST
                var response = await _httpClient.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    // Leer el mensaje de respuesta
                    var resultMessage = await response.Content.ReadAsStringAsync();
                    return $"Éxito: {resultMessage}";
                }
                else
                {
                    // Leer el error si la solicitud falla
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return $"Error: {response.StatusCode} - {errorMessage}";
                }
            }
            catch (Exception ex)
            {
                return $"Excepción: {ex.Message}";
            }
        }








        //crear referencias
        public async Task<bool> CreateReferenceAsync(ReferencesDto referenceDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Informes", referenceDto);

                if (response.IsSuccessStatusCode)
                {
                    // Procesar la respuesta si es necesario
                    return true;
                }
                else
                {
                    // Manejar errores o respuestas no exitosas
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones, puedes usar un log o notificación de error
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }



        //Llamar referencia por doctor para enviar pdf a su correo 
        public async Task<List<ReferencesDto>> GetReferencesByDoctorIdAsync(int doctorId)
        {
            var response = await _httpClient.GetAsync($"Informes/GetReferencesByDoctors/{doctorId}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                // Parsear el JSON a JsonDocument para deserializar manualmente
                var jsonDocument = JsonDocument.Parse(json);
                var rootElement = jsonDocument.RootElement;

                // Crear una lista de referencias
                var references = new List<ReferencesDto>();

                // Recorrer cada elemento dentro del array JSON de referencias
                foreach (var referenceElement in rootElement.EnumerateArray())
                {
                    // Deserializar manualmente cada referencia
                    var reference = new ReferencesDto
                    {
                       
                        DoctorId = referenceElement.GetProperty("doctorId").GetInt32(),
                        UserId = referenceElement.GetProperty("userId").GetInt32(),
                        DoctorName = referenceElement.GetProperty("doctorName").GetString(),
                        DoctorSpecialty = referenceElement.GetProperty("doctorSpecialty").GetString(),
                        Reason = referenceElement.GetProperty("reason").GetString(),
                        Diagnosis = referenceElement.GetProperty("diagnosis").GetString(),
                        DoctorSuggestions = referenceElement.GetProperty("doctorSuggestions").GetString(),
                        ReferenceDate = referenceElement.GetProperty("referenceDate").GetDateTime(),
                        FollowUpDate = referenceElement.GetProperty("followUpDate").GetDateTime()
                       
                   
                    };

                    // Agregar la referencia deserializada a la lista
                    references.Add(reference);
                }

                // Devolver la lista de referencias deserializadas
                return references;
            }

            // En caso de error, devolver una lista vacía
            return new List<ReferencesDto>();
        }


        //mandar el hsitorial de cita po gmail
        public async Task<(bool IsSuccess, string Message)> GetReferencesAndSendEmail(int doctorId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Informes/GetReferencesDoctor/{doctorId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    // Puedes procesar la respuesta si es necesario

                    // Si la respuesta es exitosa y la operación de enviar el correo es correcta
                    return (true, "El historial de citas fue enviado con éxito a su correo.");
                }
                else
                {
                    return (false, "Hubo un error al obtener las referencias.");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }


        //para obtener las creendenciales de doctor 
        public async Task<UserDto> GetDoctorByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"Doctor/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonDocument.Parse(json).RootElement;

                    // Deserializar manualmente
                    return new UserDto
                    {
                        Id = jsonObject.GetProperty("id").GetInt32(),
                       Name = jsonObject.GetProperty("name").GetString(),
                       Email = jsonObject.GetProperty("email").GetString(),
                       Dni = jsonObject.GetProperty("dni").GetString(),
                       Address = jsonObject.GetProperty("address").GetString(),
                       Cellphone = jsonObject.GetProperty("cellphone").GetString(),
                       Role = jsonObject.TryGetProperty("role", out var roleProperty) && roleProperty.ValueKind == JsonValueKind.String
                            ? roleProperty.GetString()
                            : null // Manejo de valores nulos o no string
                    };
                }
                else
                {
                    throw new Exception($"Error al obtener el doctor: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error de conexión: {ex.Message}");
            }
        }

    }
}
