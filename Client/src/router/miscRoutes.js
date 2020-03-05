
const routes = [
	{
		name: "CreateMaterial",
		path:
			"/CreateMaterial/".toLowerCase() + ":categoriesNames/:initialCategoryName?",
		components: {
			default: sunImport.CreateMaterial
		},
		props: {
			default: true
		},
		meta: {
			roles: ["Registered"]
		}
	},
	{
		name: "EditMaterial",
		path: "/EditMaterial/".toLowerCase() + ":id",
		components: {
			default: sunImport.EditMaterial
		},
		props: {
			default: route => {
				return {
					id: +route.params.id
				};
			},
			navigation: null
		},
		meta: {
			roles: ["Registered"]
		}
	},
	{
		name: "SendPrivateMessage",
		path: "/SendPrivateMessage".toLowerCase(),
		components: {
			default: sunImport.SendPrivateMessage
		},
		props: {
			default: route => {
				return {
					userId: route.query.userId,
					userName: route.query.userName,
					userLink: route.query.userLink
				};
			}
		}
	},
	{
		name: "User",
		path: "/user/:link",
		components: {
			default: sunImport.Profile
		},
		props: {
			default: true
		}
	}
];

export default routes;
