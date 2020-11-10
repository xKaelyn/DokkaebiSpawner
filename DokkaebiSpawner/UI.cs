using System.Linq;
using System.Collections.Generic;
using CitizenFX.Core;
using NativeUI;
using NativeUI.PauseMenu;
using System;

namespace DokkaebiSpawner
{
    public static class Menu
    {
        private static MenuPool menuPool;
        private static UIMenu mainMenu;
        public static string theItemDescription;

        static Menu()
        {
            menuPool = new MenuPool()
            {
                MouseEdgeEnabled = false,
                ControlDisablingEnabled = false
            };
            mainMenu = new UIMenu("Vehicle Spawner", "Categories")
            {
                MouseControlsEnabled = false
            };
            menuPool.Add(mainMenu);
            VehicleSpawnMenu(mainMenu);
            menuPool.RefreshIndex();
        }

        internal static async void Toggle()
        {
            if (menuPool.IsAnyMenuOpen())
            {
                menuPool.CloseAllMenus();
            }
            else
            {
                mainMenu.Visible = true;
                while (menuPool.IsAnyMenuOpen())
                {
                    menuPool.ProcessMenus();
                    await BaseScript.Delay(0);
                }
            }
            BaseScript.TriggerEvent("chat:addMessage", new
            {
                color = new[] { 0, 0, 0 },
                multiline = true,
                args = new[] { "[DokkaebiSpawner]", $"Spawned {theItemDescription} successfully" }
            });
        }

        private static void VehicleSpawnMenu(UIMenu menu)
        {
            foreach (var value in Main.Categories)
            {
                var category = menuPool.AddSubMenu(mainMenu, value);
                category.MouseControlsEnabled = false;
                foreach (KeyValuePair<string, Dictionary<string, string>> kvp in Main.VehicleDatabase)
                {
                    foreach (KeyValuePair<string, string> categories in kvp.Value)
                    {
                        if (categories.Value == value)
                        {
                            var menuItem = new UIMenuItem(kvp.Key.Substring(categories.Value.Length), $"{categories.Key}");
                            menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Car);
                            category.AddItem(menuItem);

                            var enabled = Main.Disabled.Contains(categories.Key);
                            if (enabled)
                            {
                                menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Lock);
                                menuItem.Description = "Out of service";
                            }
                            menuItem.Enabled = !enabled;
                        }
                    }
                }

                category.OnItemSelect += (sender, item, index) =>
                {
                    theItemDescription = String.Copy(item.Description);
                    Main.SpawnVehicle(item.Description);
                    Toggle();
                };
            }
        }
    }
}