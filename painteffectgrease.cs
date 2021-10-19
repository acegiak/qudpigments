using System;
using System.Text;
using XRL.Rules;
using XRL.World.Encounters;
using XRL.UI;
using XRL.World.Parts;

namespace XRL.World.Effects
{
	[Serializable]
	public class acegiak_PaintEffectGrease : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectGrease():base()
		{

		}

		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nChance to return scraps when tinkering.";
		}


	    public override void Register(GameObject Object)
		{
			Object.RegisterEffectEvent(this, "UseEnergy");
			base.Register(Object);
		}

		public override void Unregister(GameObject Object)
		{
			Object.UnregisterEffectEvent(this, "UseEnergy");
			base.Unregister(Object);
		}

		public override bool FireEvent(Event E)
		{
			if (E.ID == "UseEnergy" && E.GetStringParameter("Type").Contains("Skill Tinkering "))
			{
				int n = 4+Math.Max(0,Object.StatMod("Intelligence"));
				if(Stat.Roll("1d"+n.ToString())>3){
					int num3 = Stat.Roll("1d"+n.ToString());
					GameObject gameObject2 = EncounterFactory.Factory.RollOneFromTable("Scrap " + num3);
					if (Object.IsPlayer())
					{
						Popup.Show("&gYou recover " + gameObject2.a + gameObject2.DisplayName + " &gas you work.");
					}
					Object.FireEvent(XRL.World.Event.New("CommandTakeObject", "Object", gameObject2));
				}
				//gameObject2.FireEvent(Event.New("InvCommandDisassemble", "Owner", Object, "Auto", true));
				
					
			}
            return base.FireEvent(E);
		}




	}
}