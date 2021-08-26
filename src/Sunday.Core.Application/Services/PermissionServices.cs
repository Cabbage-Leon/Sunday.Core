using Sunday.Core.Domain.Entities;
using Sunday.Core.Persistence.Repositories.Base;

namespace Sunday.Core.Application.Services
{
    /// <summary>
    /// PermissionServices
    /// </summary>
    public class PermissionServices : BaseServices<Permission>, IPermissionServices
    {
        private IBaseRepository<Permission> _dal;

        public PermissionServices(IBaseRepository<Permission> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}