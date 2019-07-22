using System;
using XRL.Rules;
using XRL.World.Parts.Effects;
using XRL.UI;
using System.Collections.Generic;
using System.Linq;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_Opheliad : IPart
	{

        public bool autowrite = false;
        public string writing = null;
		public override void Register(GameObject Object)
		{
			Object.RegisterPartEvent(this, "GetInventoryActions");
			Object.RegisterPartEvent(this, "InvCommandOpheliadWrite");
			Object.RegisterPartEvent(this, "GetShortDescription");
			base.Register(Object);
            if(autowrite){
                MarkovWrite();
                for(int i = 0;i<3;i++){
                    DegradeWriting();
                }
            }
		}

		public override bool FireEvent(Event E)
		{
            IPart.AddPlayerMessage(E.ID);
			if (E.ID == "GetInventoryActions")
			{
				E.GetParameter<EventParameterGetInventoryActions>("Actions").AddAction("OpheliadWrite", 'W', false, "&WW&yrite", "InvCommandOpheliadWrite", 10);
			}
            if(E.ID == "InvCommandOpheliadWrite"){
                //Popup.Show("WHAT");
                // IPart.AddPlayerMessage("Write a memory");
                this.writing = Popup.AskString("What do you write?", "A memory of the dead",999);
                ParentObject.DisplayName = "rusty opheliad";
                ParentObject.pRender.ColorString = "&g";
                ParentObject.pRender.DetailColor = "w";
                ParentObject.GetPart<PreservableItem>().Result = "Brown Dye Phial";

                // IPart.AddPlayerMessage("done");
            }
            if (E.ID == "GetShortDescription")
			{
				if (writing != null)
				{
					string arg = "&wWritten on its petals: "+this.writing+".";
					E.SetParameter("Postfix", E.GetStringParameter("Postfix") + '\n' + arg);
                    DegradeWriting();
				}
			}
			return base.FireEvent(E);
		}


        public void MarkovWrite(){
            writing = ConversationUI.GenerateMarkovMessageSentence();
        }

        public void DegradeWriting(){
            for(int i = 0;i< this.writing.Length; i++){
                if(Stat.Rnd2.NextDouble()<0.05f){
                    string before = this.writing.Substring(0,i);
                    string after = this.writing.Substring(Math.Min(i+1,writing.Length));
                    List<string> degrade = new List<string>{".","-","_",",",""};
                    this.writing = before+degrade.GetRandomElement()+after;
                }
            }
        }
	}
}
