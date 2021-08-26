using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Core.Domain.Entities;

namespace Sunday.Core.Application.Services
{
    /// <summary>
    /// RoleModulePermissionServices
    /// </summary>
    public interface IRoleModulePermissionServices : IBaseServices<RoleModulePermission>
    {
        Task<List<RoleModulePermission>> GetRoleModule();

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