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

        if (ImGui.InputFloat("Min Scale", ref minScale))
        {
            configuration.MinScale = minScale;
            configuration.Save();
        }

        if (ImGui.InputFloat("Max Scale", ref maxScale))
        {
            configuration.MaxScale = maxScale;
            configuration.Save();
        }

        if (ImGui.InputFloat("Scale Speed", ref speed))
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
