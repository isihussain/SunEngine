using System;

namespace SunEngine.Commons.Models.Authorization
{
    /// <summary>
    /// Short token (access token) blacklisted item.
    /// </summary>
    public class BlackListShortToken
    {
        public string TokenId { get; set; }
        public DateTime Expire { get; set; }
    }
}