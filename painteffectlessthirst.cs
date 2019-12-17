using System;
using System.Text;
using XRL.Rules;

namespace XRL.World.Parts.Effects
{
	[Serializable]
	public class acegiak_PaintEffectLessThirst : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectLessThirst():base()
		{

		}

		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nYou cannot die of thirst.";
		}



	    public override void Register(GameObject Object)
		{
			Object.RegisterEffectEvent(this, "BeforeDie");
			base.Register(Object);
		}

		public override void Unregister(GameObject Object)
		{
			Object.UnregisterEffectEvent(this, "BeforeDie");
			base.Unregister(Object);
		}


        public override bool FireEvent(Event E)
		{
			if (E.ID == "BeforeDie")
			{
				if (Object.HasStat("Hitpoints") && Object.pPhysics.LastDamagedBy == null)
						{
							Object.Statistics["Hitpoints"].Penalty = 0;
						
				}
			}
            return base.FireEvent(E);
		}



	}
}