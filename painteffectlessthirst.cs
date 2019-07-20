using System;
using System.Text;
using XRL.Rules;

namespace XRL.World.Parts.Effects
{
	[Serializable]
	public class acegiak_PaintEffectLessThirst : acegiak_ModHandPainted
	{


		public acegiak_PaintEffectLessThirst():base()
		{

		}

		

		public override string GetDetails()
		{
			return base.GetDetails()+"\nYou thirst at half rate.";
		}



	    public override void Register(GameObject Object)
		{
			Object.RegisterEffectEvent(this, "CalculatingThirst");
			base.Register(Object);
		}

		public override void Unregister(GameObject Object)
		{
			Object.UnregisterEffectEvent(this, "CalculatingThirst");
			base.Unregister(Object);
		}


        public override bool FireEvent(Event E)
		{
			if (E.ID == "CalculatingThirst")
			{
				int num = (int)Math.Ceiling((float)E.GetIntParameter("Amount") * 0.5f);
				if (num < 1)
				{
					num = 1;
				}
				E.SetParameter("Amount", num);
			}
            return base.FireEvent(E);
		}



	}
	}