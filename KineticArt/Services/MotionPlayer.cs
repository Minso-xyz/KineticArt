using KineticArt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KineticArt.Services
{
    internal class MotionPlayer
    {
        public Motion Motion;

        private int currentStep = 0;

        private DateTime stepStart;

        public void Play(Motion motion)
        {
            Motion = motion;

            currentStep = 0;

            stepStart = DateTime.Now;
        }

        public void Update(Actuator[] actuators)
        {
            if (Motion == null)
                return;

            MotionStep step = Motion.Steps[currentStep];

            for (int i = 0; i < 4; i++)
            {
                actuators[i].AngularSpeed = step.LineSpeed[i];
                actuators[i].Direction = step.LineDirection[i];

                actuators[i + 4].AngularSpeed = step.SquareSpeed[i];
                actuators[i + 4].Direction = step.SquareDirection[i];
            }

            if ((DateTime.Now - stepStart).TotalMilliseconds >= step.DurationMs)
            {
                currentStep++;

                if (currentStep >= Motion.Steps.Count)
                {
                    if (Motion.Loop)
                        currentStep = 0;
                    else
                        currentStep = Motion.Steps.Count - 1;
                }

                stepStart = DateTime.Now;
            }

            // For the actual rotation, the logic in Update() has been moved here 
            foreach (var a in actuators)
            {
                a.RotatedAngle += a.AngularSpeed * (int)a.Direction;
            }
        }
    }

 


}
