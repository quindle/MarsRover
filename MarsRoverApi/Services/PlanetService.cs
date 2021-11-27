using MarsRoverApi.Models;
using MarsRoverApi.MongoDB;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverApi.Services
{
    public class PlanetService : IPlanetService
    {

        private readonly IMarsRoverContext _context;

        public PlanetService(IMarsRoverContext context) {
            _context = context;
        }

        public async Task Create(Planet planet)
        {
            await _context.Planets.InsertOneAsync(planet);
        }

        public async Task<bool> Destroy(string name)
        {
            FilterDefinition<Planet> filter = Builders<Planet>.Filter.Eq(p => p.Name, name);
            DeleteResult deleteResult = await _context
                                                .Planets
                                              .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Planet>> GetAllPlanets()
        {
            return await _context
                           .Planets
                           .Find(_ => true)
                           .ToListAsync();
        }

        public Task<Planet> GetPlanetByName(string name)
        {
            FilterDefinition<Planet> filter = Builders<Planet>.Filter.Eq(p => p.Name, name);
            return _context
                    .Planets
                    .Find(filter)
                    .FirstOrDefaultAsync();
        }
    }
}
