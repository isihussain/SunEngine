using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Controllers
{
	public class CacheAdminController : BaseAdminController
	{
		protected readonly ISectionsCache SectionsCache;
		protected readonly ICategoriesCache categoriesCache;
		protected readonly IMenuCache menuCache;
		protected readonly SpamProtectionCache spamProtectionCache;
		protected readonly IMailTemplatesCache mailTemplatesCache;
		protected readonly IConfigurationRoot configurationRoot;
		protected readonly IDynamicConfigCache dynamicConfigCache;
		protected readonly SanitizerService sanitizerService;

		public CacheAdminController(
			ISectionsCache sectionsCache,
			ICategoriesCache categoriesCache,
			IMenuCache menuCache,
			SpamProtectionCache spamProtectionCache,
			IMailTemplatesCache mailTemplatesCache,
			IConfigurationRoot configurationRoot,
			IDynamicConfigCache dynamicConfigCache,
			SanitizerService sanitizerService,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.SectionsCache = sectionsCache;
			this.categoriesCache = categoriesCache;
			this.menuCache = menuCache;
			this.spamProtectionCache = spamProtectionCache;
			this.mailTemplatesCache = mailTemplatesCache;
			this.configurationRoot = configurationRoot;
			this.dynamicConfigCache = dynamicConfigCache;
			this.sanitizerService = sanitizerService;
		}

		public IActionResult ResetAllCache()
		{
			SectionsCache.Initialize();
			categoriesCache.Initialize();
			menuCache.Initialize();
			rolesCache.Initialize();
			contentCache.Reset();
			spamProtectionCache.Reset();
			mailTemplatesCache.Initialize();
			dynamicConfigCache.Initialize();
			configurationRoot.Reload();
			sanitizerService.Initialize();

			return Ok();
		}
	}
}