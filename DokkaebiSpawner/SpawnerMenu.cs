using System.Reflection;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using NativeUI;
using static DokkaebiSpawner.GlobalVariables;

/* DokkaebiSpawner | Developed by xSklzx Dokkaebi */
/* Available via the MIT license */

public class SpawnerMenu : BaseScript
{
    private UIMenu mainMenu;
    private MenuPool _menuPool;
    private UIMenu vehicleSelectorMenu;
    private UIMenuItem navigatetoSelectorMenuItem;
    private UIMenuListItem modelList;
    private UIMenuItem confirmItem;
    private UIMenuCheckboxItem automaticallyEnter;

    private void OurOnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (sender == mainMenu)
        {
            if (selectedItem == confirmItem)
            {
                string modelname = modelList.IndexToItem(modelList.Index);
                Model vehiclemodel = new Model(modelname);

                Vector3 Position;

                Position = Game.PlayerPed.GetOffsetPosition(new Vector3(0f, 5f, 0f));

                var newVehicle = World.CreateVehicle(modelname, Position, Game.PlayerPed.Heading);

                //bool automaticallyEnter

                //if (automaticallyEnter.Checked == true)
                //{
                //    Game.PlayerPed.SetIntoVehicle(newVehicle, VehicleSeat.Driver);
                //}

                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 153, 153 },
                    args = new[] { "[DokkaebiSpawner]", $"Spawned {modelname} successfully." }
                });
            }
        }
    }

    public SpawnerMenu()
    {
        Screen.ShowNotification("DokkaebiSpawner Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " has been initialized.");
        Debug.WriteLine("DokkaebiSpawner Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " has been initialized.");

        _menuPool = new MenuPool();
        mainMenu = new UIMenu("DokkaebiSpawner", "Spawn emergency vehicles with ease");
        _menuPool.Add(mainMenu);

        vehicleSelectorMenu = new UIMenu("Vehicle Selection", "");
        _menuPool.Add(vehicleSelectorMenu);

        navigatetoSelectorMenuItem = new UIMenuItem("Vehicle Selection");
        mainMenu.AddItem(navigatetoSelectorMenuItem);
        mainMenu.BindMenuToItem(vehicleSelectorMenu, navigatetoSelectorMenuItem);
        vehicleSelectorMenu.ParentMenu = mainMenu;

        modelList = new UIMenuListItem("Vehicle", policeModelList, 0);
        vehicleSelectorMenu.AddItem(modelList);

        automaticallyEnter = new UIMenuCheckboxItem("Warp into vehicle?", false);
        mainMenu.AddItem(automaticallyEnter);

        confirmItem = new UIMenuItem("Confirm");
        mainMenu.AddItem(confirmItem);

        mainMenu.RefreshIndex();
        vehicleSelectorMenu.RefreshIndex();
        mainMenu.OnItemSelect += OurOnItemSelect;

        //  Mouse controls disabled
        _menuPool.MouseEdgeEnabled = false;
        _menuPool.ControlDisablingEnabled = false;
        mainMenu.MouseEdgeEnabled = false;
        mainMenu.MouseControlsEnabled = false;
        mainMenu.ControlDisablingEnabled = false;
        vehicleSelectorMenu.MouseEdgeEnabled = false;
        vehicleSelectorMenu.MouseControlsEnabled = false;
        vehicleSelectorMenu.ControlDisablingEnabled = false;

        Tick += async () =>
        {
            _menuPool.ProcessMenus();
            if (Game.IsControlJustPressed(0, Control.SelectCharacterFranklin) && !_menuPool.IsAnyMenuOpen())
                mainMenu.Visible = !mainMenu.Visible;
        };
    }
}