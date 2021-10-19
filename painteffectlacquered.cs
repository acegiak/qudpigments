using System;
using System.Text;
using XRL.Rules;
using XRL.World.Parts;
using Qud.API;
using XRL.UI;
using XRL.Core;
using XRL.World;
using XRL.World.Parts;

namespace XRL.World.Effects
{
	[Serializable]
	public class acegiak_PaintEffectLacquered : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectLacquered():base()
		{
		}

		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nThis is with designs repelling liquids, stains, and rust.";
		}

        

	    public override void Register(GameObject Object)
		{
			Object.RegisterEffectEvent(this, "ApplyRusted");
			Object.RegisterEffectEvent(this, "ApplyLiquidCovered");
			Object.RegisterEffectEvent(this, "ApplyLiquidStained");
			base.Register(Object);
		}

		public override void Unregister(GameObject Object)
		{
			Object.UnregisterEffectEvent(this, "ApplyRusted");
			Object.UnregisterEffectEvent(this, "ApplyLiquidCovered");
			Object.UnregisterEffectEvent(this, "ApplyLiquidStained");
			base.Unregister(Object);
		}

		public override bool FireEvent(Event E)
		{
			if (E.ID == "ApplyRusted" || E.ID == "ApplyLiquidCovered" || E.ID == "ApplyLiquidStained")
			{
				return false;
			}
			return base.FireEvent(E);
		}

	}
}