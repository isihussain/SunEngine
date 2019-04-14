using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Commons.Cache;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;

namespace SunEngine.Commons.Presenters
{
    public interface IProfilePresenter
    {
        Task<ProfileView> GetProfileAsync(string link, int? viewerUserId);
    }

    public class ProfilePresenter : DbService, IProfilePresenter
    {
        protected readonly IRolesCache RolesCache;
        
        public ProfilePresenter(DataBaseConnection db, IRolesCache rolesCache) : base(db)
        {
            this.RolesCache = rolesCache;
        }
        
        public virtual async Task<ProfileView> GetProfileAsync(string link, int? viewerUserId)
        {
            IQueryable<User> query;
            if (int.TryParse(link, out int id))
                query = db.Users.Where(x => x.Id == id);
            else
                query = db.Users.Where(x => x.Link == link);

            if (viewerUserId.HasValue)
            {
                int adminGroupId = RolesCache.AllRoles["Admin"].Id;

                var user = await query.Select(x =>
                    new ProfileView
                    {
                        Id = x.Id,
                        Name = x.UserName,
                        Information = x.Information,
                        Link = x.Link,
                        Photo = x.Photo,
                        NoBannable = x.Roles.Any(y => y.RoleId == adminGroupId),
                        HeBannedMe = x.BanList.Any(y => y.UserBanedId == viewerUserId.Value),
                    }).FirstOrDefaultAsync();

                user.IBannedHim = await db.Users.Where(y => y.Id == viewerUserId.Value)
                    .AnyAsync(x => x.BanList.Any(y => y.UserBanedId == user.Id));

                return user;
            }

            return await query.Select(x =>
                new ProfileView
                {
                    Id = x.Id,
                    Name = x.UserName,
                    Information = x.Information,
                    Link = x.Link,
                    Photo = x.Photo,
                    NoBannable = true,
                    HeBannedMe = false,
                    IBannedHim = false
                }).FirstOrDefaultAsync();
        }
    }
    
    public class UserInfoView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Avatar { get; set; }
    }

    public class ProfileView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public string Link { get; set; }

        public string Photo { get; set; }

        public bool NoBannable { get; set; }
        public bool HeBannedMe { get; set; }
        public bool IBannedHim { get; set; }

        //public string Avatar { get; set; }
    }
}