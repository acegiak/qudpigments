using System;
using System.Text;
using XRL.Rules;
using XRL.World.Encounters;
using XRL.UI;
using XRL.World.Parts;

namespace XRL.World.Effects
{
	[Serializable]
	public class acegiak_PaintEffectStaunch : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectStaunch():base()
		{

		}

		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nReduces the length of bleeding effects.";
		}


	    public override void Register(GameObject Object)
		{
			Object.RegisterEffectEvent(this, "EndTurn");
			base.Register(Object);
		}

		public override void Unregister(GameObject Object)
		{
			Object.UnregisterEffectEvent(this, "EndTurn");
			base.Unregister(Object);
		}

		public override bool FireEvent(Event E)
		{
			
			if (E.ID == "EndTurn")
			{
				
                if (Object.HasEffect("Bleeding"))
                {
                    foreach (Effect effect in Object.GetEffects("Bleeding"))
                    {
                        effect.Duration -= 1;
                    }
                }
            }
            return base.FireEvent(E);
		}




	}
	}