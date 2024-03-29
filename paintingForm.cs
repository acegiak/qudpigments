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
using XRL.World.Parts.Skill;

namespace Qud.API
{

	public class acegiak_bufftag{
		public List<string> tags;
		public string title;
		public string buffclass;

		public acegiak_bufftag(string _tags,string _title, string _buffclass=null){
			this.tags = _tags.Split(',').ToList();
			this.title = _title;
			if(_buffclass != null){
				this.buffclass = _buffclass;
			}
		}
	}

	[Serializable]
	public class acegiak_PaintingRecipe
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

		public string className;

		public string text;

		public string secretid;

		public bool revealed = false;
		
		[NonSerialized]
		public static string[] DesignElements = new string[7]{"bold lines","scales","spots","shadows","checkers","arrows","hatching"};

		public void Reveal()
		{
			if (!this.revealed)
			{
				this.revealed = true;
				Popup.Show("You learn to paint " + FormName + "!");
			}
		}

        public acegiak_PaintingRecipe(string name,string description)
		{
			string id = Guid.NewGuid().ToString();
            this.FormName = name;
            this.FormDescription = description;
			
			this.text = name + ":\n" + description;
			this.secretid = id;
            this.revealed = false;
			if(XRLCore.Core.Game.Player.Body.GetPart<acegiak_CustomsPainting>() != null){
				XRLCore.Core.Game.Player.Body.GetPart<acegiak_CustomsPainting>().Recipes.Add(this);
			}
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

			if(Factions.getIfExists(FormFaction) != null){
				if(Factions.get(FormFaction).FormatWithArticle){
					FormName = title+" of "+Factions.get(FormFaction).getFormattedName();
				}else{
					FormName = Grammar.Adjectify(Grammar.Pluralize(Factions.get(FormFaction).getFormattedName()))+" "+title;
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

		[NonSerialized]
		public List<acegiak_bufftag> bufftags = new List<acegiak_bufftag>{
			new acegiak_bufftag("Food","cooks","XRL.World.Effects.acegiak_PaintEffectCooking"),
			new acegiak_bufftag("Melee Weapon,Genades","war","XRL.World.Effects.acegiak_PaintEffectCrits"),
			new acegiak_bufftag("Missile Weapon,Ammo","hunts","XRL.World.Effects.acegiak_PaintEffectSharpEye"),
			new acegiak_bufftag("Armor,Shield","shields","XRL.World.Effects.acegiak_PaintEffectToughSkin"),
			new acegiak_bufftag("Artifacts,Entergy Cell","sparks","XRL.World.Effects.acegiak_PaintEffectGrease"),
			new acegiak_bufftag("Tonics,Meds","tender","XRL.World.Effects.acegiak_PaintEffectStaunch"),
			new acegiak_bufftag("Light Source,Feet,Water Container","wander","XRL.World.Effects.acegiak_PaintEffectTravel"),
			new acegiak_bufftag("Books,Trinket,Miscellaneous","whisper","XRL.World.Effects.acegiak_PaintEffectWhispers"),
			new acegiak_bufftag("Tools,Scrap","crafts","XRL.World.Effects.acegiak_PaintEffectGrease"),
			new acegiak_bufftag("Trade Goods,Water Container","water","XRL.World.Effects.acegiak_PaintEffectLessThirst"),
			new acegiak_bufftag("Face,Body,Blood","psy","XRL.World.Effects.acegiak_PaintEffectPsy"),
			new acegiak_bufftag("Face,Body,Blood","terror","XRL.World.Effects.acegiak_PaintEffectTerror"),
			new acegiak_bufftag("Body,Arm,Hand","fist","XRL.World.Effects.acegiak_PaintEffectPunch"),
			new acegiak_bufftag("Body,Hand,Face","phase","XRL.World.Effects.acegiak_PaintEffectPhase"),
			new acegiak_bufftag("Melee Weapon,Missile Weapon,Artefact","dry","XRL.World.Effects.acegiak_PaintEffectLacquered")

		};

		public void PopulateBuff(){
			List<string> tags = GetTags();
			tags.ShuffleInPlace();
			foreach(string tag in tags){
				//IPart.AddPlayerMessage(tag);
				title = tag;
				acegiak_bufftag match = bufftags.Where(b=>b.tags.Contains(tag)).OrderBy(b=>Stat.Rnd2.NextDouble()).FirstOrDefault();
				if(match != null){
					this.title = match.title;
					this.className = match.buffclass;
					return;
				}

			}
		}


		public void Save(SerializationWriter Writer){
			Writer.Write(FormName);
			Writer.Write(FormDescription);
			Writer.Write(FormFaction);
			Writer.Write(BaseColor);
			Writer.Write(DetailColor);
			Writer.Write(PartType);
			Writer.Write(title);
			Writer.Write(className);
			Writer.Write(text);
			Writer.Write(secretid);
			Writer.Write(revealed?1:0);
		}

		public static void Read(SerializationReader Reader, acegiak_CustomsPainting skill){
			acegiak_PaintingRecipe recipe = new acegiak_PaintingRecipe("","");
			recipe.FormName = Reader.ReadString();
			recipe.FormDescription = Reader.ReadString();
			recipe.FormFaction = Reader.ReadString();
			recipe.BaseColor = Reader.ReadString();
			recipe.DetailColor = Reader.ReadString();
			recipe.PartType = Reader.ReadString();
			recipe.title = Reader.ReadString();
			recipe.className = Reader.ReadString();
			recipe.text = Reader.ReadString();
			recipe.secretid = Reader.ReadString();
			recipe.revealed = Reader.ReadInt32() >0;
			skill.Recipes.Add(recipe);
		}

	}
}
