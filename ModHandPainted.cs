using HistoryKit;
using System;
using System.Collections.Generic;
using System.Text;
using XRL.Core;
using XRL.Rules;
using System.Linq;

namespace XRL.World.Parts.Effects
{
	[Serializable]
	public class acegiak_ModHandPainted : Effect
	{
		public string Engraving;
        public string BaseColour;
        public string DetailColour;
        public string With;
		public string Faction;

        public int? BodyPartId;

		public acegiak_ModHandPainted()
		{
            this.DisplayName = "painted";
            this.Duration = 1200*10;
		}


		public override bool SameAs(Effect e)
		{
			acegiak_ModHandPainted painted = e as acegiak_ModHandPainted;
            if(this.BodyPartId != null && painted.BodyPartId != null){
                return this.BodyPartId == painted.BodyPartId;
            }

			return base.SameAs(e);
		}

		public override void Register(GameObject Object)
		{
			Object.RegisterEffectEvent(this, "GetDisplayName");
			Object.RegisterEffectEvent(this, "GetShortDisplayName");
			Object.RegisterEffectEvent(this, "GetShortDescription");
			Object.RegisterEffectEvent(this, "GetUnknownShortDescription");
			Object.RegisterEffectEvent(this, "ApplyLiquidCovered");
			base.Register(Object);
		}
		public override void Unregister(GameObject Object)
		{
			Object.UnregisterEffectEvent(this, "GetDisplayName");
			Object.UnregisterEffectEvent(this, "GetShortDisplayName");
			Object.UnregisterEffectEvent(this, "GetShortDescription");
			Object.UnregisterEffectEvent(this, "GetUnknownShortDescription");
			Object.UnregisterEffectEvent(this, "ApplyLiquidCovered");
			base.Unregister(Object);
		}

        public override bool Render(RenderEvent E)
		{
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

                string str = "\n";
                if(this.BodyPartId != null){
                    BodyPart part = Object.GetPart<Body>()._Body.GetPartByID(BodyPartId.Value);
                    if(part != null){
                        str+= "&C"+Object.Its+" "+part.Name+" is ";
                    }
                }
                if(BaseColour != null && DetailColour != null && Engraving != null){
				    str += "&"+BaseColour+"p&"+DetailColour+"a&"+BaseColour+"i&"+DetailColour+"n&"+BaseColour+"t&"+DetailColour+"e&"+BaseColour+"d with " + Engraving +(With!=null?", depicted in "+With:"") +".&y\n";
                }
				E.SetParameter("Postfix", E.GetStringParameter("Postfix") + str);
			}
			else if ((E.ID == "GetDisplayName" || E.ID == "GetShortDisplayName") && (!Object.Understood() || !Object.HasProperName))
			{
                string str = "";
                if(BaseColour != null && DetailColour != null){
				    str = "&"+BaseColour+"p&"+DetailColour+"a&"+BaseColour+"i&"+DetailColour+"n&"+BaseColour+"t&"+DetailColour+"e&"+BaseColour+"d &y";
                }
				E.GetParameter<StringBuilder>("Prefix").Append(str);
			}
            if(E.ID == "ApplyLiquidCovered"){
                // BaseColour = null;
                // DetailColour = null;
                // Engraving = null;
                // With = null;
                // BodyPartId = null;
                // Object.RemoveEffect(this);
                this.Duration -= 1200;
            }
			return base.FireEvent(E);
		}

		public override string GetDetails()
		{
            string str = "";
            if(this.BodyPartId != null){
                BodyPart part = Object.GetPart<Body>()._Body.GetPartByID(BodyPartId.Value);
                if(part != null){
                    str+= Object.Its+" "+part.Name+" is ";
                }
            }
			str = str+"&"+BaseColour+"p&"+DetailColour+"a&"+BaseColour+"i&"+DetailColour+"n&"+BaseColour+"t&"+DetailColour+"e&"+BaseColour+"d with " + Engraving +(With!=null?", depicted in "+With:"") +".&y";
			if(this.Faction != null){
				str += "\n+10 Rep with "+this.Faction;
			}
			return str;
		}


		public override bool Apply(GameObject Object)
		{
			if(this.Faction != null){
				XRLCore.Core.Game.PlayerReputation.modify(this.Faction, 10);
			}
			return true;
		}

		public override void Remove(GameObject Object)
		{	
			if(this.Faction != null){
				XRLCore.Core.Game.PlayerReputation.modify(this.Faction, -10);
			}
		}

	}
}
