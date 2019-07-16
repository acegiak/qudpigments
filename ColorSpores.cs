using System;
using XRL.Rules;
using XRL.World.Parts.Effects;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_ColorSpores : IPart
	{

		public string Color = "Y";



		public override bool SameAs(IPart p)
		{
			acegiak_ColorSpores acegiak_ColorSpores = p as acegiak_ColorSpores;
			if (acegiak_ColorSpores.Color != Color)
			{
				return false;
			}

			return base.SameAs(p);
		}

		public override bool AllowStaticRegistration()
		{
			return true;
		}

		public override void Register(GameObject Object)
		{
			Object.RegisterPartEvent(this, "GasSpawned");
			Object.RegisterPartEvent(this, "EndTurn");
			Object.RegisterPartEvent(this, "ObjectEnteredCell");
			base.Register(Object);
		}

		public void ApplySpores(GameObject GO)
		{
			if (!GO.FireEvent("ApplySpores"))
			{
				return;
			}
			foreach(Effect e in GO.GetEffects("acegiak_ModHandPainted")){
				if(e is acegiak_ModHandPainted){
					GO.RemoveEffect(e);
				}
			}
            acegiak_ModHandPainted painted = new acegiak_ModHandPainted();
            if(Stat.Rnd2.NextDouble() <0.5f){
                painted.DetailColour = this.Color;
				painted.BaseColour = "y";
            }else{
                painted.BaseColour = this.Color;
				painted.DetailColour = "y";
            }
            painted.DisplayName = "&"+this.Color + "mottled";
            painted.Engraving = "colourful spores";
            painted.Duration = 120;
            GO.ApplyEffect(painted);
		}

		public override bool FireEvent(Event E)
		{
			if (E.ID == "EndTurn")
			{
				Cell currentCell = ParentObject.CurrentCell;
				if (currentCell != null)
				{
					foreach (GameObject item in currentCell.GetObjectsWithPart("Physics"))
					{
						if (item != ParentObject && item != ParentObject.GetPart<Gas>().Creator && item.PhaseMatches(ParentObject))
						{
							ApplySpores(item);
						}
					}
				}
			}
			else if (E.ID == "ObjectEnteredCell")
			{
				GameObject gameObjectParameter = E.GetGameObjectParameter("Object");
				if (gameObjectParameter != ParentObject && gameObjectParameter != ParentObject.GetPart<Gas>().Creator && gameObjectParameter.PhaseMatches(ParentObject))
				{
					ApplySpores(gameObjectParameter);
				}
			}
			return base.FireEvent(E);
		}
	}
}

