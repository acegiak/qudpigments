using System;
using System.Text;
using XRL.Rules;
using XRL.World.Parts;
using Qud.API;
using XRL.UI;
using XRL.Core;

namespace XRL.World.Parts.Effects
{
	[Serializable]
	public class acegiak_PaintEffectTerror : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectTerror():base()
		{

		}
		public override void Register(GameObject Object)
		{
			Object.RegisterEffectEvent(this, "TakeDamage");
			base.Register(Object);
		}

		public override void Unregister(GameObject Object)
		{
			Object.UnregisterEffectEvent(this, "TakeDamage");
			base.Unregister(Object);
		}
		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nHas a chance to terrify enemies.";
		}

        public override bool FireEvent(Event E){
            if (E.ID == "TakeDamage")
			{
                GameObject o = E.GetGameObjectParameter("Attacker");
                if(o == null || o.GetPart<Brain>() == null){
                    //Popup.Show("What are you reading??");
                    return true;
                }
				XRL.World.Parts.Mutation.Fear.ApplyFearToObject("2d8", 10, o, Object, null, false, false);
				return true;
			}

            return base.FireEvent(E);
        }

		



	}
	}