using System;
using System.Collections.Generic;
using System.Text;
using XRL.Core;
using XRL.Messages;
using XRL.Rules;
using XRL.World;
using XRL.World.Parts;
using System.Linq;
using UnityEngine;



namespace XRL.Liquids
{
	[Serializable]
	public class acegiak_LiquidDye : BaseLiquid
	{
		public new const string BaseID = "dye";

		public new const string Name = "dye";

        public string Color;

        public static Dictionary<string,string> ColorNames = new Dictionary<string,string>()
        {
            {"K","charcoal"},
            {"k","black"},
            {"Y","white"},
            {"y","ash"},
            {"r","crimson"},
            {"R","scarlet"},
            {"O","gold"},
            {"o","ochre"},
            {"W","yellow"},
            {"w","brown"},
            {"G","chartreuse"},
            {"g","moss"},
            {"b","cobalt"},
            {"B","cerulean"},
            {"c","teal"},
            {"C","turquoise"},
            {"M","magenta"},
            {"m","violet"}
        };



		public acegiak_LiquidDye()
			: base("dye")
		{
            this.Color = ColorNames.Keys.ToList().GetRandomElement();
		}

        public acegiak_LiquidDye(string color)
			: base(color+"dye")
		{
			
            this.Color = ColorNames.FirstOrDefault(x => x.Value == color).Key;
		}

        public acegiak_LiquidDye(string color, string Name)
			: base(Name)
		{
            this.Color = color;
		}

		public override List<string> GetColors()
		{
			return new List<string>(2)
            {
                Color,
                ToggleCase(Color)
            };
		}

		public override string GetColor()
		{
			return Color;
		}

		public override string GetName(LiquidVolume Liquid)
		{
			return "&"+Color+(Color=="k"?"^K":"")+""+ColorNames[Color]+" dye";
		}

		public override bool Drank(LiquidVolume Liquid, int Volume, XRL.World.GameObject Target, StringBuilder Message, ref bool ExitInterface)
		{
			Message.Append("It tastes "+ColorNames[Color]+".");
			Target.pRender.DetailColor = this.Color;
			return true;
		}

		public override string GetSmearedName(LiquidVolume Liquid)
		{
			return "&"+Color+(Color=="k"?"^K":"")+""+ColorNames[Color]+" stained";
		}

		public override void ObjectEnteredCell(LiquidVolume Liquid, XRL.World.GameObject GO)
		{
			if (Liquid.MaxVolume != -1 || !GO.HasPart("Body") || !GO.PhaseAndFlightMatches(Liquid.ParentObject) || Liquid.Volume > 1000)
			{
				return;
			}
			int num = 10 + (int)((double)(Liquid.ComponentLiquids[this.ID] * 5) / 1000.0);
			if (!GO.MakeSave("Agility", num , null, null, "Dye"))
			{
				if (GO.IsPlayer())
				{
					MessageQueue.AddPlayerMessage("&CYou get covered in "+ColorNames[this.Color]+" dye!");
				}
				GO.pRender.TileColor="&"+this.Color;
				GO.pRender.ColorString = "&"+this.Color;
				GO.pRender.DetailColor =ToggleCase(this.Color);
			}
		}

		public override string GetAdjective(LiquidVolume Liquid)
		{
			if (Liquid == null || Liquid.ComponentLiquids[ID] > 0)
			{
				return "&"+Color+(Color=="k"?"^K":"")+""+ColorNames[Color]+"";
			}
			return null;
		}

		public override void RenderPrimary(LiquidVolume Liquid, RenderEvent eRender)
		{
			if (Liquid.ParentObject.IsFreezing())
			{
				eRender.RenderString = "~";
				eRender.ColorString = "&"+Color+"^"+ToggleCase(Color);
				return;
			}
			Render pRender = Liquid.ParentObject.pRender;
			int num = (XRLCore.CurrentFrame + Liquid.nFrameOffset) % 60;
			if (Stat.RandomCosmetic(1, 600) == 1)
			{
				eRender.RenderString = "~";
				eRender.ColorString = "&"+Color+"^"+ToggleCase(Color);
			}
			if (Stat.RandomCosmetic(1, 60) == 1 || pRender.ColorString == "&"+ToggleCase(Color))
			{
				if (num < 15)
				{
					pRender.RenderString = string.Empty + '÷';
				eRender.ColorString = "&"+Color+"^"+ToggleCase(Color);
				}
				else if (num < 30)
				{
					pRender.RenderString = "~";
				    eRender.ColorString = "&"+Color+"^"+ToggleCase(Color);
				}
				else if (num < 45)
				{
					pRender.RenderString = " ";
				    eRender.ColorString = "&"+Color+"^"+ToggleCase(Color);
				}
				else
				{
					pRender.RenderString = "~";
				    eRender.ColorString = "&"+Color+"^"+ToggleCase(Color);
				}
			}
		}

		public override void RenderSecondary(LiquidVolume Liquid, RenderEvent eRender)
		{
			eRender.ColorString += "&"+Color;
		}

		public override void SmearOn(LiquidVolume Liquid, XRL.World.GameObject GO, XRL.World.GameObject By, bool FromCell)
		{
			if (Liquid.Volume > 0)
			{
				GO.pRender.TileColor   = "&"+this.Color;
				GO.pRender.ColorString = "&"+this.Color;
				GO.pRender.DetailColor = ToggleCase(this.Color);
			}
			base.SmearOn(Liquid,GO,By,FromCell);
		}


		public override void SmearOnTick(LiquidVolume Liquid, XRL.World.GameObject GO, XRL.World.GameObject By, bool FromCell)
		{
			if (Liquid.Volume > 0)
			{
				GO.pRender.TileColor   = "&"+this.Color;
				GO.pRender.ColorString = "&"+this.Color;
				GO.pRender.DetailColor = ToggleCase(this.Color);
			}
			base.SmearOnTick(Liquid,GO,By,FromCell);
		}


        public string ToggleCase(string input)
        {
            string result = string.Empty;
            char[] inputArray = input.ToCharArray();
            foreach (char c in inputArray)
            {
                if (char.IsLower(c))
                    result += Char.ToUpper(c);
                else
                    result += Char.ToLower(c);
            }
            return result;
        }

		public override float GetValuePerDram()
		{
			return 5f;
		}

	}

	[Serializable]
	[IsLiquid]
	public class acegiak_CharcoalDye : acegiak_LiquidDye
	{	public acegiak_CharcoalDye(): base("charcoal"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_blackDye : acegiak_LiquidDye
	{	public acegiak_blackDye(): base("black"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_whiteDye : acegiak_LiquidDye
	{	public acegiak_whiteDye(): base("white"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_ashDye : acegiak_LiquidDye
	{	public acegiak_ashDye(): base("ash"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_crimsonDye : acegiak_LiquidDye
	{	public acegiak_crimsonDye(): base("crimson"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_scarletDye : acegiak_LiquidDye
	{	public acegiak_scarletDye(): base("scarlet"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_goldDye : acegiak_LiquidDye
	{	public acegiak_goldDye(): base("gold"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_ochreDye : acegiak_LiquidDye
	{	public acegiak_ochreDye(): base("ochre"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_yellowDye : acegiak_LiquidDye
	{	public acegiak_yellowDye(): base("yellow"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_brownDye : acegiak_LiquidDye
	{	public acegiak_brownDye(): base("brown"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_chartreuseDye : acegiak_LiquidDye
	{	public acegiak_chartreuseDye(): base("chartreuse"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_mossDye : acegiak_LiquidDye
	{	public acegiak_mossDye(): base("moss"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_cobaltDye : acegiak_LiquidDye
	{	public acegiak_cobaltDye(): base("cobalt"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_ceruleanDye : acegiak_LiquidDye
	{	public acegiak_ceruleanDye(): base("cerulean"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_tealDye : acegiak_LiquidDye
	{	public acegiak_tealDye(): base("teal"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_turquoiseDye : acegiak_LiquidDye
	{	public acegiak_turquoiseDye(): base("turquoise"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_magentaDye : acegiak_LiquidDye
	{	public acegiak_magentaDye(): base("magenta"){}	}

	[Serializable]
	[IsLiquid]
	public class acegiak_violetDye : acegiak_LiquidDye
	{	public acegiak_violetDye(): base("violet"){}	}
}
