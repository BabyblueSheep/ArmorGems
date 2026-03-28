using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArmorGems;

//Moves Chlorophyte clearing Leaf Crystal outside UpdateArmorSets to not make Armor Gems remove it.
//Also moves Solar Armor resetting its counter earlier.
internal sealed class SpecificArmorFixes : ILoadable
{
    public void Load(Mod mod)
    {
        IL_Player.UpdateArmorSets += RemoveLeafCrystalClear;
        IL_Player.UpdateArmorSets += RemoveSolarCounterClear;
        IL_Player.Update += AddLeafCrystalClear;
    }

    public void Unload()
    {
        IL_Player.UpdateArmorSets -= RemoveLeafCrystalClear;
        IL_Player.UpdateArmorSets -= RemoveSolarCounterClear;
        IL_Player.Update -= AddLeafCrystalClear;
    }

    private void RemoveLeafCrystalClear(ILContext il)
    {
        var cursor = new ILCursor(il);

        cursor.GotoNext(i => i.MatchCall<Player>(nameof(Player.DelBuff))); //Goes near code that removes Beetle Might. Not what we need.
        cursor.GotoNext(i => i.MatchCall<Player>(nameof(Player.DelBuff))); //Goes near code that removes Beetle Endurance. Not what we need.
        cursor.GotoNext(i => i.MatchCall<Player>(nameof(Player.DelBuff))); //Goes near code that removes Leaf Crystal. What we need.

        cursor.EmitPop();
        cursor.EmitPop(); //Pop the player and buff index from the stack.
        cursor.Remove(); //Remove the DelBuff call instruction to prevent breakage.
    }

    private void RemoveSolarCounterClear(ILContext il)
    {
        var cursor = new ILCursor(il);

        cursor.GotoNext(i => i.MatchStfld<Player>(nameof(Player.solarCounter)));
        cursor.GotoNext(i => i.MatchStfld<Player>(nameof(Player.solarCounter)));
        cursor.GotoNext(i => i.MatchStfld<Player>(nameof(Player.solarCounter)));
        cursor.GotoNext(i => i.MatchStfld<Player>(nameof(Player.solarCounter))); //Goes near code that resets solarCounter if Solar armor isn't equipped.

        cursor.EmitPop();
        cursor.EmitPop(); //Pop the player and the number zero from the stack.
        cursor.Remove(); //Remove the field setting to prevent breakage.
    }

    private void AddLeafCrystalClear(ILContext il)
    {
        var cursor = new ILCursor(il);

        cursor.GotoNext(i => i.MatchCall<Player>(nameof(Player.ResetVisibleAccessories))); //Goes near a call to ResetVisibleAccessories. A bit before accessories and armor get updated.

        cursor.EmitLdarg0(); //Emit the player object.
        cursor.EmitDelegate((Player player) =>
        {
            var isWearingChlorophyte = (player.head == ArmorIDs.Head.ChlorophyteMask || player.head == ArmorIDs.Head.ChlorophyteHelmet || player.head == ArmorIDs.Head.ChlorophyteHeadgear)
                && player.body == ArmorIDs.Body.ChlorophytePlateMail && player.legs == ArmorIDs.Legs.ChlorophyteGreaves;

            isWearingChlorophyte = isWearingChlorophyte || player.GetModPlayer<ArmorGemSpecificFixesModPlayer>().HasChlorophyte;
            if (!isWearingChlorophyte)
            {
                player.ClearBuff(BuffID.LeafCrystal);
            }

            var isWearingSolar = player.head == ArmorIDs.Head.SolarFlareHelmet && player.body == ArmorIDs.Body.SolarFlareBreastplate && player.legs == ArmorIDs.Legs.SolarFlareLeggings;
            isWearingSolar = isWearingSolar || player.GetModPlayer<ArmorGemSpecificFixesModPlayer>().HasSolar;
            if (!isWearingSolar)
            {
                player.solarCounter = 0;
            }

            player.GetModPlayer<ArmorGemSpecificFixesModPlayer>().HasChlorophyte = false;
            player.GetModPlayer<ArmorGemSpecificFixesModPlayer>().HasSolar = false;
        });
    }
}

internal sealed class ArmorGemSpecificFixesModPlayer : ModPlayer
{
    public bool HasChlorophyte { get; set; }
    public bool HasSolar { get; set; }
}