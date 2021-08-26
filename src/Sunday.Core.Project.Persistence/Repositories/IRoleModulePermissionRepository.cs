using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Core.Domain.Entities;
using Sunday.Core.Persistence.Repositories.Base;

namespace Sunday.Core.Persistence.Repositories
{
    /// <summary>
    /// IRoleModulePermissionRepository
    /// </summary>
    public interface IRoleModulePermissionRepository : IBaseRepository<RoleModulePermission>//类名
    {
        Task<List<TestMuchTableResult>> QueryMuchTable();

        Task<List<RoleModulePermission>> RoleModuleMaps();

        Task<List<RoleModulePermission>> GetRMPMaps();

        /// <summary>
        /// 批量更新菜单与接口的关系
        /// </summary>
        /// <param name="permissionId">菜单主键</param>
        /// <param name="moduleId">接口主键</param>
        /// <returns></returns>
        Task UpdateModuleId(int permissionId, int moduleId);
    }
}