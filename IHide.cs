using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface IHide //if it is hunted, for each predator generate a random number and if it is higher than a certian value that predator will "lose" the prey. The prey's movement speed is set to zero.
    { //maybe have a IFlee
        public int StealthLevel { get; set; } 
        public float TimeHidden { get; set; }
        public float MaxHideTime { get; set; }
        public void HideFromPredator(); //maybe old/young animals got a harder time to hide.

    }
}
