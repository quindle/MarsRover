using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MarsRoverApi.Models
{
    /// <summary>
    /// Oggetto che rappresenta un pianeta
    /// </summary>
    public class Planet
    {
        /// <summary>
        /// Internal ID di MongoDB
        /// </summary>
        [BsonId]
        [JsonIgnore]
        public ObjectId InternalId { get; set; }

        /// <summary>
        /// Numero di righe del pianeta
        /// </summary>
        public int Rows { get; set; } = 10;

        /// <summary>
        /// Numero di colonne del pianeta
        /// </summary>
        public int Columns { get; set; } = 10;

        /// <summary>
        /// Nome del pianeta
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ostacoli sul pianeta
        /// </summary>
        public IList<Item> Obstacles { get; internal set; } = new List<Item>();

    }
}
