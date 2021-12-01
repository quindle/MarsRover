using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverApi.Models
{
    /// <summary>
    /// Rappresenta qualunque oggetto con una posizione
    /// </summary>
    public class Item
    {

        /// <summary>
        /// Indica la posizione sul pianeta
        /// </summary>
       public Location Position { get; set; }

        public string Name { get; set; } = Guid.NewGuid().ToString();
    }
}
