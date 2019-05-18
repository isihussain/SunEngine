using System.IO;
using Newtonsoft.Json.Linq;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Authorization;
using SunEngine.Core.Security;

namespace SunEngine.DataSeed
{
    public class MenuSeeder
    {
        private readonly DataContainer dataContainer;
        private readonly string configDir;

        public MenuSeeder(DataContainer dataContainer, string configDir)
        {
            this.configDir = configDir;
            this.dataContainer = dataContainer;
        }

        public void Seed()
        {
            var files = Directory.GetFiles(configDir);
            foreach (var file in files)
            {
                string jsonText = File.ReadAllText(file);
                JObject jObject = JObject.Parse(jsonText);
                SeedMenuItem(jObject);
            }
        }

        private MenuItem SeedMenuItem(JObject jObject)
        {
            MenuItem menuItem = new MenuItem();

            menuItem.Id = dataContainer.NextMenuItemId();
            menuItem.SortNumber = menuItem.Id;
            
            if (jObject.ContainsKey("Name"))
                menuItem.Name = (string)jObject["Name"];
            
            menuItem.Title = (string)jObject["Title"];
            
            if (jObject.ContainsKey("SubTitle"))
                menuItem.SubTitle = (string)jObject["SubTitle"];
            
            if (jObject.ContainsKey("RouteName"))
                menuItem.RouteName = (string)jObject["RouteName"];
            
            if (jObject.ContainsKey("RouteParams"))
                menuItem.RouteParamsJson = (string)jObject["RouteParams"];
            
            if (jObject.ContainsKey("Exact"))
                menuItem.Exact = (bool)jObject["Exact"];
            
            if (jObject.ContainsKey("IsSeparator"))
                menuItem.IsSeparator = (bool) jObject["IsSeparator"];
            
            if (jObject.ContainsKey("CssClass"))
                menuItem.CssClass = (string) jObject["CssClass"];
            
            if (jObject.ContainsKey("SettingsJson"))
                menuItem.SettingsJson = (string) jObject["Settings"];
            
            if (jObject.ContainsKey("Icon"))
                menuItem.Icon = (string) jObject["Icon"];
            
            if (jObject.ContainsKey("CustomIcon"))
                menuItem.CustomIcon = (string) jObject["CustomIcon"];
            
            if (jObject.ContainsKey("ExternalUrl"))
                menuItem.ExternalUrl = (string) jObject["ExternalUrl"];

            if (jObject.ContainsKey("Roles"))
                menuItem.Roles = (string) jObject["Roles"];
            else
                menuItem.Roles = string.Join(',', RoleNames.Unregistered, RoleNames.Registered); 
            
            dataContainer.MenuItems.Add(menuItem);

            if (jObject.ContainsKey("SubMenuItems"))
            {
                JArray jSubMenuItems = (JArray)jObject["SubMenuItems"];
                foreach (JObject jSubMenuItem in jSubMenuItems)
                {
                    var subMenuItem = SeedMenuItem(jSubMenuItem);
                    subMenuItem.ParentId = menuItem.Id;
                }
            }

            return menuItem;
        }
    }
}