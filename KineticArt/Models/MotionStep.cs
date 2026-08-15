using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KineticArt.Models
{
    internal class MotionStep
    {
        public int DurationMs {  get; set; }
        public double[] LineSpeed { get; set; }
        public double[] SquareSpeed { get; set; }

        public RotationDirection[] LineDirection { get; set; }
        public RotationDirection[] SquareDirection { get; set; }
        //public enum RotationDirection
        //{
        //    Clockwise = 1,
        //    CounterClockwise = -1
        //}


    }
}
