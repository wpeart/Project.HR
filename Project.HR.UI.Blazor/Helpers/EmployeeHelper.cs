using Project.HR.Domain.DTOs;
using System.Text.Json;

namespace Project.HR.UI.Blazor.Helpers
{
    public class EmployeeHelper
    {
        private readonly IHttpClientFactory _clientFactory;

        public EmployeeHelper(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<EmployeeDTO>> GetEmployeesAsync()
        {
            var client = _clientFactory.CreateClient("HRApiClient");
            var response = await client.GetAsync("api/employees");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<EmployeeDTO>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return new List<EmployeeDTO>();
        }

    }
}
