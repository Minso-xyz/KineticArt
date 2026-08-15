using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KineticArt.Models
{
    internal class Actuator
    {
        public ActuatorType.ShapeType Type { get; set; }
        //public double Angle { get; set; }
        public double AngularSpeed { get; set; }

        public RotationDirection Direction { get; set; }

        //public Point Pivot { get; set; }
        public double PivotX { get; set; }
        public double PivotY { get; set; }

        public double InitialAngle { get; set; }    
        public double RotatedAngle {  get; set; }
        public double DisplayAngle {  get; set; }   
    }


}
