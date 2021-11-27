using MarsRoverApi.Models;
using MarsRoverApi.MongoDB;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverApi.Services
{
    public class RoverService : IRoverService
    {

        private readonly IMarsRoverContext _context;

        public RoverService(IMarsRoverContext context)
        {
            _context = context;
        }

        public async Task<bool> Retrieve(string name)
        {

            FilterDefinition<Rover> filter = Builders<Rover>.Filter.Eq(p => p.Name, name);
            DeleteResult deleteResult = await _context
                                                .Rovers
                                              .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Rover>> GetAllRovers()
        {
            return await _context
                          .Rovers
                          .Find(_ => true)
                          .ToListAsync();
        }

        public Task<Rover> GetRoverByName(string name)
        {
            FilterDefinition<Rover> filter = Builders<Rover>.Filter.Eq(p => p.Name, name);
            return _context
                    .Rovers
                    .Find(filter)
                    .FirstOrDefaultAsync();
        }

        public async Task Launch(Rover rover)
        {
            await _context.Rovers.InsertOneAsync(rover);
        }
    }
}
