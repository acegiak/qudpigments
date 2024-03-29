using System;
using XRL.Core;
using XRL.World.Parts;

namespace XRL.World.Effects
{
	[Serializable]
	public class CookingDomainLovesick_ProceduralCookingTriggeredAction : ProceduralCookingTriggeredAction
	{
		public override string GetDescription()
		{
			return "@they become lovesick.";
		}

		public override string GetNotification()
		{
			return "@they become lovesick.";
		}

		public override void Apply(GameObject go)
		{
			if (!go.HasEffect("Lovesick"))
			{
				go.ApplyEffect(new Lovesick(50, XRLCore.Core.Game.Player.Body));
			}
		}
	}
}
