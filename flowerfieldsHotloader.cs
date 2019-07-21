using System;
using XRL.Rules;
using Qud.API;
using XRL.Rules;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_FlowerHotloader : IPart
	{
		public int ChanceOneIn = 100;

		public acegiak_FlowerHotloader()
		{
			
		}

		public override bool SameAs(IPart p)
		{
			return false;
		}

		public override void Register(GameObject Object)
		{
			Object.RegisterPartEvent(this, "EnteredCell");
			base.Register(Object);
		}

        public void AddAFlower(){
            string flowertype = EncountersAPI.GetARandomDescendentOf("FlowerBush");
            if(ParentObject.CurrentCell != null){
                foreach(Cell c in ParentObject.CurrentCell.GetEmptyAdjacentCells(0,3)){
					if(Stat.Rnd2.NextDouble()<0.05f){
						c.AddObject(GameObjectFactory.Factory.CreateObject(flowertype));
					}
				}
                //ParentObject.CurrentCell.AddObject(GO);
            }
        }

		public override bool FireEvent(Event E)
		{
			if (E.ID == "EnteredCell" || E.ID == "EnterCell"|| E.ID == "ObjectCreated")
			{
				if (Stat.Random(1, ChanceOneIn) == 1)
				{
					AddAFlower();
				}
				return true;
			}
			return base.FireEvent(E);
		}
	}
}
