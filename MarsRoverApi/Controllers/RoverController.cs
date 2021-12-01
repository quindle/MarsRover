using MarsRoverApi.Exception;
using MarsRoverApi.Models;
using MarsRoverApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverApi.Controllers
{
    /// <summary>
    /// API lanciare, controllare e recuperare i rover
    /// </summary>
    [Route("api/[Controller]")]
    public class RoverController
    {

        private readonly IRoverService _service;
        private readonly IPlanetService _planetService;

        /// <summary>
        /// Costruttore per RoverController
        /// </summary>
        /// <param name="service">roverService</param>
        /// <param name="planetService">planetService</param>
        public RoverController(IRoverService service, IPlanetService planetService) {
            _service = service;
            _planetService = planetService;

        }

        // GET api/rovers
        /// <summary>
        /// Mostra la lista di tutti i Rover
        /// </summary>
        /// <returns>Lista dei Rover esistenti</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rover>>> Get()
        {
            return new ObjectResult(await _service.GetAllRovers());
        }


        // GET api/rovers/:name
        /// <summary>
        /// Mostra i dati di uno specifico Rover
        /// </summary>
        /// <param name="name">nome del rover da cercare</param>
        /// <returns>Rover</returns>
        [HttpGet("{name}")]
        public async Task<ActionResult<Rover>> Get([FromRoute] string name)
        {
            var rover = await _service.GetRoverByName(name);
            if (rover == null)
                return new NotFoundResult();

            return new ObjectResult(rover);
        }


        // POST api/rovers
        /// <summary>
        /// Lancia un nuovo Rover
        /// </summary>
        /// <param name="rover">Rover da lanciare</param>
        /// <returns>il nuovo rover lanciato</returns>
        //[HttpPost]
        //public async Task<ActionResult<Rover>> Post([FromBody] Rover rover)
        //{            
        //    await _service.Launch(rover);
        //    return new OkObjectResult(rover);
        //}

        // PUT api/rovers/:name
        /// <summary>
        /// Muove un rover sulla superficie del pianeta seguendo i comandi dati in input
        /// </summary>
        /// <param name="name">Nome del rover</param>
        /// <param name="commands">stringa di comandi concatenati da eseguire nell'alfabeto {l = left; r= right; f=forward; b= backward }</param>
        /// <returns></returns>
        [HttpPut("{name}")]
        public async Task<ActionResult<Rover>> Move([FromRoute] string name, [FromBody] string commands)
        {
            Rover r = await _service.GetRoverByName(name);

            if (r == null)
                return new NotFoundObjectResult(name);

            string obstacleMessage = string.Empty;

            try
            {
               await  _planetService.RemoveObstacle(r.Planet, name);

                await _service.Move(r, commands);              

            }
            catch (ObstacleException obsEx)
            {
                obstacleMessage = $"E' stato rilevato l'ostacolo di nome {obsEx.Obstacle.Name} in posizione Column: {obsEx.Obstacle.Position.Column }, Row: {obsEx.Obstacle.Position.Row}. Il Rover si è fermato in posizione Column: {r.Position.Column} Row: {r.Position.Row}";
            }
            finally {
                await _planetService.AddObstacle(r.Planet, r);
            }

            if (!string.IsNullOrEmpty(obstacleMessage)) {
                return new ConflictObjectResult(obstacleMessage);
            }

            return new OkObjectResult(r);

        }


        // POST api/rovers
        /// <summary>
        /// Fai "atterrare" un Rover su un pianeta
        /// </summary>
        /// <param name="roverName">Nome del Rover</param>
        /// <param name="planetName">Pianeta sul quale atterrare</param>
        /// <param name="position">Posizione sul quale atterrare</param>
        /// <returns>Rover</returns>
        [HttpPost("{roverName}/{planetName}")]
        public async Task<ActionResult<Rover>> Post([FromRoute] string roverName,[FromRoute] string planetName, [FromBody]Location position)
        {
            if (planetName == null) {
                return new BadRequestObjectResult("il parametro planet non può essere nullo");
            }

            Rover r = await _service.GetRoverByName(roverName);

            if (r == null)
            {
                r = new Rover() { Name = roverName };
                await _service.Launch(r);
            }

            Planet p = await _planetService.GetPlanetByName(planetName);

            if (p == null)
                throw new ArgumentException($"Nessun pianeta trovato con nome {planetName}");
                
                     
            if (r != null) {
                if (await _service.Land(r, p,position))
                    await _planetService.AddObstacle(p, r);
            }
            
            return new OkObjectResult(r);
        }

        // DELETE api/rovers/:name
        /// <summary>
        /// Fa rientrare un rover
        /// </summary>
        /// <param name="name">fa rientrare un rover ovunque esso sia</param>
        /// <returns></returns>
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var rover = await _service.GetRoverByName(name);
            if (rover == null)
                return new NotFoundResult();
            if (await _service.Retrieve(name) && !string.IsNullOrEmpty(rover.PlanetName))
                await _planetService.RemoveObstacle(rover.Planet, name);
            return new OkResult();
        }
    }
}
