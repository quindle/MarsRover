using MarsRoverApi.Models;
using MarsRoverApi.MongoDB.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace MarsRoverApi.MongoDB
{
    public class MarsRoverContext : IMarsRoverContext
    {
        private readonly IMongoDatabase _db;
        public MarsRoverContext(MongoDBConfig config)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(
         new MongoUrl(config.ConnectionString)
       );


            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            var client = new MongoClient(settings);            
            _db = client.GetDatabase(config.Database);            
        }
        public IMongoCollection<Planet> Planets => _db.GetCollection<Planet>("planets");
        public IMongoCollection<Rover> Rovers => _db.GetCollection<Rover>("rovers");
    }
}
