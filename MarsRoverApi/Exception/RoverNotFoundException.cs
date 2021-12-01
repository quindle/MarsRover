using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverApi.Exception
{
    public class RoverNotFoundException: System.Exception
    {

        public string Name { get; internal set; }
        public RoverNotFoundException(string name):base()
        {
            Name = name;
        }

    }
}
