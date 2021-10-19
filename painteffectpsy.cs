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
	public class acegiak_PaintEffectPsy : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectPsy():base()
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
			return base.GetDetails()+"\nHas a chance to confuse enemies.";
		}

        public override bool FireEvent(Event E){
            if (E.ID == "TakeDamage")
			{
                GameObject o = E.GetGameObjectParameter("Attacker");
                if(o == null || o.GetPart<Brain>() == null){
                    //Popup.Show("What are you reading??");
                    return true;
                }
				if(!o.MakeSave("Wisdom", 10, Object, null, "Dazzling Paint")){
                    o.ApplyEffect(new Confused(15,1,3));
                }
				return true;
			}

            return base.FireEvent(E);
        }

		



	}
	}