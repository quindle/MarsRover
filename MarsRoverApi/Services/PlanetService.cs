using MarsRoverApi.Extensions;
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

        public async Task<Planet> AddObstacle(Planet planet,Item obstacle)
        {

            if (!planet.ExistLocation(obstacle.Position))
                throw new ArgumentOutOfRangeException("L'ostacolo non può essere messo nella posizione indicata. La posizione non esiste sul pianeta");
                
            planet.Obstacles.Add(new Item() { Position = obstacle.Position, Name = obstacle.Name });
                FilterDefinition<Planet> filterPlanet = Builders<Planet>.Filter.Eq(p => p.Name, planet.Name);
                UpdateDefinition<Planet> updatePlanet = Builders<Planet>.Update.Set("Obstacles", planet.Obstacles);

           UpdateResult update = await _context.Planets.UpdateOneAsync(filterPlanet, updatePlanet);

            if (update.IsAcknowledged && update.ModifiedCount > 0)
                return planet;


            throw new System.Exception("Si sono verificati dei problemi durante l'aggiunta di un ostacolo");
            
        }

        public async Task<bool> RemoveObstacle(Planet planet,string obstacleName) {

            Item obs = planet.Obstacles.SingleOrDefault(i => i.Name == obstacleName);

            if (obs != null) {
                planet.Obstacles.Remove(obs);
            }


            FilterDefinition<Planet> filterPlanet = Builders<Planet>.Filter.Eq(p => p.Name, planet.Name);
            UpdateDefinition<Planet> updatePlanet = Builders<Planet>.Update.Set("Obstacles", planet.Obstacles);

            UpdateResult update = await _context.Planets.UpdateOneAsync(filterPlanet, updatePlanet);

            return update.IsAcknowledged && update.ModifiedCount > 0;
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
