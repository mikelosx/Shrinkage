using Dalamud.Configuration;
using System;

namespace Shrinkage;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;
    
    public bool AdjustAnimScale { get; set; } = true;

    public float Speed { get; set; } = 2.0f;
    public float MinScale { get; set; } = 0.1f;
    public float MaxScale { get; set; } = 10.0f;
    
    public void Save()
    {
        if (MinScale < 0.01f){ MinScale = 0.01f; }
        if (MaxScale > 3.00f){ MaxScale = 3.00f; }
        
        if (MinScale > MaxScale){ MinScale = MaxScale; }
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
