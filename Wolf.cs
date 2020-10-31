using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Wolf : Carnivore, ISleep, ITerritorial
    {
        public Wolf(string species, int reproductionAge, float[] location, float maxAge, int[] birthAmount, float movementSpeed, float hunger, Point[] design, int[] colour, string[] foodSource, string active, float nutrienceValue, IArraySupport helper) : base(species, reproductionAge, location, maxAge, birthAmount, movementSpeed, hunger, design, colour, foodSource, active, nutrienceValue, helper)
        {
            //Wolf wolf = new Wolf(null, 1, null, 2, null, 3, 4, null, null, null, "5", 1, Helper.Instance);
            //helper.DeepCopy(new int[] { 5 });
        }

        public string[] Targets { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AttackOtherEventHandler(string ID)
        {
            throw new NotImplementedException();
        }

        public override void Control()
        {
            base.Control();
        }

        public void FindTargetEventHandler()
        {
            throw new NotImplementedException();
        }

        public void IsAttackedEventHandler()
        {
            throw new NotImplementedException();
        }
    }
}
