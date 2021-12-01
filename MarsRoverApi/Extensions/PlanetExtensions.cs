using MarsRoverApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverApi.Extensions
{
    public static class PlanetExtensions
    {

        public static Location GetRandomLocation(this Planet p) { 
            return new Location() { Row = new Random().Next(0, p.Rows - 1), Column = new Random().Next(0,p.Columns - 1) };
        }

        public static bool HasObstacleAt(this Planet p, Location l)
        {
            return p.Obstacles.Any(i => i.Position.Row == l.Row && i.Position.Column == l.Column);
        }

        public static bool ExistLocation(this Planet p, Location l) {
            return l.Column >= 0 && l.Column < p.Columns && l.Row >= 0 && l.Row < p.Rows;
        }



    }
}
