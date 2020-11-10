using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using NativeUI;

namespace DokkaebiSpawner
{
    public static class Menu
    {
        private static MenuPool menuPool;
        private static UIMenu mainMenu;

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
            Screen.ShowNotification("DokkaebiSpawner Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " has been initialized.");
            Debug.WriteLine("DokkaebiSpawner Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " has been initialized.");
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
                    Main.SpawnVehicle(item.Description);
                    Toggle();
                };
            }
        }
    }
}