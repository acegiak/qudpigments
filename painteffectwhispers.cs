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
	public class acegiak_PaintEffectWhispers : acegiak_ModHandPainted
	{
		int amount =1;

		public acegiak_PaintEffectWhispers():base()
		{

		}
		
		public override string GetDetails()
		{
			return base.GetDetails()+"\n+1 MA";
		}

		public override bool Apply(GameObject Object)
		{
			amount = 1;
			Object.Statistics["MA"].Bonus += amount;
			return true;
		}

		public override void Remove(GameObject Object)
		{
			Object.Statistics["MA"].Bonus -= amount;
			amount = 0;
		}
		



	}
	}