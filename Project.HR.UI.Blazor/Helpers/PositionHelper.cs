using Microsoft.Extensions.Diagnostics.HealthChecks;
using Project.HR.Domain.DTOs;
using Project.HR.Domain.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Project.HR.UI.Blazor.Helpers
{
    public class PositionHelper
    {
        private readonly IHttpClientFactory _clientFactory;
        public PositionHelper(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<List<PostionDTO>> GetAllPositions()
        {
            List<PostionDTO> positions = new List<PostionDTO>();
            var client = _clientFactory.CreateClient("HRApiClient");
            positions = await client.GetFromJsonAsync<List<PostionDTO>>("api/positions");
            


            return positions;
        }

        public async Task<PostionDTO> CreatePositionAsync(PostionDTO position)
        {
            string strPos= JsonSerializer.Serialize(position);

            PostionDTO ret = new PostionDTO();
            var client = _clientFactory.CreateClient("HRApiClient");
            if (position != null)
            {
                var response = await client.PostAsJsonAsync("api/positions", position);
                response.EnsureSuccessStatusCode();
                string strResponse = await response.Content.ReadAsStringAsync();
                ret = JsonSerializer.Deserialize<PostionDTO>(strResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }) ?? new PostionDTO();
            }
            return ret;
        }

        public async Task<PostionDTO> UpdatePositionAsync(PostionDTO position)
        {
            PostionDTO ret = new PostionDTO();
            var client = _clientFactory.CreateClient("HRApiClient");
            if (position != null && position.Id.HasValue)
            {
                var response = await client.PutAsJsonAsync($"api/positions/{position.Id}", position);
                response.EnsureSuccessStatusCode();
                string strResponse = await response.Content.ReadAsStringAsync();
                ret = JsonSerializer.Deserialize<PostionDTO>(strResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                }) ?? new PostionDTO();
            }
            return ret;
        }

        public async Task<bool> DeletePositionAsync(int positionId)
        {
            var client = _clientFactory.CreateClient("HRApiClient");
            var response = await client.DeleteAsync($"api/positions/{positionId}");
            return response.IsSuccessStatusCode;
        }
    }
}
