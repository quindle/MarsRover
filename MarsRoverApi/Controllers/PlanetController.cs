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
       
    [Route("api/Planet")]
    public class PlanetController : ControllerBase
    {

        private readonly IPlanetService _service;

        public PlanetController(IPlanetService service) {
            _service = service;
        }

        // GET api/planets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Planet>>> Get()
        {
            return new ObjectResult(await _service.GetAllPlanets());
        }


        // GET api/planets/:name
        [HttpGet("{name}")]
        public async Task<ActionResult<Planet>> Get(string name)
        {
            var planet = await _service.GetPlanetByName(name);
            if (planet == null)
                return new NotFoundResult();

            return new ObjectResult(planet);
        }


        // POST api/planets
        [HttpPost]
        public async Task<ActionResult<Planet>> Post([FromBody] Planet planet)
        {            
            await _service.Create(planet);
            return new OkObjectResult(planet);
        }

        // DELETE api/planets/:name
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
