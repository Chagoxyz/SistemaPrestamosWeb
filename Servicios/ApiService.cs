using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPrestamosWeb.Servicios
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress =
                new System.Uri("https://localhost:7104/api/");
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<T> PostAsync<T>(
            string url,
            object datos,
            int? idUsuarioAuditoria = null)
        {
            var contenido = new StringContent(
                JsonConvert.SerializeObject(datos),
                Encoding.UTF8,
                "application/json");

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                url);

            request.Content = contenido;

            AgregarUsuarioAuditoria(
                request,
                idUsuarioAuditoria);

            var response =
                await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var json =
                await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<T> PutAsync<T>(
    string url,
    object datos,
    int? idUsuarioAuditoria = null)
        {
            var contenido = new StringContent(
                JsonConvert.SerializeObject(datos),
                Encoding.UTF8,
                "application/json");

            var request = new HttpRequestMessage(
                HttpMethod.Put,
                url);

            request.Content = contenido;

            AgregarUsuarioAuditoria(
                request,
                idUsuarioAuditoria);

            var response = await _httpClient.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new System.Exception(
                    $"API {(int)response.StatusCode}: {json}"
                );
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task DeleteAsync(
            string url,
            int? idUsuarioAuditoria = null)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Delete,
                url);

            AgregarUsuarioAuditoria(
                request,
                idUsuarioAuditoria);

            var response =
                await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        private void AgregarUsuarioAuditoria(
            HttpRequestMessage request,
            int? idUsuarioAuditoria)
        {
            if (idUsuarioAuditoria.HasValue)
            {
                request.Headers.Add(
                    "X-Usuario-Id",
                    idUsuarioAuditoria.Value.ToString());
            }
        }
    }
}