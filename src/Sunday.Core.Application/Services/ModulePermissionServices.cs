using Sunday.Core.Domain.Entities;
using Sunday.Core.Persistence.Repositories.Base;

namespace Sunday.Core.Application.Services
{
    /// <summary>
    /// ModulePermissionServices
    /// </summary>
    public class ModulePermissionServices : BaseServices<ModulePermission>, IModulePermissionServices
    {
        private IBaseRepository<ModulePermission> _dal;

        public ModulePermissionServices(IBaseRepository<ModulePermission> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}