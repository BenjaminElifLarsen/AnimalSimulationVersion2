using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface ISound //perhaps have a modifier for how good they hear (and maybe also a ISmell)
    {
        public void GenerateSound();

        public void HearSound();
    }
}
