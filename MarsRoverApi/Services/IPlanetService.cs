using MarsRoverApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverApi.Services
{
    public interface IPlanetService
    {       
        Task<IEnumerable<Planet>> GetAllPlanets();

        Task<Planet> GetPlanetByName(string name);

        Task Create(Planet planet);

        Task<bool> Destroy(string name);               

    }
}
