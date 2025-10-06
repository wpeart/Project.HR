using Project.HR.Domain.DTOs;
using Project.HR.Domain.Models;
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


        public async Task<EmployeeDTO?> GetEmployeeByIdAsync(int id)
        {
            var client = _clientFactory.CreateClient("HRApiClient");
            var response = await client.GetAsync($"api/employees/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<EmployeeDTO>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        public async Task<EmployeeDTO?> CreateEmployeeAsync(EmployeeDTO employee)
        {
            var client = _clientFactory.CreateClient("HRApiClient");
            var response = await client.PostAsJsonAsync("api/employees", employee);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<EmployeeDTO>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return null;
        }

        public async Task<Employee?> UpdateEmployeeAsync(int id, EmployeeDTO employee)
        {
            var client = _clientFactory.CreateClient("HRApiClient");
            var response = await client.PutAsJsonAsync($"api/employees/{id}", employee);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Employee>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Update failed: {response.StatusCode} - {errorContent}");

            return null;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var client = _clientFactory.CreateClient("HRApiClient");
            var response = await client.DeleteAsync($"api/employees/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return false;
        }

    }
}
