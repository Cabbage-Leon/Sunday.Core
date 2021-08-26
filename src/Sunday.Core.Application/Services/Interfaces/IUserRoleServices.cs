using System.Threading.Tasks;
using Sunday.Core.Domain.Entities;

namespace Sunday.Core.Application.Services
{
    /// <summary>
    /// UserRoleServices
    /// </summary>
    public interface IUserRoleServices : IBaseServices<UserRole>
    {
        Task<UserRole> SaveUserRole(int uid, int rid);

        Task<int> GetRoleIdByUid(int uid);
    }
}