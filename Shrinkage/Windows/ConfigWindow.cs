using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;

namespace Shrinkage.Windows;

public class ConfigWindow : Window, IDisposable
{
    private readonly Configuration configuration;
    
    public ConfigWindow(Plugin plugin) : base("Shrinkage Config")
    {

        Size = new Vector2(350, 180);
        SizeCondition = ImGuiCond.Always;

        configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        var speed = configuration.Speed;
        var maxScale = configuration.MaxScale;
        var minScale = configuration.MinScale;
        var animScale = configuration.AdjustAnimScale;

        if (ImGui.DragFloat("Minimum Size", ref minScale, 0.01F, 0.01F, 3.00F))
        {
            configuration.MinScale = minScale;
            if (configuration.MaxScale < configuration.MinScale){ configuration.MaxScale = configuration.MinScale; }
            configuration.Save();
        }

        if (ImGui.DragFloat("Maximum Size", ref maxScale, 0.01F, 0.01F, 3.00F))
        {
            configuration.MaxScale = maxScale;
            if (configuration.MinScale > configuration.MaxScale){ configuration.MinScale = configuration.MaxScale; }
            configuration.Save();
        }

        if (ImGui.DragFloat("Shrink Speed", ref speed, 0.1F, 0.1F, 100.0F))
        {
            configuration.Speed = speed;
            configuration.Save();
        }

        if (ImGui.Checkbox("Scale camera and animation speed with size", ref animScale))
        {
            configuration.AdjustAnimScale = animScale;
            configuration.Save();
        }
    }
}
