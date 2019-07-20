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
	public class acegiak_PaintEffectPunch : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectPunch():base()
		{

		}

		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nIncreases the damage of your natural attacks.";
		}

        public string DamageMod(string damage, int mod){
            string dice = "";
            int bonus = 0;
            if(damage.Contains("-")){
                string[] split = damage.Split('-');
                dice = split[0];
                bonus = Int32.Parse(split[1]) * -1;
            }else if (damage.Contains("+")){ 
                string[] split = damage.Split('+');
                dice = split[0];
                bonus = Int32.Parse(split[1]);
            }else if (damage.Contains("d")){
                dice = damage;
                bonus = 0;
            }else{
                bonus = Int32.Parse(damage);
            }
            bonus += mod;
            if(dice == ""){
                dice = bonus.ToString();
            }else{
                damage = dice+(bonus>0?"+":"-")+Math.Abs(bonus).ToString();
            }
            return damage;
        }

        public override bool Apply(GameObject GO){
            if(GO == null || GO.GetPart<Body>() == null){
                return base.Apply(GO);
            }
        	
				Body part = GO.GetPart<Body>();
				foreach (BodyPart item in part.GetParts())
				{
					if (item.DefaultBehavior != null && item.DefaultBehavior.GetPart<MeleeWeapon>() != null)
					{
						string damage = item.DefaultBehavior.GetPart<MeleeWeapon>().BaseDamage;
                        
                        damage = DamageMod(damage,2);

                        item.DefaultBehavior.GetPart<MeleeWeapon>().BaseDamage = damage;

					}
					
                }
            return base.Apply(GO);

        }

        public override void Remove(GameObject GO){
            if(GO == null || GO.GetPart<Body>() == null){
                base.Apply(GO);
                return;
            }
        	
				Body part = GO.GetPart<Body>();
				foreach (BodyPart item in part.GetParts())
				{
					if (item.DefaultBehavior != null && item.DefaultBehavior.GetPart<MeleeWeapon>() != null)
					{
						string damage = item.DefaultBehavior.GetPart<MeleeWeapon>().BaseDamage;
                        
                        damage = DamageMod(damage,-2);

                        item.DefaultBehavior.GetPart<MeleeWeapon>().BaseDamage = damage;

					}
					
                }
            base.Remove(GO);
        }

	}
	}