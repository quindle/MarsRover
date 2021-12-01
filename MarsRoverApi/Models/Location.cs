using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverApi.Models
{

    /// <summary>
    /// Posizione sulla superficie di un pianeta
    /// </summary>
    public class Location
    {
        /// <summary>
        /// coordinate Y
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Coordinata X
        /// </summary>
        public int Column { get; set; }
    }
}
