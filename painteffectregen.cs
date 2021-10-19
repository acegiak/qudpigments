using System;
using System.Text;
using XRL.Rules;
using XRL.World.Parts;

namespace XRL.World.Effects
{
	[Serializable]
	public class acegiak_PaintEffectRegen : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectRegen():base()
		{

		}

		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nYou gain 15% regeneration.";
		}



	    public override void Register(GameObject Object)
		{
			Object.RegisterEffectEvent(this, "Regenerating2");
			base.Register(Object);
		}

		public override void Unregister(GameObject Object)
		{
			Object.UnregisterEffectEvent(this, "Regenerating2");
			base.Unregister(Object);
		}

		public override bool FireEvent(Event E)
		{
			if (E.ID == "Regenerating2")
			{
				float num = 1.15f;
				int value = (int)Math.Ceiling((float)E.GetIntParameter("Amount") * num);
				E.SetParameter("Amount", value);
			}
		
            return base.FireEvent(E);
		}



	}
	}