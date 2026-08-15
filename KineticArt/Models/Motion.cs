using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KineticArt.Models
{
    internal class Motion
    {
        public List<MotionStep> Steps { get; set; }
        = new List<MotionStep>();

        public bool Loop { get; set; }


    }
}
