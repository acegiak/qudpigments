using System;
using System.Text;
using XRL.Rules;
using XRL.World.Parts;

namespace XRL.World.Effects
{
	[Serializable]
	public class acegiak_PaintEffectCooking : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectCooking():base()
		{

		}

		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nYou heal small amounts of hitpoints from eating.";
		}



	    public override void Register(GameObject Object)
		{
			Object.RegisterEffectEvent(this, "Eating");
			Object.RegisterEffectEvent(this, "CookedAt");
			base.Register(Object);
		}

		public override void Unregister(GameObject Object)
		{
			Object.UnregisterEffectEvent(this, "Eating");
			Object.UnregisterEffectEvent(this, "CookedAt");
			base.Unregister(Object);
		}


        public override bool FireEvent(Event E)
		{
            if(E.ID == "CookedAt"){
                Object.Heal(Object.Stat("level")/2);

            }
			if (E.ID == "Eating")
			{
				GameObject GO = E.GetGameObjectParameter("Food");
                if(GO != null){
                    int w = GO.pPhysics.Weight*Object.Stat("level");
                    Object.Heal(w);
                }
			}
            return base.FireEvent(E);
		}



	}
}