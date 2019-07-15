using System;
using XRL.Core;
using XRL.UI;
using Qud.API;
using XRL.Language;
using XRL.Rules;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_PaintingTeacher : IPart
	{
		public string useFactionForFeelingFloor;

		public bool pettableIfPositiveFeeling;

		private bool bOnlyAllowIfLiked = true;

		public override bool SameAs(IPart p)
		{
			return false;
		}

		public override bool AllowStaticRegistration()
		{
			return true;
		}

		public override void Register(GameObject Object)
		{

			Object.RegisterPartEvent(this, "GetInventoryActions");
			Object.RegisterPartEvent(this, "InvCommandLearnPaint");
			base.Register(Object);
		}

        public void TeachPainting(GameObject who){
            acegiak_PaintingRecipe recipe = new acegiak_PaintingRecipe("children","kiddos, so many wee ones");
			if(ParentObject.pBrain != null){
				recipe.FormFaction = ParentObject.pBrain.GetPrimaryFaction();
				if(Stat.Rnd2.NextDouble()<0.5f){
					recipe.FormName = ParentObject.DisplayNameOnly+"'s warpaint";
				}else{
					if(Factions.FactionList[ParentObject.pBrain.GetPrimaryFaction()].FormatWithArticle){
						recipe.FormName = "warpaint of "+Factions.FactionList[ParentObject.pBrain.GetPrimaryFaction()].getFormattedName();
					}else{
						recipe.FormName = Grammar.Adjectify(Factions.FactionList[ParentObject.pBrain.GetPrimaryFaction()].getFormattedName())+" warpaint";
					}
				}
			}
            recipe.Reveal();
        }

		public override bool FireEvent(Event E)
		{
			
			if (E.ID == "GetInventoryActions")
			{
				E.GetParameter<EventParameterGetInventoryActions>("Actions").AddAction("LearnPaint", 'l', false, "&Wl&yearn painting", "InvCommandLearnPaint", 10);
			}
			if (E.ID == "InvCommandLearnPaint")
			{
                TeachPainting(E.GetGameObjectParameter("Owner"));
				E.RequestInterfaceExit();
			}
			return base.FireEvent(E);
		}
	}
}
