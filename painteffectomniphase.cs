using System;
using System.Text;
using XRL.Rules;
using XRL.World.Parts;
using Qud.API;
using XRL.UI;
using XRL.Core;
using XRL.World;

namespace XRL.World.Effects
{
	[Serializable]
	public class acegiak_PaintEffectPhase : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectPhase():base()
		{

		}

		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nYou become omniphased.";
		}

        public override bool Apply(GameObject GO){
        	GO.SetStringProperty("omniphase", "true");
			return base.Apply(GO);
        }

        public override void Remove(GameObject GO){
            GO.DeleteStringProperty("omniphase");
			base.Remove(GO);

        }

		



	}
	}