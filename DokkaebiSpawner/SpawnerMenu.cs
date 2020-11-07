using CitizenFX.Core;
using NativeUI;
using static DokkaebiSpawner.GlobalVariables;

public class SpawnerMenu : BaseScript
{
    private MenuPool _menuPool;
    private static UIMenu mainMenu;
    private static UIMenu vehicleSelectorMenu;
    private static UIMenuItem navigateToSelectorMenuItem;
    private static UIMenuCheckboxItem automaticallyEnterVehicle;
    private static UIMenuListItem modelList;
    private static UIMenuItem confirmItem;

    public void AutomaticallyEnterVehicle(UIMenu menu)
    {
        menu.AddItem(automaticallyEnterVehicle);
    }

    public void Confirm(UIMenu menu)
    {
        menu.AddItem(confirmItem);
    }

    public static void OurOnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (sender == mainMenu)
        {
            if (selectedItem == confirmItem)
            {
                string modelname = modelList.IndexToItem(modelList.Index);
                Model vehiclemodel = new Model(modelname);
                bool automaticallyenter = automaticallyEnterVehicle.Checked;
                Vector3 Position;
                Position = Game.PlayerPed.GetOffsetPosition(new Vector3(0f, 1f, 0f));

                Vehicle newVehicle = new Vehicle(vehiclemodel);
            }
        }
    }

    public SpawnerMenu()
    {
        _menuPool = new MenuPool();
        //var mainMenu = new UIMenu("DokkaebiSpawner", "Spawn emergency vehicles with ease");
        _menuPool.Add(mainMenu);

        vehicleSelectorMenu = new UIMenu("Selector Menu", "");
        vehicleSelectorMenu.SetMenuWidthOffset(30);
        _menuPool.Add(vehicleSelectorMenu);

        navigateToSelectorMenuItem = new UIMenuItem("Vehicle Selector Menu");
        mainMenu.AddItem(navigateToSelectorMenuItem);
        mainMenu.BindMenuToItem(vehicleSelectorMenu, navigateToSelectorMenuItem);
        vehicleSelectorMenu.ParentMenu = mainMenu;

        modelList = new UIMenuListItem("Model", policeModelList, 0);
        vehicleSelectorMenu.AddItem(modelList);

        AutomaticallyEnterVehicle(mainMenu);
        Confirm(mainMenu);
        _menuPool.RefreshIndex();
        vehicleSelectorMenu.RefreshIndex();

        mainMenu.OnItemSelect += OurOnItemSelect;

        _menuPool.MouseEdgeEnabled = false;
        _menuPool.ControlDisablingEnabled = false;
        vehicleSelectorMenu.MouseEdgeEnabled = false;
        vehicleSelectorMenu.ControlDisablingEnabled = false;

        Tick += async () =>
        {
            _menuPool.ProcessMenus();
            if (Game.IsControlJustPressed(0, Control.SelectCharacterFranklin) && !_menuPool.IsAnyMenuOpen())
                mainMenu.Visible = !mainMenu.Visible;
        };
    }
}