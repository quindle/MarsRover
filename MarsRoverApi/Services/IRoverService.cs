using MarsRoverApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverApi.Services
{
    public interface IRoverService
    {
        Task<IEnumerable<Rover>> GetAllRovers();

        Task<Rover> GetRoverByName(string name);

        Task Launch(Rover rover);

        Task<bool> Retrieve(string name);
    }
}
