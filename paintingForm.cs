using System;
using XRL.World.Skills.Cooking;
using XRL.UI;
using XRL.Core;

namespace Qud.API
{
	[Serializable]
	public class acegiak_PaintingRecipe: JournalObservation
	{
		public string FormName;
        public string FormDescription;

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
	}
}
