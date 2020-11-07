using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace DokkaebiSpawner
{
    public class Spawner : BaseScript
    {
        public Spawner()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;

            RegisterCommand("car", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                var model = "adder";
                if (args.Count > 0)
                {
                    model = args[0].ToString();
                }

                var hash = (uint)GetHashKey(model);
                if (!IsModelInCdimage(hash) || !IsModelAVehicle(hash))
                {
                    TriggerEvent("chat:AddMessage", new
                    {
                        color = new[] {255, 0, 0},
                        args = new[] {"[DokkaebiSpawner]", $"Spawning {model}."}
                    });
                    return;
                }

                var vehicle = await World.CreateVehicle(model, Game.PlayerPed.Position, Game.PlayerPed.Heading);

                Game.PlayerPed.SetIntoVehicle(vehicle, VehicleSeat.Driver);

                TriggerEvent("chat:AddMessage", new
                {
                    color = new[] {255, 0, 0},
                    args = new[] {"[DokkaebiSpawner]", $"Spawned {model} successfully."}
                });
            }), false);
        }
    }
}