const variables = {

  // ----- Auto change values -----
  // auto-start

  "Materials": {
    "CommentsPageSize": 5,
    "TimeToOwnEditInMinutes": 15,
    "TimeToOwnDeleteInMinutes": 15
  },
  "Global": {
    "SiteName": "SunEngine Demo 1111"
  },
  "Comments": {
    "TimeToOwnDeleteInMinutes": 17,
    "TimeToOwnEditInMinutes": 17
  }
,
 // auto-end

  DbColumnSizes: {
    Categories_Name: 64,
    Categories_Title: 256,
    Categories_Icon: 64,
    Users_UserName: 64,
    Users_Email: 64,
    Users_Link: 32,
    Users_PasswordMinLength: 6,
    Materials_Name: 32,
    Materials_Title: 256,
    Materials_SubTitle: 256,
    Tags_Name: 64,
    Roles_Name: 64,
    Roles_Title: 64,
    OperationKey_Name: 100,
    MenuItems_Name: 32,
    MenuItems_Title: 64,
    MenuItems_SubTitle: 64,
    MenuItems_RouteName: 64,
    MenuItems_CssClass: 64,
    MenuItems_Icon: 64,
    Components_Name: 32,
    Components_Type: 32
  },
  Misc: {
    AdminRoleUsersMaxUsersTake: 40,
    DefaultAvatar: "default-avatar.svg"
  },
};


if (config.SiteUrl.startsWith("http://"))
  config.SiteSchema = "http://";
else if (config.SiteUrl.startsWith("https://"))
  config.SiteSchema = "https://";
else
  throw "SiteUrl in config.js have to start with 'http://' or 'https://'.";


Object.freeze(config);
Object.freeze(variables);


// Add styles skin file
document.writeln(`<link href="${config.Skin}?skinver=2145201108" rel="stylesheet">`);

