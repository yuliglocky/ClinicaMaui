using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicalUtp.Controllers
{
    public static class HttpClientProvider
    {
        private static readonly HttpClient _httpClient;

        static HttpClientProvider()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://2f99-200-124-21-55.ngrok-free.app/api/")
            };
        }   

        public static HttpClient Client => _httpClient;
    }
}
