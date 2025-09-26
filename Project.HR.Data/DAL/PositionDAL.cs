using Microsoft.EntityFrameworkCore;
using Project.HR.Data.Models;
using Project.HR.Domain.Interfaces;
using Project.HR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.HR.Data.DAL
{
    public class PositionDAL : IPositionService
    {
        private readonly HRDbContext _context;

        public PositionDAL(HRDbContext context)
        {
            _context = context;
        }

        public async Task<Position> CreatePositionAsync(Position position)
        {
            await _context
               .Positions
               .AddAsync(position);
            await _context
                .SaveChangesAsync();

            return position;
        }

        public async Task<bool> DeletePositionAsync(int id)
        {
            await _context.Positions
                 .Where(p => p.Id == id)
                 .ExecuteDeleteAsync();
            return true;
        }

        public async Task<List<Position>> GetAllPositionsAsync()
        {
            return await _context.Positions
                 .AsNoTracking()
                 .ToListAsync();
        }

        public async Task<Position?> GetPositionByNameAsync(string positionName)
        {
            return await _context.Positions
                 .AsNoTracking()
                 .FirstOrDefaultAsync(p => p.Title == positionName);
        }

        public Task<bool> IsPositionAvailableAsync(string positionName)
        {
            throw new NotImplementedException();
        }

        public async Task<Position?> UpdatePositionAsync(int id, Position position)
        {
            await _context.Positions
                 .Where(p => p.Id == id)
                 .ExecuteUpdateAsync(p => p
                     .SetProperty(p => p.Title, position.Title)
                     .SetProperty(p => p.Description, position.Description)
                 );
            return position;
        }


    }
}
