using System;
using XRL.World.Skills.Cooking;
using XRL.UI;
using XRL.Core;
using XRL.World;
using XRL.World.Parts;
using XRL.Rules;
using XRL.Language;
using XRL.Liquids;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HistoryKit;


namespace Qud.API
{
	[Serializable]
	public class acegiak_PaintingRecipe: JournalObservation
	{
		public string FormName;
        public string FormDescription;
		public string FormFaction;

		public string BaseColor;

		public string DetailColor;

		public string PartType;

		[NonSerialized]
		GameObject fetish;

		[NonSerialized]
		GameObject secondaryFetish;

		public string title;

		public string secondaryEffect;

		public string className;
		
		[NonSerialized]
		public static string[] DesignElements = new string[7]{"bold lines","scales","spots","shadows","checkers","arrows","hatching"};

		public override void Reveal()
		{
			if (!this.revealed)
			{
				this.revealed = true;
				Popup.Show("You learn to paint " + FormName + "!");
			}
		}

        public acegiak_PaintingRecipe(string name,string description)
		{
			id = Guid.NewGuid().ToString();
            this.FormName = name;
            this.FormDescription = description;
			
			this.text = name + ":\n" + description;
			this.secretid = id;
			this.attributes.Add("painting");
			this.attributes.Add(id);
            this.category = "Painting";
            this.revealed = false;
            this.time = XRLCore.Core.Game.TimeTicks;
			JournalAPI.Observations.Add(this);

			JournalAPI.Observations.Sort((JournalObservation a, JournalObservation b) => a.time.CompareTo(b.time));
			
        }


		public void PopulateBase(){
			if(Stat.Rnd2.NextDouble()<0.5f){
				BaseColor = acegiak_LiquidDye.ColorNames.Keys.ToList().GetRandomElement();
			}
			if(Stat.Rnd2.NextDouble()<0.5f){
				DetailColor = acegiak_LiquidDye.ColorNames.Keys.ToList().GetRandomElement();
			}
		}

		public void PopulateFaction(string faction){
			FormFaction = faction;
		}

		public void PopulatePerson(GameObject person){
			
			Inventory inventory = null;
			if(person != null){
				inventory = person.GetPart<Inventory>();
			}
			if(inventory != null){
				fetish = person.GetPart<Inventory>().GetObjects().GetRandomElement();
			}

			if(Stat.Rnd2.NextDouble()<0.5f && inventory != null){
				secondaryFetish = person.GetPart<Inventory>().GetObjects().GetRandomElement();
			}

			if(Stat.Rnd2.NextDouble()<0.5f && person != null && person.GetPart<Body>() != null){
				PartType = person.GetPart<Body>()._Body.GetParts().Where(p=>!p.Extrinsic && !p.Abstract).GetRandomElement().Type;
			}

			if(Stat.Rnd2.NextDouble() <0.25f){
				FormFaction = person.the+person.DisplayNameOnly;
			}
		}



		public void PopulateDescriptions(){

			PopulateBuff();
			if(Stat.Rnd2.NextDouble() <0.5f){
					title = title+"paint";
			}else{
					title = title+"mark";
			}

			if(Factions.FactionList.Keys.ToList().Contains(FormFaction)){
				if(Factions.FactionList[FormFaction].FormatWithArticle){
					FormName = title+" of "+Factions.FactionList[FormFaction].getFormattedName();
				}else{
					FormName = Grammar.Adjectify(Grammar.Pluralize(Factions.FactionList[FormFaction].getFormattedName()))+" "+title;
				}
			}else{
				if(FormFaction.ToLower().Contains("this")){
					FormName = title+" of "+FormFaction;
				}else{
					FormName = Grammar.Adjectify(FormFaction)+" "+title;
				}
			}

			FormName = Grammar.MakeTitleCaseWithArticle(FormName);


			if(BaseColor != null){
				if(BaseColor.Length == 1){
					FormDescription += acegiak_LiquidDye.ColorNames[BaseColor];
				}else{
					FormDescription += BaseColor;
				}
			}
			if(DetailColor != null){
				if(BaseColor != null){
					FormDescription +=" and ";
				}
				if(DetailColor.Length == 1){
					FormDescription += acegiak_LiquidDye.ColorNames[DetailColor];
				}else{
					FormDescription += DetailColor;
				}
			}

			List<string> depictions = new List<string>(acegiak_PaintingRecipe.DesignElements);
			string item1 = depictions.GetRandomElement();
			string item2 = depictions.GetRandomElement();

			if(fetish != null){
				item1 = Grammar.Pluralize(Grammar.GetRandomMeaningfulWord(Regex.Replace(fetish.DisplayNameOnly,"\\[.*?\\]","")));
			}
			if(secondaryFetish != null){
				item2 = Grammar.Pluralize(Grammar.GetRandomMeaningfulWord(Regex.Replace(secondaryFetish.DisplayNameOnly,"\\[.*?\\]","")));
			}

			FormDescription += " "+item1;
			if(Stat.Rnd2.NextDouble()<0.5f || secondaryFetish != null){
				FormDescription += " and "+item2;
			}

			if(PartType != null){
				if(Stat.Rnd2.NextDouble()<0.5f){
					FormDescription = PartType+" painted with "+FormDescription;
				}else{
					FormDescription = FormDescription+" painted across the "+PartType;
				}
			}

		}

		public List<string> GetTags(){
			List<string> tags = new List<string>();
			if(secondaryFetish != null){
				tags.Add(secondaryFetish.pPhysics.Category);
			}
			if(fetish != null){
				tags.Add(fetish.pPhysics.Category);
			}
			if(PartType != null){
				tags.Add(PartType);
			}
			if(BaseColor != null && BaseColor.Length > 1){
				tags.Add(BaseColor);
			}
			if(DetailColor != null && DetailColor.Length > 1){
				tags.Add(DetailColor);
			}
			return tags;
		}

		public void PopulateBuff(){
			List<string> tags = GetTags();
			tags.ShuffleInPlace();
			foreach(string tag in tags){
				//IPart.AddPlayerMessage(tag);
				title = tag;
				if(tag == "Food"){
					title = "cooks";
					
				}
				if(tag == "Melee Weapon" || tag == "Grenades"){
					title = "war";
					
				}
				if(tag == "Missile Weapon" || tag == "Ammo"){
					title = "hunts";
					
				}
				if(tag == "Armor" || tag == "Shield"){
					title = "shields";
					
				}
				if(tag == "Artifacts" || tag == "Energy Cell"){
					title = "sparks";
					className = "XRL.World.Parts.Effects.acegiak_PaintEffectSpark";
				}
				if(tag == "Tonics" || tag == "Meds"){
					title = "tender";
					
				}
				if(tag == "Light Source" || tag=="Feet" || tag == "Water Container"){
					title = "wander";
					
				}
				if(tag == "Books" || tag == "Trinket" || tag=="Miscelleous"){
					title = "whisper";
					className = "XRL.World.Parts.Effects.acegiak_PaintEffectWhispers";

					
				}
				if(tag == "Tools" || tag == "Scrap"){
					title = "crafts";
					className = "XRL.World.Parts.Effects.acegiak_PaintEffectSpark";
				}
				if(tag == "Trade Goods" || tag == "Water Container"){
					title = "water";
					
				}
				


			}
		}

	}
}
