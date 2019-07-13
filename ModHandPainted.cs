using HistoryKit;
using System;
using System.Collections.Generic;
using System.Text;
using XRL.Core;
using XRL.Rules;
using System.Linq;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_ModHandPainted : IModification
	{
		public string Engraving;
        public string BaseColour;
        public string DetailColour;
        public string With;

        public List<List<string>> PaintedParts = new List<List<string>>();

		public bool bLookedAt;



		public acegiak_ModHandPainted()
		{
		}

		public acegiak_ModHandPainted(int Tier)
			: base(Tier)
		{
		}

		public override void Configure()
		{
			WorksOnSelf = true;
		}

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
			Object.RegisterPartEvent(this, "GetDisplayName");
			Object.RegisterPartEvent(this, "GetShortDisplayName");
			Object.RegisterPartEvent(this, "GetShortDescription");
			Object.RegisterPartEvent(this, "GetUnknownShortDescription");
			Object.RegisterPartEvent(this, "Equipped");
			Object.RegisterPartEvent(this, "Unequipped");
			Object.RegisterPartEvent(this, "AfterLookedAt");
			Object.RegisterPartEvent(this, "ApplyLiquidCovered");
			base.Register(Object);
		}

        public override bool Render(RenderEvent E)
		{
			if(PaintedParts.Count() > 0){
                List<string> partstrings = PaintedParts[PaintedParts.Count-1];
                E.ColorString = "&"+partstrings[1];
                E.DetailColor = partstrings[2];
            }
            if(BaseColour != null){
                E.ColorString = "&"+BaseColour;
            }
            if(DetailColour != null){
                E.DetailColor = DetailColour;

            }
			return true;
		}

		public override bool FireEvent(Event E)
		{
        if (E.ID == "GetShortDescription" || E.ID == "GetUnknownShortDescription")
			{

                string str = "";
                if(PaintedParts.Count() > 0){
                    foreach(List<string> partstrings in PaintedParts){
                        BodyPart part = ParentObject.GetPart<Body>()._Body.GetPartByID(Int32.Parse(partstrings[0]));
                        if(part != null){
                            str+= "&C"+ParentObject.Its+" "+part.Name+" is &"+partstrings[1]+"p&"+partstrings[2]+"a&"+partstrings[1]+"i&"+partstrings[2]+"n&"+partstrings[1]+"t&"+partstrings[2]+"e&"+partstrings[1]+"d with "+partstrings[3]+
                            ((partstrings[4] != null && partstrings[4] != "")?" in "+ partstrings[4]:"")
                            +".&y\n";
                        }
                    }
                }else if(BaseColour != null && DetailColour != null && Engraving != null){
				    str = "\n&"+BaseColour+"p&"+DetailColour+"a&"+BaseColour+"i&"+DetailColour+"n&"+BaseColour+"t&"+DetailColour+"e&"+BaseColour+"d with " + Engraving +(With!=null?" in "+With:"") +".&y\n";
                }
				E.SetParameter("Postfix", E.GetStringParameter("Postfix") + str);
			}
			else if ((E.ID == "GetDisplayName" || E.ID == "GetShortDisplayName") && (!ParentObject.Understood() || !ParentObject.HasProperName))
			{
                string str = "";
                if(PaintedParts.Count() > 0){
                    foreach(List<string> partstrings in PaintedParts){
                        BodyPart part = ParentObject.GetPart<Body>()._Body.GetPartByID(Int32.Parse(partstrings[0]));
                        if(part != null){
                            str+= "&"+partstrings[1]+"p&"+partstrings[2]+"a&"+partstrings[1]+"i&"+partstrings[2]+"n&"+partstrings[1]+"t&"+partstrings[2]+"e&"+partstrings[1]+"d &y";
                        }
                    }
                }else if(BaseColour != null && DetailColour != null && Engraving != null){
				    str = "&"+BaseColour+"p&"+DetailColour+"a&"+BaseColour+"i&"+DetailColour+"n&"+BaseColour+"t&"+DetailColour+"e&"+BaseColour+"d &y";
                }
				E.GetParameter<StringBuilder>("Prefix").Append(str);
			}
            if(E.ID == "ApplyLiquidCovered"){
                BaseColour = null;
                DetailColour = null;
                Engraving = null;
                With = null;
                PaintedParts = new List<List<string>>();
            }
			return base.FireEvent(E);
		}
	}
}
