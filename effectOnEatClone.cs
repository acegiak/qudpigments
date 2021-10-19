using System;
using XRL.Rules;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_EffectOnEat : IPart
	{
		public string DisplayName;

		public string Effect = "CookingDomainReflect_Reflect100_ProceduralCookingTriggeredAction_Effect";

		public string Duration;

		public acegiak_EffectOnEat()
		{
			base.Name = "EffectOnEat";
		}

		public override bool SameAs(IPart p)
		{
			acegiak_EffectOnEat effectOnEat = p as acegiak_EffectOnEat;
			return effectOnEat.Effect == Effect && effectOnEat.Duration == Duration;
		}

		public override void Register(GameObject Object)
		{
			Object.RegisterPartEvent(this, "OnEat");
			base.Register(Object);
		}

		public override bool FireEvent(Event E)
		{
			if (E.ID == "OnEat")
			{
				Effect effect = Activator.CreateInstance(ModManager.ResolveType("XRL.World.Effects." + Effect)) as Effect;
				if (!string.IsNullOrEmpty(Duration))
				{
					effect.Duration = Stat.Roll(Duration);
				}
				if (!string.IsNullOrEmpty(DisplayName))
				{
					effect.DisplayName = DisplayName;
				}
				GameObject parameter = E.GetParameter<GameObject>("Eater");
				parameter.ApplyEffect(effect);
				return true;
			}
			return base.FireEvent(E);
		}
	}
}
