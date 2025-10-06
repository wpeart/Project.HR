using Project.HR.Domain.DTOs;
using Project.HR.Domain.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Project.HR.UI.Blazor.Helpers
{
    public class RolesHelper
    {
        private readonly IHttpClientFactory _clientFactory;

        public RolesHelper(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<UserRolesDTO>> GetAllRoles()
        {
            List<UserRolesDTO> rolesDto = new List<UserRolesDTO>();
            List<UserRoles> roles = new();

            var client = _clientFactory.CreateClient("HRApiClient");

            roles = await client.GetFromJsonAsync<List<UserRoles>>("api/roles");

            foreach (var role in roles)
            {
                UserRolesDTO dto = new UserRolesDTO
                {
                    RoleName = role.RoleName,
                    RoleDescription = role.RoleDescription,
                    RoleId = role.RoleId
                };
                rolesDto.Add(dto);
            }

            return rolesDto;
        }

        public async Task<UserRolesDTO> CreateRoleAsync(UserRolesDTO role)
        {
            UserRolesDTO ret = new UserRolesDTO();
            var client = _clientFactory.CreateClient("HRApiClient");
            if (role != null)
            {
                var response = await client.PostAsJsonAsync("api/roles", role);
                response.EnsureSuccessStatusCode();
                string strResponse = await response.Content.ReadAsStringAsync();

                UserRoles roles = new UserRoles();


                roles = JsonSerializer.Deserialize<UserRoles>(strResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                ret = new UserRolesDTO
                {
                    RoleId = roles.RoleId,
                    RoleName = roles.RoleName,
                    RoleDescription = roles.RoleDescription
                };

            }
            return ret;
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            bool ret = false;
            var client = _clientFactory.CreateClient("HRApiClient");
            if (roleId > 0)
            {
                var response = await client.DeleteAsync($"api/roles/{roleId}");
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    ret = true;
                }
            }
            return ret;
        }

        public async Task<UserRolesDTO> UpdateRoleAsync(int roleId, UserRolesDTO role)
        {
            UserRolesDTO ret = new UserRolesDTO();
            var client = _clientFactory.CreateClient("HRApiClient");

            var response = await client.PutAsJsonAsync($"api/roles/{roleId}", role);
                response.EnsureSuccessStatusCode();
            string strResponse = await response.Content.ReadAsStringAsync();
            UserRoles roles = new UserRoles();

            roles = JsonSerializer.Deserialize<UserRoles>(strResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            ret = new UserRolesDTO
            {
                RoleId = roles.RoleId,
                RoleName = roles.RoleName,
                RoleDescription = roles.RoleDescription
            };

            return ret;
        }
    }
}
