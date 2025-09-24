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
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
