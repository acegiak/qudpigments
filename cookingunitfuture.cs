using System;
using XRL.Rules;

namespace XRL.World.Parts.Effects
{
	[Serializable]
	public class CookingDomainOracular_UnitPremonitionLength : ProceduralCookingEffectUnit
	{
		public GameObject Object;

		public override string GetDescription()
		{
			return "Causes @thisCreature to see into the future for up to 50 turns.";
		}

		public override void Apply(GameObject Object, Effect parent)
		{
			int numClones = Stat.Random(5, 50);
			Object.ApplyEffect(new acegiak_OracularPremonition(numClones));
		}

		public override void Remove(GameObject Object, Effect parent)
		{
		}
	}
}
