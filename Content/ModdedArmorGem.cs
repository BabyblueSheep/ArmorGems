using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static ArmorGems.Content.ModdedArmorGemGenerator;

namespace ArmorGems.Content
{
    public class ModdedArmorGem : ModItem
    {
        public ArmorSet armorSet;
        public string header;

        protected override bool CloneNewInstances => true;
        //TODO: remix has a mod preface like "CalamityMod/" before its sticky/bouncy rogue items. add that here, so the mods that add Terra sets arent fucked
        public override string Name => header + " Armor Gem";
        public override string Texture => "ArmorGems/Content/BaseArmorGem";

        public ModdedArmorGem(ArmorSet armorSet, string header)
        {
            this.armorSet = armorSet;
            this.header = header;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine("HelmetTooltip", "Helmets:"));
            foreach (ModItem item in armorSet.helmets)
            {
                tooltips.Add(new TooltipLine(item.Name + "Tooltip", item.Name));
            }
            tooltips.Add(new TooltipLine("ChestplateTooltip", "Chestplates:"));
            foreach (ModItem item in armorSet.chestplates)
            {
                tooltips.Add(new TooltipLine(item.Name + "Tooltip", item.Name));
            }
            tooltips.Add(new TooltipLine("LeggingsTooltip", "Leggings:"));
            foreach (ModItem item in armorSet.leggings)
            {
                tooltips.Add(new TooltipLine(item.Name + "Tooltip", item.Name));
            }
        }
    }
}
