using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverApi.Models
{
    public class Rover : Item
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public string Name { get; set; }

        public CardinalPoint Orientation { get; set; }
         
    }
}
