using Project.HR.Domain.Models;

namespace Project.HR.Domain.Interfaces
{
    public interface IPositionService
    {
        Task<List<Position>> GetAllPositionsAsync();
        Task<bool> IsPositionAvailableAsync(string positionName);
        Task<Position> CreatePositionAsync(Position position);
        Task<Position?> GetPositionByNameAsync(string positionName);
        Task<bool> DeletePositionAsync(string positionName);
        Task<Position?> UpdatePositionAsync(string positionName, Position position);


    }
}
