using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface IPack //: IFamily
    {
        public enum PackRelationship
        {
            Alpha = 1,
            Member = 2,
            NonMember = 0
        }
        public abstract (PackRelationship Relationship, string ID)[] PackMembers { get; set; }
        public abstract byte PackSize { get; set; }
        public abstract byte MaxPackSize { get; set; }
        public abstract float TimeBetweenFights { get; set; }
        public abstract float FightCooldown { get; set; }
        public abstract bool CanFightForAlpha { get; set; } //how to ensure the animals do not fight all the time and also figure out why one animal would fight another for alpha
        public abstract void FightAlpha(string alphaID); //transmit delegate. Have an event for doing health damage
        public abstract void FightChallenger(string challengerID); //transmit delegate
        
    }
}
