using CitizenFX.Core;
using NativeUI;
using static DokkaebiSpawner.GlobalVariables;

public class SpawnerMenu : BaseScript
{
    private MenuPool _menuPool;
    public void VehicleSelection(UIMenu menu)
    {
        var selectionMenuItem = new UIMenuListItem("Model", policeModelList, 0);
        menu.AddItem(selectionMenuItem);
    }

    public void AutomaticallyEnterVehicle(UIMenu menu)
    {
        var automaticallyEnterVehicle = new UIMenuCheckboxItem("Automatically enter vehicle", false);
        menu.AddItem(automaticallyEnterVehicle);
    }

    public void Confirm(UIMenu menu)
    {
        var confirmCheckbox = new UIMenuCheckboxItem("Confirm", false);
        menu.AddItem(confirmCheckbox);
    }

    public SpawnerMenu()
    {
        _menuPool = new MenuPool();
        var mainMenu = new UIMenu("DokkaebiSpawner", "Spawn emergency vehicles with ease");
        _menuPool.Add(mainMenu);
        VehicleSelection(mainMenu);
        AutomaticallyEnterVehicle(mainMenu);
        Confirm(mainMenu);
        _menuPool.RefreshIndex();

        _menuPool.MouseEdgeEnabled = false;
        _menuPool.ControlDisablingEnabled = false;

        Tick += async () =>
        {
            _menuPool.ProcessMenus();
            if (Game.IsControlJustPressed(0, Control.SelectCharacterFranklin) && !_menuPool.IsAnyMenuOpen())
                mainMenu.Visible = !mainMenu.Visible;
        };
    }
}