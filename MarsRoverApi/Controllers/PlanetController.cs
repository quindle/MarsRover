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
    /// Controller dei Pianeti
    /// </summary>
    [Route("api/Planet")]
    public class PlanetController : ControllerBase
    {

        private readonly IPlanetService _service;

        /// <summary>
        /// Costruttore per il controllore dei pianeti
        /// </summary>
        /// <param name="service">implementazione di IPlanetService</param>
        public PlanetController(IPlanetService service) {
            _service = service;
        }

        // GET api/planets
        /// <summary>
        /// Mostra la lista di tutti i pianeti
        /// </summary>
        /// <returns>Lista dei pianeti presenti</returns>
        /// <remarks>
        /// Sample request:
        ///    GET api/planets/Mars
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Planet>>> Get()
        {
            return new ObjectResult(await _service.GetAllPlanets());
        }


        // GET api/planets/:name
        /// <summary>
        /// Mostra i dati di uno specifico pianeta
        /// </summary>
        /// <param name="name">Nome del pianeta</param>
        /// <returns>Pianeta</returns>
        ///  <remarks>
        /// Sample request:
        ///    GET api/planets/Mars
        /// </remarks>
        [HttpGet("{name}")]
        public async Task<ActionResult<Planet>> Get(string name)
        {
            var planet = await _service.GetPlanetByName(name);
            if (planet == null)
                return new NotFoundResult();

            return new ObjectResult(planet);
        }

        // PUT api/planets/:name
        /// <summary>
        /// Aggiuni un ostacolo al pianeta
        /// </summary>
        /// <param name="name">Nome del pianeta</param>
        /// <returns>Pianeta</returns>
        [HttpPut("{name}")]
        public async Task<ActionResult<Planet>> Put([FromRoute]string name,[FromBody] Location position)
        {
            var planet = await _service.GetPlanetByName(name);
            if (planet == null)
                return new NotFoundResult();

            planet = await _service.AddObstacle(planet, new Item() { Position = position });


            return new ObjectResult(planet);
        }



        // POST api/planets
        /// <summary>
        /// Crea un nuovo pianeta
        /// </summary>
        /// <param name="planet"></param>
        /// <returns>Il pianeta Creato</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /planets
        ///     {
        ///        "row": 10,
        ///        "columns": 10,
        ///        "name": "Mars"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<Planet>> Post([FromBody] Planet planet)
        {            
            await _service.Create(planet);
            return new OkObjectResult(planet);
        }

     
        /// <summary>
        /// Distrugge un pianeta
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/planets/Mars
        /// </remarks>
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var post = await _service.GetPlanetByName(name);
            if (post == null)
                return new NotFoundResult();
            await _service.Destroy(name);
            return new OkResult();
        }
    }
}
