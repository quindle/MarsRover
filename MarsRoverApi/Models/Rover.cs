using MarsRoverApi.Exception;
using MarsRoverApi.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarsRoverApi.Models
{

    /// <summary>
    /// Oggetto che rappresenta un Rover
    /// </summary>
    public class Rover : Item
    {
        /// <summary>
        /// Identificativo interno  di MongoDB
        /// </summary>
        [BsonId]
        [JsonIgnore]
        public ObjectId InternalId { get; set; }

        /// <summary>
        /// Pianeta sul quale è presente il Rover
        /// </summary>
        [JsonIgnore]
        public Planet Planet { get; set; }

        /// <summary>
        /// Nome del pianeta
        /// </summary>
        [JsonPropertyName("planetName")]
        public string PlanetName { get { return Planet?.Name; } }

        /// <summary>
        /// Orientamento del Rover (N,S,E,W)
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CardinalPoint Orientation { get; set; }

        /// <summary>
        /// Indica se il Rover è atterrato su un pianeta
        /// </summary>
        [JsonIgnore]
        public bool IsLanded { get { return Planet != null; } }

        /// <summary>
        /// Ruota di novanta gradi a destra
        /// </summary>
        public void Right() {
            Orientation = (CardinalPoint)(((int)Orientation + 1) % 4);        
        }

        /// <summary>
        /// Ruota di 90 gradi asinistra
        /// </summary>
        public void Left()
        {
            if (Orientation == CardinalPoint.N)
                Orientation = CardinalPoint.W;
            else
                Orientation = (CardinalPoint)((int)Orientation - 1);
        }

        /// <summary>
        /// Muove in avanti di una posizione
        /// </summary>
        public void Forward() {
            Position = Move(1);
        }

        /// <summary>
        /// Muove indietro di una posizione
        /// </summary>
        public void Backward()
        {
            Position = Move(-1);
        }

        /// <summary>
        /// Atterra su un pianeta
        /// </summary>
        /// <param name="planetToLand"></param>
        public void Land(Planet planetToLand)
        {

            if (this.Planet != null)
            {
                throw new NotSupportedException($"Il Rover è già atterrato sul pianeta {this.Planet.Name}");
            }

            if (planetToLand == null)
            {
                throw new ArgumentNullException(nameof(planetToLand), "Impossibile atterrare su un pianeta inesistente");
            }

            this.Planet = planetToLand;
            Location landPosition = null;
            do {
                landPosition = this.Planet.GetRandomLocation();
            } while (planetToLand.HasObstacleAt(landPosition));

            this.Position = landPosition;
           this.Planet.Obstacles.Add(this);

        }

        private Location Move(short stepNumber)
        {
            Location newPosition = new Location() { Column = Position.Column, Row = Position.Row };

            switch (Orientation)
            {
                case CardinalPoint.N:
                    newPosition.Row+= stepNumber;
                    break;
                case CardinalPoint.S:
                    newPosition.Row-= stepNumber;
                    break;
                case CardinalPoint.E:
                    newPosition.Column+= stepNumber;
                    break;
                case CardinalPoint.W:
                    newPosition.Column-= stepNumber;
                    break;
            }

            newPosition.Row = newPosition.Row % Planet.Rows;

            if (newPosition.Row < 0) newPosition.Row = Planet.Rows + newPosition.Row;

            newPosition.Column = newPosition.Column % Planet.Columns;

            if (newPosition.Column < 0) newPosition.Column = Planet.Columns + newPosition.Column;

            if (Planet.HasObstacleAt(newPosition))
            {
                throw new ObstacleException(Planet.Obstacles.Where(obs=> obs.Position.Row == newPosition.Row && obs.Position.Column == newPosition.Column).Single(), "Ostacolo Rilevato. Impossibile muovere nella posizione desiderata");
            }

            return newPosition;
        }
    }

}
