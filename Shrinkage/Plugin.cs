using System;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using Shrinkage.Windows;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using Vector3 = FFXIVClientStructs.FFXIV.Common.Math.Vector3;


namespace Shrinkage;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    
    [PluginService] internal static IPartyList PartyList { get; private set; } = null!;

    [PluginService]
    internal static IFramework Framework { get; private set; } = null!;

    private const string CommandName = "/shrinkage";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("Shrinkage");
    private ConfigWindow ConfigWindow { get; init; }
    public Plugin()
    {
        
        Framework.Update += OnFrameworkUpdate;
        
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        
        ConfigWindow = new ConfigWindow(this);

        WindowSystem.AddWindow(ConfigWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "opens the shrinkage config window"
        });
        
        PluginInterface.UiBuilder.Draw += WindowSystem.Draw;
        
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUi;
        PluginInterface.UiBuilder.OpenMainUi += ToggleConfigUi;
    }

    public void Dispose()
    {
        // Unregister all actions to not leak anything during disposal of plugin
        PluginInterface.UiBuilder.Draw -= WindowSystem.Draw;
        PluginInterface.UiBuilder.OpenConfigUi -= ToggleConfigUi;
        
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();

        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        ConfigWindow.Toggle();
    }
    
    private unsafe void OnFrameworkUpdate(IFramework framework)
    {
        if (PartyList.Length == 0)
        {
            var player = ClientState.LocalPlayer;
            if (player == null) return;

            AdjustScale((Character*)player.Address);
        }
        else
        {
            foreach (var member in PartyList)
            {
                var actor = member.GameObject;
                if (actor == null) continue;
                
                AdjustScale((Character*)actor.Address);
            }
        }
    }

    public unsafe void AdjustScale(Character* actor)
    {
        if (actor == null) return;
        float maxhp = actor->MaxHealth;
        float shield = ((float)actor->ShieldValue / 100f) * maxhp;
        float health = actor->Health + shield;
        float hpRatio = health / maxhp;
        float targetScale = Math.Clamp(hpRatio, Configuration.MinScale, Configuration.MaxScale);

        var draw = actor->DrawObject;

        if (draw != null)
        {
            float scale = draw->Scale.Y;
            scale = float.Lerp(scale, targetScale, Configuration.Speed / 60f);
            draw->Scale = new Vector3(scale, scale, scale);

            if (Configuration.AdjustAnimScale)
            {
                actor->Scale = scale;
            }
            else
            {
                actor->Scale = 1.0f;
            }
        }
    }
    
    public void ToggleConfigUi() => ConfigWindow.Toggle();
}
