using MarsRoverApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverApi.Exception
{
    public class ObstacleException : System.Exception
    {
        public ObstacleException(Item obstacle, string message) : base(message)
        {
            this.Obstacle = obstacle;
        }

        public Item Obstacle { get; internal set; }
    }
}
