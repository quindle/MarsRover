using MarsRoverApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverApi.MongoDB
{
    public interface IMarsRoverContext
    {
        IMongoCollection<Planet> Planets { get; }

        public IMongoCollection<Rover> Rovers { get; }
    }
}
