﻿
export default {
	name: "Articles",
	title: "Articles",

	setCategoryRoute(category) {
		category.route = {
			name: `cat-${category.name}`,
			params: {}
		};
	},

	getRoutes(category) {
		const name = category.name;
		const nameLower = name.toLowerCase();
		const pathBegin = "/" + category.urlPath;

		return [
			{
				name: `cat-${name}`,
				path: pathBegin,
				components: {
					default: sunImport.ArticlesPage
				},
				props: {
					default: {
						categoryName: nameLower
					}
				},
				meta: {
					category: category
				}
			},
			{
				name: `cat-${name}-mat`,
				path: `${pathBegin}/:idOrName`,
				components: {
					default: sunImport.Material
				},
				props: {
					default: route => {
						return {
							categoryName: nameLower,
							idOrName: route.params.idOrName
						};
					}
				},
				meta: {
					category: category
				}
			}
		];
	}
};
