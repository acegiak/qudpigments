using System;
using System.Text;
using XRL.Rules;

namespace XRL.World.Parts.Effects
{
	[Serializable]
	public class acegiak_PaintEffectSpark : Effect
	{
		public int HealCounter;

		public int InspectorPropertyApplied;

		public int DisassemblePropertyApplied;

		public string DamageAttributes;

		public GameObject SittingOn;

		public int _Level;

		public int Level
		{
			get
			{
				return _Level;
			}
			set
			{
				UnapplyProperties();
				_Level = value;
				ApplyProperties();
			}
		}

		public acegiak_PaintEffectSpark()
		{
			Duration = 1200*12;
			base.DisplayName = "&Csparkspaint";
		}

		

		public override bool SameAs(Effect e)
		{
			return false;
		}

		public override string GetDetails()
		{
			return "Aids in examining and disassembling artifacts.";
		}


		public void ApplyProperties()
		{
			if (Object != null)
			{
				if (InspectorPropertyApplied != 0)
				{
					Object.ModIntProperty("InspectorEquipped", -InspectorPropertyApplied);
				}
				InspectorPropertyApplied = Math.Min(2 + Level, 4);
				Object.ModIntProperty("InspectorEquipped", InspectorPropertyApplied,  true);
				if (DisassemblePropertyApplied != 0)
				{
					Object.ModIntProperty("DisassembleBonus", -DisassemblePropertyApplied);
				}
				DisassemblePropertyApplied = Math.Min(2 + Level, 4);
				Object.ModIntProperty("DisassembleBonus", DisassemblePropertyApplied,  true);
			}
		}

		public void UnapplyProperties()
		{
			if (Object != null)
			{
				if (InspectorPropertyApplied != 0)
				{
					Object.ModIntProperty("InspectorEquipped", -InspectorPropertyApplied, RemoveIfZero: true);
				}
				InspectorPropertyApplied = 0;
				if (DisassemblePropertyApplied != 0)
				{
					Object.ModIntProperty("DisassembleBonus", -DisassemblePropertyApplied, RemoveIfZero: true);
				}
				DisassemblePropertyApplied = 0;
			}
		}


		public override void Register(GameObject Object)
		{
			Object.RegisterEffectEvent(this, "AfterDeepCopyWithoutEffects");
			Object.RegisterEffectEvent(this, "BeforeDeepCopyWithoutEffects");
			Object.RegisterEffectEvent(this, "BodyPositionChanged");
			Object.RegisterEffectEvent(this, "EndTurn");
			Object.RegisterEffectEvent(this, "GetDisplayName");
			Object.RegisterEffectEvent(this, "LeaveCell");
			Object.RegisterEffectEvent(this, "MovementModeChanged");
			ApplyProperties();
			ApplyStats();
			base.Register(Object);
		}

		public override void Unregister(GameObject Object)
		{
			Object.UnregisterEffectEvent(this, "AfterDeepCopyWithoutEffects");
			Object.UnregisterEffectEvent(this, "BeforeDeepCopyWithoutEffects");
			Object.UnregisterEffectEvent(this, "BodyPositionChanged");
			Object.UnregisterEffectEvent(this, "EndTurn");
			Object.UnregisterEffectEvent(this, "GetDisplayName");
			Object.UnregisterEffectEvent(this, "LeaveCell");
			Object.UnregisterEffectEvent(this, "MovementModeChanged");
			UnapplyStats();
			UnapplyProperties();
			base.Unregister(Object);
		}


		public override bool FireEvent(Event E)
		{
			if (E.ID == "BeforeDeepCopyWithoutEffects")
			{
				UnapplyProperties();
			}
			else if (E.ID == "AfterDeepCopyWithoutEffects")
			{
				ApplyProperties();
			}
			return base.FireEvent(E);
		}
	}
}
