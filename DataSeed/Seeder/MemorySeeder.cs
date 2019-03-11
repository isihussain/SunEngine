using System;
using System.IO;
using System.Linq;
using NJsonSchema;
using SunEngine.Models;
using SunEngine.Models.Authorization;
using SunEngine.Security.Authorization;
using SunEngine.Utils;

namespace DataSeedDev.Seeder
{
    public class MemorySeeder
    {
        private readonly DataContainer dataContainer;

        private readonly MaterialsSeeder materialsSeeder;

        private readonly UsersSeederJson usersSeederJson;


        private readonly string configDir;

        public MemorySeeder(string configDir)
        {
            this.configDir = configDir;
            dataContainer = new DataContainer();
            materialsSeeder = new MaterialsSeeder(dataContainer);
            usersSeederJson = new UsersSeederJson(dataContainer, configDir);
        }

        public DataContainer Seed()
        {
            StartConsoleLog();

            SeedOperationKeys();

            SeedSectionTypes();

            SeedUsers();

            SeedCategories();

            SeedRoles();

            SeedUserRoles();


            return dataContainer;
        }

        private void StartConsoleLog()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Data seed in memory:");
            Console.ResetColor();
        }

        private void SeedUsers()
        {
            Console.WriteLine("Users");

            usersSeederJson.SeedUsers();
        }

        private void SeedUserRoles()
        {
            Console.WriteLine("UsersRoles");

            usersSeederJson.SeedUserRoles();
        }

        private void SeedSectionTypes()
        {
            Console.WriteLine("SectionTypes");

            SectionType sectionTypeArticles = new SectionType
            {
                Id = dataContainer.NextSectionTypeId(),
                Name = "Articles",
                Title = "Статьи"
            };
            dataContainer.SectionTypes.Add(sectionTypeArticles);

            SectionType sectionTypeForum = new SectionType
            {
                Id = dataContainer.NextSectionTypeId(),
                Name = "Forum",
                Title = "Форум"
            };
            dataContainer.SectionTypes.Add(sectionTypeForum);

            SectionType sectionTypeBlog = new SectionType
            {
                Id = dataContainer.NextSectionTypeId(),
                Name = "Blog",
                Title = "Блог"
            };
            dataContainer.SectionTypes.Add(sectionTypeBlog);
        }


        private void SeedRoles()
        {
            Console.WriteLine("Roles");

            string pathToUserGroupsConfig = Path.GetFullPath(configDir + "/UserGroups.json");
            string pathToUserGroupsSchema = Path.GetFullPath(configDir + "/UserGroups.schema.json");
            JsonSchema4 schema = JsonSchema4.FromFileAsync(pathToUserGroupsSchema).GetAwaiter().GetResult();


            RolesFromJsonLoader fromJsonLoader =
                new RolesFromJsonLoader(dataContainer.Categories.ToDictionary(x => x.Name),
                    dataContainer.OperationKeys.ToDictionary(x => x.Name), schema);

            var json = File.ReadAllText(pathToUserGroupsConfig);

            fromJsonLoader.Seed(json);

            dataContainer.Roles = fromJsonLoader.roles;
            dataContainer.CategoryAccesses = fromJsonLoader.categoryAccesses;
            dataContainer.CategoryOperationAccesses = fromJsonLoader.categoryOperationAccesses;
        }

        private void SeedOperationKeys()
        {
            Console.WriteLine("OperationKeys");

            var keys = OperationKeysContainer.GetAllOperationKeys();

            foreach (var key in keys)
            {
                var operationKey = new OperationKey
                {
                    OperationKeyId = dataContainer.NextOperationKeyId(),
                    Name = key
                };

                dataContainer.OperationKeys.Add(operationKey);
            }
        }


        private void SeedCategories()
        {
            Console.WriteLine("Categories");

            SeedRootCategory();
            SeedCategoriesFromDirectory();
            DetectCategoriesParents();
        }

        private void DetectCategoriesParents()
        {
            foreach (var category in dataContainer.Categories)
            {
                if (category.ParentId.HasValue)
                    category.Parent = dataContainer.Categories.FirstOrDefault(x => x.Id == category.ParentId.Value);
            }
        }

        private void SeedRootCategory()
        {
            int id = dataContainer.NextCategoryId();
            Category rootCategory = new Category
            {
                Id = id,
                Name = Category.RootName,
                NameNormalized = Normalizer.Normalize(Category.RootName),
                Title = "Корень",
                SortNumber = id
            };
            dataContainer.RootCategory = rootCategory;
            dataContainer.Categories.Add(rootCategory);
        }

        private void SeedCategoriesFromDirectory()
        {
            var fileNames = Directory.GetFiles(Path.GetFullPath(configDir + "/CategoriesStartConfig"));

            CategoriesSeederJson categoriesSeederJson =
                new CategoriesSeederJson(dataContainer, materialsSeeder);
            foreach (var fileName in fileNames)
            {
                categoriesSeederJson.Seed(fileName);
            }
        }
    }
}