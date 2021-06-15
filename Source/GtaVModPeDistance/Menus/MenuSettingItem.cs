namespace GtaVModPeDistance.Menus
{
    public abstract class MenuSettingItem
    {
        protected Settings.SettingToSave settingToSave = Settings.SettingToSave.NONE;
        public abstract void Save();
    }
}
