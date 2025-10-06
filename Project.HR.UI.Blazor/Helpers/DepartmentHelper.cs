using Project.HR.Domain.DTOs;
using Project.HR.Domain.Helpers;
using Project.HR.Domain.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Project.HR.UI.Blazor.Helpers
{
    public class DepartmentHelper
    {
        private readonly IHttpClientFactory _clientFactory;

        public DepartmentHelper(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<DepartmentDTO>?> GetAllDepartments()
        {
            List<DepartmentDTO>? departments = new List<DepartmentDTO>();

            var client = _clientFactory.CreateClient("HRApiClient");

            departments = await client.GetFromJsonAsync<List<DepartmentDTO>>("api/departments");




            return departments;

        }

        public async Task<DepartmentDTO> GetDepartmentById(int id)
        {
            DepartmentDTO ret = new DepartmentDTO();

            var client = _clientFactory.CreateClient("HRApiClient");

            var response = await client.GetAsync($"api/departments/{id}");

            response.EnsureSuccessStatusCode();

            string strResponse = await response.Content.ReadAsStringAsync();

            LogErrorHelper.LogError(strResponse, null, LogErrorHelper.ErrorLevel.Trace);
            ret = JsonSerializer.Deserialize<DepartmentDTO>(strResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new DepartmentDTO();
            return ret;

        }

        public async Task<DepartmentDTO> CreateDepartmentAsync(DepartmentDTO department)
        {
            DepartmentDTO ret = new DepartmentDTO();

            var client = _clientFactory.CreateClient("HRApiClient");

            if (department != null)
            {
                if (department.ParentDepartmentId == 0)
                {
                    department.ParentDepartmentId = null;
                }
                var response = await client.PostAsJsonAsync("api/departments", department);
                response.EnsureSuccessStatusCode();

                string strResponse = await response.Content.ReadAsStringAsync();
                LogErrorHelper.LogError(strResponse, null, LogErrorHelper.ErrorLevel.Trace);

                ret = JsonSerializer.Deserialize<DepartmentDTO>(strResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new DepartmentDTO();


            }
            return ret;
        }

        public async Task<DepartmentDTO> UpdateDepartmentAsync(int id, DepartmentDTO department)
        {
            var client = _clientFactory.CreateClient("HRApiClient");
            DepartmentDTO ret = new DepartmentDTO();

            if (department != null)
            {
                if (department.ParentDepartmentId == 0)
                {
                    department.ParentDepartmentId = null;
                }
                var response = await client.PutAsJsonAsync($"api/departments/{id}", department);
                response.EnsureSuccessStatusCode();

                string strResponse = await response.Content.ReadAsStringAsync();
                LogErrorHelper.LogError(strResponse, null, LogErrorHelper.ErrorLevel.Trace);

                ret = JsonSerializer.Deserialize<DepartmentDTO>(strResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new DepartmentDTO();
            }

            return ret;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var client = _clientFactory.CreateClient("HRApiClient");
            var response = await client.DeleteAsync($"api/departments/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                string strResponse = await response.Content.ReadAsStringAsync();
                LogErrorHelper.LogError(strResponse, null, LogErrorHelper.ErrorLevel.Warn);
                return false;
            }
        }
    }
}
