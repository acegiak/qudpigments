using System;
using XRL.Rules;
using Qud.API;
using XRL.Rules;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_PigmentMerchantHotloader : IPart
	{
        public acegiak_PigmentMerchantHotloader(){
                            AddToPopTable("RandomLiquid", new PopulationTable { Name = "RandomDye" });

        }
        // Helper method to fudge into the most common/simple pop tables.
        public static bool AddToPopTable(string table, params PopulationItem[] items) {
            PopulationInfo info;
            if (!PopulationManager.Populations.TryGetValue(table, out info))
                return false;
                
            // If this is a single group population, add to that group.
            if (info.Items.Count == 1 && info.Items[0] is PopulationGroup) { 
                var group = info.Items[0] as PopulationGroup;
                group.Items.AddRange(items);
                return true;
            }

            info.Items.AddRange(items);
            return true;
        }



    }

}