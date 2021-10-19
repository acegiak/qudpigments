using System;
using XRL.Core;
using XRL.UI;
using XRL.World.Parts.Mutation;
using XRL.World.Parts;

namespace XRL.World.Effects
{
	[Serializable]
	public class acegiak_OracularPremonition : Effect
	{
		public bool WasPlayer;

		public int HitpointsAtSave;

		public int TemperatureAtSave;

		private long ActivatedSegment;

		public acegiak_OracularPremonition()
		{
		}

		public acegiak_OracularPremonition(int _Duration)
			: this()
		{
			Duration = _Duration;
		}

		public override bool SameAs(Effect e)
		{
			return false;
		}

		public override string GetDescription()
		{
			return "oracular premonition";
		}

		public override string GetDetails()
		{

				return "Can peer into the near future.";
			
		}

		public override bool Apply(GameObject Object)
		{
			if (Object.IsPlayer())
			{
				Popup.Show("The clouds part in your mind and a ray of clarity strikes through.");
			}
			if (Object.IsPlayer())
			{
				WasPlayer = true;
				Precognition.Save();
			}
			else
			{
				WasPlayer = false;
				SensePsychicEffect sensePsychicEffect = SensePsychic.SensePsychicFromPlayer(Object);
				if (sensePsychicEffect != null)
				{
					Effect.AddPlayerMessage("You sense a subtle psychic disturbance.");
				}
			}
			HitpointsAtSave = Object.hitpoints;
			TemperatureAtSave = Object.pPhysics.Temperature;
			return true;
		}

		public override void Remove(GameObject Object)
		{
			if ( Object.IsPlayer() && WasPlayer && Popup.ShowYesNo("Your premonition is about to run out. Would you like to return to the start of your vision?") == DialogResult.Yes)
			{
				Precognition.Load();
			}
		}

		public override void Register(GameObject Object)
		{
			Object.RegisterEffectEvent(this, "BeginTakeAction");
			Object.RegisterEffectEvent(this, "BeforeDie");
			base.Register(Object);
		}

		public override void Unregister(GameObject Object)
		{
			Object.UnregisterEffectEvent(this, "BeginTakeAction");
			Object.UnregisterEffectEvent(this, "BeforeDie");
			base.Unregister(Object);
		}

		public override bool FireEvent(Event E)
		{
		
			if (E.ID == "BeforeDie")
			{
				if (Duration > 0)
				{
					if (ActivatedSegment == XRLCore.Core.Game.Segments)
					{
						Object.hitpoints = HitpointsAtSave;
						if (Object.pPhysics != null)
						{
							Object.pPhysics.Temperature = TemperatureAtSave;
						}
						return false;
					}
					if (Object.IsPlayer())
					{
						if (WasPlayer)
						{
							if (Popup.ShowYesNo("You sense your imminent demise, would you like to return to the start of your vision?") == DialogResult.Yes)
							{
								Precognition.Load();
								ActivatedSegment = XRLCore.Core.Game.Segments;
								return false;
							}
						}
					}
					else if (!Object.IsOriginalPlayerBody() && Object.FireEvent("CheckRealityDistortionUsability"))
					{
						Duration = 0;
						if (Object.Statistics.ContainsKey("Hitpoints"))
						{
							ActivatedSegment = XRLCore.Core.Game.Segments;
							Object.hitpoints = HitpointsAtSave;
							if (Object.pPhysics != null)
							{
								Object.pPhysics.Temperature = TemperatureAtSave;
							}
							Object.DilationSplat();
							string verb = "swim";
							string extra = "before your eyes";
							string terminalPunctuation = "!";
							GameObject @object = Object;
							DidX(verb, extra, terminalPunctuation);
							return false;
						}
					}
				}
			}
			else if (E.ID == "BeginTakeAction")
			{
				if (Duration > 0 && Object.CurrentCell != null)
				{
					Duration--;
					if (Duration <= 0)
					{
						if (Object.IsPlayer())
						{
							Popup.Show("Your mind clouds over once again.");
						}
					}
		
				}
			}
			
			return base.FireEvent(E);
		}

		public override bool Render(RenderEvent E)
		{
			int num = XRLCore.CurrentFrame % 60;
			if (Duration > 0 && num > 30 && num < 40)
			{
				E.Tile = null;
				E.RenderString = string.Empty + 'ì';
				E.ColorString = "&C";
			}
			return true;
		}
	}
}
