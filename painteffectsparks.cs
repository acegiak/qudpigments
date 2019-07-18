using System;
using System.Text;
using XRL.Rules;

namespace XRL.World.Parts.Effects
{
	[Serializable]
	public class acegiak_PaintEffectSpark : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectSpark():base()
		{

		}

		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nAids in examining and disassembling artifacts.";
		}


		public override bool Apply(GameObject Object)
		{
			if (Object != null)
			{
				Object.ModIntProperty("InspectorEquipped", 3,  true);
				Object.ModIntProperty("DisassembleBonus", 3,  true);
			}
			return base.Apply(Object);
		}

		public override void Remove(GameObject Object)
		{
			if (Object != null)
			{
				Object.ModIntProperty("InspectorEquipped", -3,  true);
				Object.ModIntProperty("DisassembleBonus", -3,  true);
			}
			base.Remove(Object);
		}



	}
	}