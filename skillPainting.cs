using System;
using System.Collections.Generic;
using XRL.UI;
using XRL.World.AI.GoalHandlers;
using UnityEngine;
using XRL.Liquids;
using System.Linq;
using System.Text;
using Qud.API;
using XRL.World.Parts.Effects;

namespace XRL.World.Parts.Skill
{
	[Serializable]
	internal class acegiak_CustomsPainting : BaseSkill
	{
		public acegiak_CustomsPainting()
		{
			DisplayName = "acegiak_CustomsPainting";
		}

		public override bool AllowStaticRegistration()
		{
			return true;
		}

		public override void Register(XRL.World.GameObject Object)
		{
			Object.RegisterPartEvent(this, "OwnerGetInventoryActions");
			Object.RegisterPartEvent(this, "InvCommandPainting");
			Object.RegisterPartEvent(this, "InvCommandPaintingSelf");
			base.Register(Object);
		}

        public void Paint(XRL.World.GameObject Canvas,XRL.World.GameObject Artist){


            //choose colour
            if(Artist.IsPlayer()){


                // CHOOSE BODY PART
                
                List<string> ChoiceList = new List<string>();
                List<char> HotkeyList = new List<char>();
                char ch = 'a';

                BodyPart paintingpart = null;

                if(Canvas.GetPart<Body>() != null){
                    List<BodyPart> parts = new List<BodyPart>();
                    ch = 'a';
                    foreach(BodyPart part in Canvas.GetPart<Body>()._Body.GetParts())
                    {
                        if(!part.Abstract && !part.Extrinsic){
                            parts.Add(part);
                            HotkeyList.Add(ch);
                            ChoiceList.Add(part.Name);
                            ch = (char)(ch + 1);
                        }
                    }
                    if (parts.Count == 0)
                    {
                        Popup.Show("They don't have any body parts??");
                        return;
                    }
                    int partChoice = Popup.ShowOptionList(string.Empty, ChoiceList.ToArray(), HotkeyList.ToArray(), 0, "Which part of "+Canvas.the+Canvas.DisplayNameOnly+" will you paint?", 60,  false, true);
                    if (partChoice >= 0){
                        paintingpart = parts[partChoice];
                    }

                }



                // CHOOSE DESIGN


                ChoiceList = new List<string>();
                HotkeyList = new List<char>();
                ChoiceList.Add("Custom design.");
                ch = 'a';
                HotkeyList.Add(ch);
                ch = (char)(ch + 1);

                List<acegiak_PaintingRecipe> recipes = new List<acegiak_PaintingRecipe>();


                foreach(JournalObservation observation in JournalAPI.GetObservations(null)){
                    if(observation is acegiak_PaintingRecipe){
                        acegiak_PaintingRecipe recipe = observation as acegiak_PaintingRecipe;
                        if(recipe.revealed){
                            if(recipe.PartType == null || (paintingpart != null && recipe.PartType == paintingpart.Type)){
                                recipes.Add(recipe);
                                ChoiceList.Add(recipe.FormName);
                                HotkeyList.Add(ch);
                                ch = (char)(ch + 1);
                            }
                        }
                    }
                }
                string BaseColour = "";
                string DetailColour = "";
                List<string> chosenStuffs = new List<string>();

                int designNumber = Popup.ShowOptionList(string.Empty, ChoiceList.ToArray(), HotkeyList.ToArray(), 0, "Choose a design.", 60,  false,  true);




                // CHOOSE BASE COLOR

                Inventory part2 = Artist.GetPart<Inventory>();
                List<XRL.World.GameObject> ObjectChoices = new List<XRL.World.GameObject>();
                ChoiceList = new List<string>();
                HotkeyList = new List<char>();
                ch = 'a';
                part2.ForeachObject(delegate(XRL.World.GameObject GO)
                {
                    if(GO != null && GO.GetPart<LiquidVolume>() != null &&  GO.GetPart<LiquidVolume>().GetPrimaryLiquid() != null && GO.GetPart<LiquidVolume>().GetPrimaryLiquid().Name != "water"){
                        if(designNumber <= 0 ||recipes[designNumber-1].BaseColor == null ||
                        (recipes[designNumber-1].BaseColor.Length == 1 && GO.GetPart<LiquidVolume>().GetPrimaryLiquidColor() == recipes[designNumber-1].BaseColor) ||
                        (recipes[designNumber-1].BaseColor.Length > 1 && GO.GetPart<LiquidVolume>().GetPrimaryLiquid().Name == recipes[designNumber-1].BaseColor) ){
                            ObjectChoices.Add(GO);
                            HotkeyList.Add(ch);
                            ChoiceList.Add(GO.DisplayName);
                            ch = (char)(ch + 1);
                        }
                    }
                });
                if (ObjectChoices.Count == 0)
                {
                    if(designNumber > 0){
                        Popup.Show("You have no "+acegiak_LiquidDye.ColorNames[recipes[designNumber-1].BaseColor]+" pigments.");
                    }else{
                        Popup.Show("You have nothing to paint with.");
                    }
                    return;
                }
                int baseColor = Popup.ShowOptionList(string.Empty, ChoiceList.ToArray(), HotkeyList.ToArray(), 0, "Choose your base colour.", 60,  false, true);
                if (baseColor >= 0)
                {
                    BaseColour = ObjectChoices[baseColor].GetPart<LiquidVolume>().GetPrimaryLiquidColor();
                    if(ObjectChoices[baseColor].GetPart<LiquidVolume>().GetPrimaryLiquid() is acegiak_LiquidDye){
                        //that's fine
                    }else{
                        StringBuilder b = new StringBuilder();
                        ObjectChoices[baseColor].GetPart<LiquidVolume>().AppendLiquidName(b);
                        chosenStuffs.Add(b.ToString());
                    }
                }
                if(BaseColour == ""){
                    BaseColour = Canvas.pRender.TileColor==String.Empty?Canvas.pRender.GetForegroundColor().Replace("&",""):Canvas.pRender.TileColor.Replace("&","");
                }




                //CHOOSE DETAIL COLOR

                List<XRL.World.GameObject> ObjectChoices2 = new List<XRL.World.GameObject>();
                ChoiceList = new List<string>();
                HotkeyList = new List<char>();
                ch = 'a';
                part2.ForeachObject(delegate(XRL.World.GameObject GO)
                {
                    if(GO != null && GO.GetPart<LiquidVolume>() != null &&  GO.GetPart<LiquidVolume>().GetPrimaryLiquid() != null && GO.GetPart<LiquidVolume>().GetPrimaryLiquid().Name != "water"){
                        if(designNumber <= 0 ||recipes[designNumber-1].DetailColor == null ||
                        (recipes[designNumber-1].DetailColor.Length == 1 && GO.GetPart<LiquidVolume>().GetPrimaryLiquidColor() == recipes[designNumber-1].DetailColor) ||
                        (recipes[designNumber-1].DetailColor.Length > 1 && GO.GetPart<LiquidVolume>().GetPrimaryLiquid().Name == recipes[designNumber-1].DetailColor) ){
                            ObjectChoices2.Add(GO);
                            HotkeyList.Add(ch);
                            ChoiceList.Add(GO.DisplayName);
                            ch = (char)(ch + 1);
                        }
                    }
                });
                if (ObjectChoices2.Count == 0)
                {
                    if(designNumber > 0){
                        Popup.Show("You have no "+acegiak_LiquidDye.ColorNames[recipes[designNumber-1].DetailColor]+" pigments.");
                    }else{
                        Popup.Show("You have nothing to paint with.");
                    }
                    return;
                }
                int detailColor = Popup.ShowOptionList(string.Empty, ChoiceList.ToArray(), HotkeyList.ToArray(), 0, "Choose your detail colour.", 60,  false, true);
                if (detailColor >= 0)
                {
                    DetailColour = ObjectChoices[detailColor].GetPart<LiquidVolume>().GetPrimaryLiquidColor();
                    if(ObjectChoices[detailColor].GetPart<LiquidVolume>().GetPrimaryLiquid() is acegiak_LiquidDye){
                        //that's fine
                    }else{
                        StringBuilder c = new StringBuilder();
                        ObjectChoices[detailColor].GetPart<LiquidVolume>().AppendLiquidName(c);
                        chosenStuffs.Add(c.ToString());
                    }
                }
                if(DetailColour == ""){
                    DetailColour = Canvas.pRender.DetailColor;
                }


                if(detailColor < 0 && baseColor < 0){
                    return;
                }



                // DO THE PAINTING

                acegiak_ModHandPainted painting = new acegiak_ModHandPainted();
                if(designNumber > 0 && recipes[designNumber-1].className != null){
                    Type t = typeof(acegiak_ModHandPainted);

                    painting = Activator.CreateInstance(Type.GetType(recipes[designNumber-1].className)) as acegiak_ModHandPainted;
                    // painting = (acegiak_ModHandPainted)Activator.CreateInstance(null, recipes[designNumber-1].className ).Unwrap();
                }

                painting.Engraving = "mysterious shapes";

                if(designNumber ==0) {
                    painting.Engraving = Popup.AskString("What do you depict?", string.Empty, 999);
                }
                if(designNumber > 0){
                    painting.Engraving = recipes[designNumber-1].FormDescription;
                    painting.Faction = recipes[designNumber-1].FormFaction;
                    painting.DisplayName = recipes[designNumber-1].FormName;
                }

                painting.BaseColour = BaseColour;
                painting.DetailColour = DetailColour;
                if(paintingpart != null){
                    painting.BodyPartId = paintingpart.ID;
                }
                if(chosenStuffs.Count >0){
                    painting.With = String.Join(" and ",chosenStuffs.ToArray());
                }

                if(baseColor >=0){
                    ObjectChoices[baseColor].GetPart<LiquidVolume>().GetPrimaryLiquid().PouredOn(new LiquidVolume(ObjectChoices[baseColor].GetPart<LiquidVolume>().GetPrimaryLiquid().ID,1),Canvas);
                    ObjectChoices[baseColor].GetPart<LiquidVolume>().UseDrams(1);
                }
                if(detailColor >=0){
                    ObjectChoices2[detailColor].GetPart<LiquidVolume>().GetPrimaryLiquid().PouredOn(new LiquidVolume(ObjectChoices[detailColor].GetPart<LiquidVolume>().GetPrimaryLiquid().ID,1),Canvas);
                    ObjectChoices2[detailColor].GetPart<LiquidVolume>().UseDrams(1);
                }
                Canvas.ApplyEffect(painting);

            }
        }

		public override bool FireEvent(Event E)
		{
			 if (E.ID == "OwnerGetInventoryActions")
			{
				EventParameterGetInventoryActions eventParameterGetInventoryActions = E.GetParameter("Actions") as EventParameterGetInventoryActions;
				XRL.World.GameObject GO = E.GetGameObjectParameter("Object");

                eventParameterGetInventoryActions.AddAction("Paint", 'P', true, "&WP&yaint "+GO.the+GO.DisplayNameOnly, "InvCommandPainting");


                if(GO.GetPart<LiquidVolume>() != null){
                    eventParameterGetInventoryActions.AddAction("PaintSelf", 's', true, "Paint your&Ws&yelf", "InvCommandPaintingSelf");
                }
                return true;
            }
			if (E.ID == "InvCommandPainting")
			{
				XRL.World.GameObject Owner = E.GetGameObjectParameter("Owner");
				XRL.World.GameObject Object = E.GetGameObjectParameter("Object");
                if(Object.pBrain != null && Object.pBrain.GetFeeling(Owner)<50){
                    Object.pBrain.AdjustFeeling(Owner,-10);
                    if(Owner.IsPlayer()){
                        Popup.Show(Object.The+Object.DisplayNameOnly+" doesn't want to be painted!");
                    }
                    if(Object.MakeSave("Agility", 20, Owner, null, "Painting")){
                        return false;
                    }
                }
                Paint(Object,Owner);
			}
			if (E.ID == "InvCommandPaintingSelf")
			{
				XRL.World.GameObject Owner = E.GetGameObjectParameter("Owner");
                
                Paint(Owner,Owner);
			}
			return base.FireEvent(E);
		}

		public override bool AddSkill(GameObject GO)
		{
			return true;
		}

		public override bool RemoveSkill(GameObject GO)
		{
			return true;
		}
	}
}
