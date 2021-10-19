 using System;
using System.Text;
using XRL.Rules;
using XRL.World.Parts;
using Qud.API;
using XRL.UI;
using XRL.Core;

namespace XRL.World.Effects
{
	[Serializable]
	public class acegiak_PaintEffectTravel : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectTravel():base()
		{

		}
		public override void Register(GameObject Object)
		{
			Object.RegisterEffectEvent(this, "TravelSpeed");
			base.Register(Object);
		}

		public override void Unregister(GameObject Object)
		{
			Object.UnregisterEffectEvent(this, "TravelSpeed");
			base.Unregister(Object);
		}

		public override string GetDetails()
		{
			return base.GetDetails()+"\nIncreases travel speed on world map by 10%.";
		}

        public override bool FireEvent(Event E){
            if (E.ID == "TravelSpeed")
			{
						E.SetParameter("PercentageBonus", E.GetIntParameter("PercentageBonus") + 10);
			}

            return base.FireEvent(E);
        }

		



	}
	}
 
 
