using System;
using System.Collections.Generic;
using Wintellect.PowerCollections;
using XRL.Rules;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_ColorPuff : IPart
	{

        public string Color;
        public string GasBlueprint;


        int nCooldown = 0;

		public acegiak_ColorPuff()
		{
		}

		public override void Register(GameObject Object)
		{
			Object.RegisterPartEvent(this, "BeginTakeAction");
			Object.RegisterPartEvent(this, "ApplySpores");
			base.Register(Object);
		}

		public override bool FireEvent(Event E)
		{
			if (E.ID == "BeginTakeAction")
			{

				//ParentObject.UseEnergy(1000, "Color Puff");
				nCooldown--;
				if (nCooldown <= 0 )
				{
					bool flag = false;
					List<Cell> localAdjacentCells = ParentObject.pPhysics.CurrentCell.GetLocalAdjacentCells();
					if (localAdjacentCells != null)
					{
						foreach (Cell item in localAdjacentCells)
						{
							if (item.HasObjectWithPart("Brain"))
							{
								foreach (GameObject item2 in item.GetObjectsWithPart("Brain"))
								{
									
                                flag = true;
                                break;
									
								}
							}
						}
					}
					if (flag)
					{
						ParentObject.ParticleBlip("&W*");
						for (int i = 0; i < localAdjacentCells.Count/4; i++)
						{
							GameObject gameObject = localAdjacentCells[i].AddObject(GasBlueprint);
							Gas part = gameObject.GetPart<Gas>();
							part.ColorString = "&"+this.Color;
							part.Creator = ParentObject;
                            
						}
						nCooldown = 20 + Stat.Random(1, 6);
					}
				}

			}
			else if (E.ID == "ApplySpores")
			{
				return false;
			}
			return base.FireEvent(E);
		}

	}
}
