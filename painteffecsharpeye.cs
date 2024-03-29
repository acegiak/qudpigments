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
	public class acegiak_PaintEffectSharpEye : acegiak_ModHandPainted
	{

		string desc = "";


		public acegiak_PaintEffectSharpEye():base()
		{
		}

		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nFiring missile weapons costs 25% less energy.";
		}

        

	    public override void Register(GameObject Object)
		{
			Object.RegisterEffectEvent(this, "CommandFireMissile");
			base.Register(Object);
		}

		public override void Unregister(GameObject Object)
		{
			Object.UnregisterEffectEvent(this, "CommandFireMissile");
			base.Unregister(Object);
		}

		public override bool FireEvent(Event E)
		{
			if (E.ID == "CommandFireMissile")
			{
				float multiplier = 1f;
				if(E.GetParameter("EnergyMultiplier") != null){
					multiplier = (float)E.GetParameter("EnergyMultiplier");
				}
				multiplier = multiplier * 0.75f;
				E.SetParameter("EnergyMultiplier",multiplier);
			}
			return base.FireEvent(E);
		}
	}
}