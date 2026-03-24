using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace ArmorGems;

[AutoloadHead]
internal sealed class EnchanterNPC : ModNPC
{
    internal sealed class EnchanterRespawnSystem : ModSystem
    {
        public static bool CanEnchanterRespawn { get; set; } = false;

        public override void ClearWorld()
        {
            CanEnchanterRespawn = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag[nameof(CanEnchanterRespawn)] = CanEnchanterRespawn;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            CanEnchanterRespawn = tag.GetBool(nameof(CanEnchanterRespawn));
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.WriteFlags(CanEnchanterRespawn);
        }

        public override void NetReceive(BinaryReader reader)
        {
            reader.ReadFlags(out bool canEnchanterRespawnCopy);
            CanEnchanterRespawn = canEnchanterRespawnCopy;
        }
    }

    public int NPCHeadShimmerTextureSlot { get; private set; }

    public override string Texture => "ArmorGems/Assets/EnchanterNPC";

    public override void Load()
    {
        NPCHeadShimmerTextureSlot = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");

        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Gore_Hat");
        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Gore_Hat_Party");
        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Gore_Arm");
        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Gore_Leg");

        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Shimmer_Gore_Hat");
        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Shimmer_Gore_Scrap1");
        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Shimmer_Gore_Scrap2");
        GoreLoader.AddGoreFromTexture<SimpleModGore>(Mod, "ArmorGems/Assets/EnchanterNPC_Shimmer_Gore_Scrap3");
    }

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 23;
        NPCID.Sets.ExtraFramesCount[Type] = 7;
        NPCID.Sets.AttackFrameCount[Type] = 2;

        NPCID.Sets.DangerDetectRange[Type] = 700;
        NPCID.Sets.PrettySafe[Type] = 100;
        NPCID.Sets.AttackType[Type] = 2;
        NPCID.Sets.AttackTime[Type] = 30;
        NPCID.Sets.AttackAverageChance[Type] = 30;

        NPCID.Sets.HatOffsetY[Type] = 2;

        NPCID.Sets.ShimmerTownTransform[Type] = true;

        //NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<ExamplePersonEmote>();

        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new()
        {
            Velocity = 1f,
            PortraitPositionXOverride = 1,
        });

        NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

        NPC.Happiness
            .SetBiomeAffection<HallowBiome>(AffectionLevel.Like)
            .SetBiomeAffection<OceanBiome>(AffectionLevel.Dislike)
            .SetNPCAffection(NPCID.Golfer, AffectionLevel.Love)
            .SetNPCAffection(NPCID.Merchant, AffectionLevel.Like)
            .SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Dislike)
            .SetNPCAffection(NPCID.Cyborg, AffectionLevel.Hate);

        ContentSamples.NpcBestiaryRarityStars[Type] = 2;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange([
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow,
			new FlavorTextBestiaryInfoElement("Mods.ArmorGems.EnchanterNPC.Bestiary"),
        ]);
    }

    public override ITownNPCProfile TownNPCProfile()
    {
        return new Profiles.StackedNPCProfile(
            new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party"),
            new Profiles.DefaultNPCProfile(Texture + "_Shimmer", NPCHeadShimmerTextureSlot, Texture + "_Shimmer_Party")
        );
    }

    public override List<string> SetNPCNameList()
    {
        return [
            "Billy",
            "Gilly",
            "Jilly",
            "Tilly"
        ];
    }

    public override string GetChat()
    {
        var chat = new WeightedRandom<string>();

        chat.Add("peepee");
        chat.Add("poopoo");

        return chat;
    }

    public override void SetChatButtons(ref string button, ref string button2)
    {
        button = "Enchant";
    }

    public override void OnChatButtonClicked(bool firstButton, ref string shop)
    {
        if (firstButton)
        {
            
        }
    }

    public override bool CanGoToStatue(bool toKingStatue) => toKingStatue;

    public override void SetDefaults()
    {
        (NPC.width, NPC.height) = (40, 18);

        NPC.lifeMax = 250;
        NPC.damage = 10;
        NPC.defense = 15;
        NPC.knockBackResist = 0.5f;

        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;

        NPC.aiStyle = NPCAIStyleID.Passive;
        NPC.townNPC = true;
        NPC.friendly = true;

        AnimationType = NPCID.Wizard;
    }

    public override void OnSpawn(IEntitySource source)
    {
        if (source is EntitySource_SpawnNPC)
        {
            EnchanterRespawnSystem.CanEnchanterRespawn = true;
        }
    }

    public override bool CanTownNPCSpawn(int numTownNPCs)
    {
        if (EnchanterRespawnSystem.CanEnchanterRespawn)
            return true;

        foreach (var player in Main.ActivePlayers)
        {
            if (player.armor[0].type > ItemID.None && player.armor[1].type > ItemID.None && player.armor[2].type > ItemID.None)
                return true;
        }

        return false;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        if (Main.netMode == NetmodeID.Server) return;

        if (NPC.life > 0)
        {
            for (int i = 0; i < hit.Damage / NPC.lifeMax * 100.0; i++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hit.HitDirection, -1f);

            return;
        }

        for (int i = 0; i < 50; i++)
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hit.HitDirection, -2.5f);

        var headGore = Mod.Find<ModGore>("EnchanterNPC_Gore_Hat").Type;
        var headPartyGore = Mod.Find<ModGore>("EnchanterNPC_Gore_Hat_Party").Type;
        var armGore = Mod.Find<ModGore>("EnchanterNPC_Gore_Arm").Type;
        var legGore = Mod.Find<ModGore>("EnchanterNPC_Gore_Leg").Type;

        var shimmerHatGore = Mod.Find<ModGore>("EnchanterNPC_Shimmer_Gore_Hat").Type;
        var shimmerScrap1Gore = Mod.Find<ModGore>("EnchanterNPC_Shimmer_Gore_Scrap1").Type;
        var shimmerScrap2Gore = Mod.Find<ModGore>("EnchanterNPC_Shimmer_Gore_Scrap2").Type;
        var shimmerScrap3Gore = Mod.Find<ModGore>("EnchanterNPC_Shimmer_Gore_Scrap3").Type;

        var hatGore = NPC.GetPartyHatGore();
        if (hatGore > 0)
            Gore.NewGore(NPC.position, NPC.velocity, hatGore);
        Gore.NewGore(NPC.position, NPC.velocity, NPC.IsShimmerVariant ? shimmerHatGore : (hatGore > 0 ? headPartyGore : headGore));
        Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y + 20f), NPC.velocity, NPC.IsShimmerVariant ? shimmerScrap1Gore : armGore);
        Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y + 20f), NPC.velocity, NPC.IsShimmerVariant ? shimmerScrap2Gore : armGore);
        Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y + 34f), NPC.velocity, NPC.IsShimmerVariant ? shimmerScrap3Gore : legGore);
        Gore.NewGore(new Vector2(NPC.position.X, NPC.position.Y + 34f), NPC.velocity, NPC.IsShimmerVariant ? shimmerScrap3Gore : legGore);
    }

    public override void TownNPCAttackStrength(ref int damage, ref float knockback)
    {
        damage = 18;
        knockback = 3f;
    }

    public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
    {
        cooldown = 15;
        randExtraCooldown = 15;
    }

    public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
    {
        projType = ProjectileID.BallofFire;
        attackDelay = 15;
    }

    public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
    {
        multiplier = 6f;
        gravityCorrection = 20f;
    }
}