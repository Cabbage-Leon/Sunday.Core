using System.Threading.Tasks;
using Sunday.Core.Domain.Entities;

namespace Sunday.Core.Application.Services
{
    /// <summary>
    /// sysUserInfoServices
    /// </summary>
    public interface ISysUserInfoServices : IBaseServices<SysUserInfo>
    {
        Task<SysUserInfo> SaveUserInfo(string loginName, string loginPwd);

        Task<string> GetUserRoleNameStr(string loginName, string loginPwd);
    }
}