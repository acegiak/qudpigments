using System;
using System.Collections.Generic;
using System.Text;
using XRL.Core;
using XRL.Messages;
using XRL.Rules;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Parts.Effects;
using UnityEngine;
using XRL.Liquids;
using XRL.World.Tinkering;
using System.Linq;

namespace XRL.World.Parts
{
    public class acegiak_PigmentLiquidInitializer : IPart
    {
        //this code runs early during game boot - the game creates a temporary instance of every object from the blueprints
        public acegiak_PigmentLiquidInitializer()
        {
            Debug.Log("Initializing Pigment Liquids.");
            
            for(int i=0;i < acegiak_LiquidDye.ColorNames.Keys.ToList().Count;i++){
                Debug.Log("dye count: "+i.ToString());

                acegiak_LiquidDye dye = new acegiak_LiquidDye(acegiak_LiquidDye.ColorNames.Keys.ToList()[i]);
                Debug.Log("dye ID: "+acegiak_LiquidDye.BaseID+i);

                dye.ID = Convert.ToByte(acegiak_LiquidDye.BaseID+i);
                //dye.Name = dye.GetName(null);
                Debug.Log((dye.ID).ToString()+": "+acegiak_LiquidDye.ColorNames.Values.ToList()[i]+"dye "+ acegiak_LiquidDye.ColorNames.Keys.ToList()[i]);
                Debug.Log(dye.GetName(null)+" "+dye.Color);
                LiquidVolume.ComponentLiquidTypes.Add(Convert.ToByte(dye.ID), dye);
			    LiquidVolume.ComponentLiquidNameMap.Add(acegiak_LiquidDye.ColorNames.Values.ToList()[i]+"dye", LiquidVolume.ComponentLiquidTypes[Convert.ToByte(dye.ID)]);
            }

			// LiquidVolume.ComponentLiquidTypes.Add(Convert.ToByte(acegiak_LiquidFurlingAgent.ID), new acegiak_LiquidFurlingAgent());
			// LiquidVolume.ComponentLiquidNameMap.Add("furlingagent", LiquidVolume.ComponentLiquidTypes[Convert.ToByte(acegiak_LiquidFurlingAgent.ID)]);


			// LiquidVolume.ComponentLiquidTypes.Add(Convert.ToByte(acegiak_LiquidGrowthAgent.ID), new acegiak_LiquidGrowthAgent());
			// LiquidVolume.ComponentLiquidNameMap.Add("growthagent", LiquidVolume.ComponentLiquidTypes[Convert.ToByte(acegiak_LiquidGrowthAgent.ID)]);

			// LiquidVolume.ComponentLiquidTypes.Add(Convert.ToByte(acegiak_LiquidRestrainingAgent.ID), new acegiak_LiquidRestrainingAgent());
			// LiquidVolume.ComponentLiquidNameMap.Add("restrainingagent", LiquidVolume.ComponentLiquidTypes[Convert.ToByte(acegiak_LiquidRestrainingAgent.ID)]);


			// LiquidVolume.ComponentLiquidTypes.Add(Convert.ToByte(acegiak_LiquidSoothingAgent.ID), new acegiak_LiquidSoothingAgent());
			// LiquidVolume.ComponentLiquidNameMap.Add("soothingagent", LiquidVolume.ComponentLiquidTypes[Convert.ToByte(acegiak_LiquidSoothingAgent.ID)]);


			// BitType item = new BitType(113, 'w', "&wwood scrap");
			// BitType.BitTypes.Add(item);
            // BitType.BitMap.Add(item.Color, item);
            // if (!BitType.LevelMap.ContainsKey(item.Level))
            // {
            //     BitType.LevelMap.Add(item.Level, new List<BitType>());
            // }
            // BitType.LevelMap[item.Level].Add(item);
            // BitType.BitSortOrder.Add('w',133);
			
        }
    }
}