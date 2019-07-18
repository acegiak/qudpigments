using System;
using System.Text;
using XRL.Rules;
using XRL.World.Parts;
using Qud.API;
using XRL.UI;
using XRL.Core;

namespace XRL.World.Parts.Effects
{
	[Serializable]
	public class acegiak_PaintEffectWhispers : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectWhispers():base()
		{

		}

		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nGrants strange insights into the meanings under words.";
		}

        public override bool FireEvent(Event E){
            if (E.ID == "InvCommandRead")
			{
                GameObject o = E.GetGameObjectParameter("Object");
                if(o == null){
                    Popup.Show("What are you reading??");
                }
				if(o.GetPart<MarkovBook>() != null && !XRLCore.Core.Game.HasStringGameState("AlreadyRead_" + o.GetPart<MarkovBook>().BookSeed)){
                    JournalAPI.RevealRandomSecret();

                }
				return true;
			}

            return base.FireEvent(E);
        }

		



	}
	}