using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ArmorGems;

internal sealed class ArmorGems : Mod
{
    public static ArmorGems Instance { get; private set; }

    public override void Load()
    {
        Instance = this;
    }

    public override void Unload()
    {
        Instance = null;
    }
}