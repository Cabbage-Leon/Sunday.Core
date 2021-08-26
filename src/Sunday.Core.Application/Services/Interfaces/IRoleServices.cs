using System.Threading.Tasks;
using Sunday.Core.Domain.Entities;

namespace Sunday.Core.Application.Services
{
    /// <summary>
    /// RoleServices
    /// </summary>
    public interface IRoleServices : IBaseServices<Role>
    {
        Task<Role> SaveRole(string roleName);

        Task<string> GetRoleNameByRid(int rid);
    }
}