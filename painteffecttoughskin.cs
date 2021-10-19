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
	public class acegiak_PaintEffectToughSkin : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectToughSkin():base()
		{

		}

		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nYou gain +1 AV.";
		}

        public override bool Apply(GameObject GO){
        	GO.ApplyStatShift("AV",1);
			return base.Apply(GO);
        }

        public override void Remove(GameObject GO){
            GO.UnapplyStatShift("AV",1);
			base.Remove(GO);

        }

		



	}
	}