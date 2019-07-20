using System;
using XRL.Core;
using XRL.UI;
using Qud.API;
using XRL.Language;
using XRL.Rules;
using XRL.Liquids;
using System.Linq;
using System.Collections.Generic;
using XRL.World;
using XRL.World.Parts.Skill;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_PaintingTeacher : IPart
	{

		private string myRecipeId;

		[NonSerialized]
		private acegiak_PaintingRecipe myRecipe;

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

			Object.RegisterPartEvent(this, "VisitConversationNode");
			Object.RegisterPartEvent(this, "ShowConversationChoices");
			base.Register(Object);
		}

        public acegiak_PaintingRecipe GetPaintingRecipe(){
			if(ParentObject.IsPlayer()){
				return null;
			}
			if(this.myRecipe != null){
				return this.myRecipe;
			}
			if(this.myRecipeId != null){
				foreach(acegiak_PaintingRecipe observation in acegiak_CustomsPainting.Recipes){
					if(observation is acegiak_PaintingRecipe && observation.secretid == this.myRecipeId){
						this.myRecipe = observation as acegiak_PaintingRecipe;
						return this.myRecipe;
					}
				}
			}

			acegiak_PaintingRecipe recipe = new acegiak_PaintingRecipe("","");
			recipe.PopulateBase();
			recipe.PopulateFaction(ParentObject.pBrain.GetPrimaryFaction());
			recipe.PopulatePerson(ParentObject);
			recipe.PopulateDescriptions();
			
			recipe.text = "Painting Style: "+recipe.FormName + "\n" + recipe.FormDescription;
            this.myRecipe = recipe;
			this.myRecipeId = myRecipe.secretid;
			return this.myRecipe;
        }

		public override bool FireEvent(Event E)
		{

			if(E.ID == "ShowConversationChoices" ){
				if(XRLCore.Core.Game.Player.Body.GetPart<acegiak_CustomsPainting>()!= null ){
					if(this.GetPaintingRecipe() != null && !this.GetPaintingRecipe().revealed){
					
					
					if(E.GetParameter<ConversationNode>("CurrentNode") != null && E.GetParameter<ConversationNode>("CurrentNode") is WaterRitualNode){
						WaterRitualNode wrnode = E.GetParameter<ConversationNode>("CurrentNode") as WaterRitualNode;
						List<ConversationChoice> Choices = E.GetParameter<List<ConversationChoice>>("Choices") as List<ConversationChoice>;

						if(Choices.Where(b=>b.ID == "LearnPaintingStyle").Count() <= 0){

							bool canlearn = XRLCore.Core.Game.PlayerReputation.get(ParentObject.pBrain.GetPrimaryFaction()) >50;

							ConversationChoice conversationChoice = new ConversationChoice();
							conversationChoice.Text = (canlearn?"&G":"&K")+"Teach me to paint "+this.GetPaintingRecipe().FormName+" [-50 reputation]";
							conversationChoice.GotoID = "End";
							conversationChoice.ParentNode = wrnode;
							conversationChoice.ID = "LearnPaintingStyle";
							conversationChoice.onAction = delegate()
							{
								if(!canlearn){
									Popup.Show("You do not have enough reputation.");
									return false;
								}
								this.GetPaintingRecipe().revealed = true;
								Popup.Show("You learned to paint: "+this.GetPaintingRecipe().FormName);
								XRLCore.Core.Game.PlayerReputation.modify(Factions.FactionList[ParentObject.pBrain.GetPrimaryFaction()].Name, -50,false);

								return true;
							};
							Choices.Add(conversationChoice);
							Choices.Sort(new ConversationChoice.Sorter());
							// wrnode.Choices.Add(conversationChoice);
							// wrnode.SortEndChoicesToEnd();
							E.SetParameter("CurrentNode",wrnode);
						}
					}
					
					}
				}
			}
			return base.FireEvent(E);
		}
	}
}
