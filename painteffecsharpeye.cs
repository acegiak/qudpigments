using System;
using System.Text;
using XRL.Rules;
using XRL.World.Parts;
using Qud.API;
using XRL.UI;
using XRL.Core;
using XRL.World;

namespace XRL.World.Parts.Effects
{
	[Serializable]
	public class acegiak_PaintEffectSharpEye : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectSharpEye():base()
		{

		}

		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nYou gain a +2 bonus to your Agility when aiming missile weapons.";
		}

        public override bool Apply(GameObject GO){
        	GO.ApplyStatShift("AV",1);
            GO.ModIntProperty("MissileWeaponAccuracyBonus", 2,true);
			return base.Apply(GO);
        }

        public override void Remove(GameObject GO){
            GO.ModIntProperty("MissileWeaponAccuracyBonus", -2,true);
			base.Remove(GO);

        }

		



	}
	}