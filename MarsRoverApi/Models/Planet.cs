using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MarsRoverApi.Models
{
    public class Planet
    {
        [BsonId]
        public ObjectId InternalId { get; set; }

        public int Rows { get; set; } = 10;
        public int Columns { get; set; } = 10;

        public string Name { get; set; }

        public IEnumerable<Item> Obstacles;

    }
}
