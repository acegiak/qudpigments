using System;

namespace XRL.World.Parts.Effects
{
	[Serializable]
	public class CookingDomainOracularPremonition_ProceduralCookingTriggeredAction : ProceduralCookingTriggeredAction
	{
		public override string GetDescription()
		{
			return "@they peer into the future for a short time.";
		}

		public override string GetNotification()
		{
			return "@they peer into the future for a short time.";
		}

		public override void Apply(GameObject go)
		{
			if (!go.HasEffect("acegiak_OracularPremonition"))
			{
				go.ApplyEffect(new acegiak_OracularPremonition(50));
			}
		}
	}
}
