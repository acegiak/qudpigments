using System;
using System.Text;
using XRL.Rules;

namespace XRL.World.Parts.Effects
{
	[Serializable]
	public class acegiak_PaintEffectCrits : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectCrits():base()
		{

		}

		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nYour chance to score critical hits increases by 5%.";
		}



	    public override void Register(GameObject Object)
		{
			Object.RegisterEffectEvent(this, "GetNaturalHitBonus");
			base.Register(Object);
		}

		public override void Unregister(GameObject Object)
		{
			Object.UnregisterEffectEvent(this, "GetNaturalHitBonus");
			base.Unregister(Object);
		}

		public override bool FireEvent(Event E)
		{
			if (E.ID == "GetNaturalHitBonus")
			{
				E.SetParameter("Result", E.GetIntParameter("Result", 0) + 1);
			}
            return base.FireEvent(E);
		}



	}
	}