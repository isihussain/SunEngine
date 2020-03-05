﻿export default {
	name: "Articles1",
	title: "Articles with 1 level subcategories",

	setCategoryRoute(category) {
		category.route = {
			name: `cat-${category.name}`,
			params: {}
		};

		for (const cat of category.subCategories) {
			cat.route = {
				name: `cat-${category.name}-cat`,
				params: {
					categoryName: cat.name
				}
			};
		}
	},

	getRoutes(category) {
		const name = category.name;
		const nameLower = name.toLowerCase();

		return [
			{
				name: `cat-${name}`,
				path: "/" + nameLower,
				components: {
					default: sunImport.ArticlesMultiCatPage,
					navigation: sunImport.ArticlesPanel
				},
				props: {
					default: { categoriesNames: nameLower, pageTitle: category.title },
					navigation: { categories: sunImport.Categories1, categoryName: name }
				},
				meta: {
					category: category
				}
			},
			{
				name: `cat-${name}-cat`,
				path: `/${nameLower}/:categoryName`,
				components: {
					default: sunImport.ArticlesPage,
					navigation: sunImport.ArticlesPanel
				},
				props: {
					default: true,
					navigation: { categories: sunImport.Categories1, categoryName: name }
				},
				meta: {
					category: category
				}
			},
			{
				name: `cat-${name}-cat-mat`,
				path: `/${nameLower}/:categoryName/:idOrName`,
				components: {
					default: sunImport.Material,
					navigation: sunImport.ArticlesPanel
				},
				props: {
					default: route => {
						return {
							categoryName: route.params.categoryName,
							idOrName: route.params.idOrName
						};
					},
					navigation: { categories: sunImport.Categories1, categoryName: name }
				},
				meta: {
					category: category
				}
			}
		];
	}
};
