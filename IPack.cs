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
        public abstract bool CanFightForAlpha { get; set; } //how to ensure the animals do not fight all the time and also figure out why one animal would fight another for alpha
        public abstract void FightAlpha(string alphaID); //delegate
        public abstract void FightChallenger(string challengerID); //delegate
    }
}
