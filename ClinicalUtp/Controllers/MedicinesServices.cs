using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ClinicalUtp.Controllers;
using ClinicalUtp.Models;

namespace ClinicalUtp.Controllers
{
    public class MedicinesServices
    {
        private readonly HttpClient _httpClient;
        public MedicinesServices(HttpClient httpClient)
        {
            _httpClient = HttpClientProvider.Client;

        }

        public async Task<List<MedicineDto>> GetMedicinesAsync()
        {
              var response = await _httpClient.GetAsync("Medicines");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<MedicineDto>>();
                }
                return new List<MedicineDto>();

        }


        public async Task<MedicineDto> GetMedicineAsync(int id)
        {
            var medicine = await _httpClient.GetFromJsonAsync<MedicineDto>($"Medicine/{id}");
            return medicine;
        }

      
        public async Task<bool> AddMedicineAsync(MedicineDto newMedicine)
        {
            var response = await _httpClient.PostAsJsonAsync("Medicine", newMedicine);
            return response.IsSuccessStatusCode;
        }


    }


}
