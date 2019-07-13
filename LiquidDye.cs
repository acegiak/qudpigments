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
	internal class acegiak_LiquidDye : BaseLiquid
	{
		public new const int BaseID = 140;

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
            {"o","orange"},
            {"W","yellow"},
            {"w","brown"},
            {"G","lime"},
            {"g","green"},
            {"b","blue"},
            {"B","cerulean"},
            {"c","teal"},
            {"C","turquoise"},
            {"M","magenta"},
            {"m","violet"}
        };



		public acegiak_LiquidDye()
			: base()
		{
            this.Color = ColorNames.Keys.ToList().GetRandomElement();
		}

        public acegiak_LiquidDye(string color)
			: base()
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
			return "&"+Color+""+ColorNames[Color]+" dye";
		}

		public override bool Drank(LiquidVolume Liquid, int Volume, XRL.World.GameObject Target, StringBuilder Message, ref bool ExitInterface)
		{
			Message.Append("It tastes "+ColorNames[Color]+".");
			Target.pRender.DetailColor = this.Color;
			return true;
		}

		public override string GetSmearedName(LiquidVolume Liquid)
		{
			return "&"+Color+""+ColorNames[Color]+" stained";
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

		public override void RenderSmearPrimary(LiquidVolume Liquid, RenderEvent eRender)
		{
			int num = XRLCore.CurrentFrame % 60;
			if (num > 5 && num < 15)
			{
				eRender.ColorString = "&"+Color;
			}
			base.RenderSmearPrimary(Liquid, eRender);
		}

		public override string GetAdjective(LiquidVolume Liquid)
		{
			if (Liquid == null || Liquid.ComponentLiquids[Convert.ToByte(ID)] > 0)
			{
				return "&"+Color+""+ColorNames[Color]+"";
			}
			return null;
		}

		public override void RenderBackground(LiquidVolume Liquid, RenderEvent eRender)
		{
			eRender.ColorString = "^k" + eRender.ColorString;
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

		public override bool PouredOn(LiquidVolume Liquid, XRL.World.GameObject GO)
		{
			// if (Liquid.ComponentLiquids.Count != 1)
			// {
			// 	return true;
			// }
			if (Liquid.Volume > 0)
			{
				GO.pRender.TileColor="&"+this.Color;
				GO.pRender.ColorString = "&"+this.Color;
				GO.pRender.DetailColor =ToggleCase(this.Color);


			}
			return true;
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
	}
}
