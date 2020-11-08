using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface IPack //: IFamily
    {
        public abstract string[] Alphas { get; set; }
        public abstract string[] PackMembers { get; set; }
        public abstract byte PackSize { get; set; }
        public abstract byte MaxPackSize { get; set; }
        public abstract bool CanFightForAlpha { get; set; }
        public abstract void FightAlpha(string alphaID); //delegate
        public abstract void FightChallenger(string challengerID); //delegate
    }
}
