using MarsRoverApi.Exception;
using MarsRoverApi.Extensions;
using MarsRoverApi.Models;
using MarsRoverApi.MongoDB;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarsRoverApi.Services
{
    public class RoverService : IRoverService
    {

        private readonly IMarsRoverContext _context;

        /// <summary>
        /// Costruttore RoverService
        /// </summary>
        /// <param name="context"></param>
        public RoverService(IMarsRoverContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Rimuove il rover ovunque esso sia
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> Retrieve(string name)
        {
            Rover r = await GetRoverByName(name);

            if (r == null)
                throw new RoverNotFoundException(name);


            if (r.Planet != null)
                r.Planet.Obstacles.Remove(r);            

            FilterDefinition<Rover> filter = Builders<Rover>.Filter.Eq(r => r.Name, name);
                DeleteResult deleteResult = await _context
                                                        .Rovers
                                                      .DeleteOneAsync(filter);

            return (deleteResult.IsAcknowledged
             && deleteResult.DeletedCount > 0);        


        }


        /// <summary>
        /// Restituisce l'elenco dei Rover
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Rover>> GetAllRovers()
        {
            return await _context
                          .Rovers
                          .Find(_ => true)
                          .ToListAsync();
        }

        /// <summary>
        /// Ricerca un Rover dal suo nome
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<Rover> GetRoverByName(string name)
        {
            FilterDefinition<Rover> filter = Builders<Rover>.Filter.Eq(p => p.Name, name);
            return _context
                    .Rovers
                    .Find(filter)
                    .FirstOrDefaultAsync();
        }


        /// <summary>
        /// Crea un nuovo Rover
        /// </summary>
        /// <param name="rover"></param>
        /// <returns></returns>
        public async Task Launch(Rover rover)
        {
            await _context.Rovers.InsertOneAsync(rover);
        }

        /// <summary>
        /// Fa atterrare un rover su un Pianeta
        /// </summary>
        /// <param name="rover">Rover che deve atterrare</param>
        /// <param name="planetToLand">Pianeta di destinazion</param>
        /// <param name="position">Posizione di atterraggio</param>
        /// <returns></returns>
        public async Task<bool> Land(Rover rover, Planet planetToLand,Location? position = null)
        {
            if (rover == null)
                throw new ArgumentNullException(nameof(rover));

            //rover.Land(planet);
            if (rover.Planet != null)
            {
                throw new NotSupportedException($"Il Rover è già atterrato sul pianeta {rover.Planet.Name}");
            }

            if (planetToLand == null)
            {
                throw new ArgumentNullException(nameof(planetToLand), "Impossibile atterrare su un pianeta inesistente");
            }



            Location landPosition = null;

            if (position != null)
            {
                if (!planetToLand.ExistLocation(position))
                {
                    throw new ArgumentNullException(nameof(planetToLand), "Impossibile atterrare: la posizione indicata non esiste");
                }

                if (planetToLand.HasObstacleAt(position))
                {
                    throw new ArgumentNullException(nameof(planetToLand), "Impossibile atterrare: Nella posizione indicata c'è un ostacolo");
                }
                landPosition = position;
            }
            else {
                do
                {
                    landPosition = rover.Planet.GetRandomLocation();
                } while (planetToLand.HasObstacleAt(landPosition));

            }

            rover.Planet = planetToLand;                                              
            
           

            rover.Position = landPosition;

            return await UpdatePosition(rover,rover.Planet, rover.Position, rover.Orientation);
                        
       
        }

        /// <summary>
        /// Muovi il rover secondo i comandi ricevuti
        /// </summary>
        /// <param name="rover">rover da muovere</param>
        /// <param name="complexCommand">comandi da eseguire</param>
        public async Task Move(Rover rover, string complexCommand) {

            if (rover == null)
                throw new ArgumentNullException(nameof(rover));

            Regex regEx = new Regex(@"^[lrfb]+$");

            if (!regEx.IsMatch(complexCommand)) {
                throw new ArgumentException("Comando non valido, uno o più azioni non sono valide. Le azioni valide sono: [lrfg]", nameof(complexCommand));
            } 
            
           
            foreach (char atomicCommand in complexCommand) {
              
                  await  executeCommand(rover, atomicCommand);              
            }             

            }
       

        private async Task executeCommand(Rover rover, char atomicCommand)
        {
            switch (atomicCommand) {
                case 'l':
                    rover.Left();
                    break;
                case 'r':
                    rover.Right();
                    break;
                case 'f':
                    rover.Forward();
                    break;
                case 'b':
                    rover.Backward();
                    break;
                default:
                    throw new InvalidOperationException($"'{atomicCommand}' non è un comando valido per il Rover");
            }

            await UpdatePosition(rover,rover.Planet, rover.Position, rover.Orientation);


        }


        private async Task<bool> UpdatePosition(Rover rover, Planet planet, Location position, CardinalPoint orientation)
        {

            FilterDefinition<Rover> filter = Builders<Rover>.Filter.Eq(r => r.Name, rover.Name);
            UpdateDefinition<Rover> update = Builders<Rover>.Update.Set("Planet",planet).Set("Position", position).Set("Orientation", orientation);

            UpdateResult updateRoverResult = await _context
                                        .Rovers
                                        .UpdateOneAsync(filter, update);

            return updateRoverResult.IsAcknowledged && updateRoverResult.ModifiedCount > 0;
        }


    }

}
