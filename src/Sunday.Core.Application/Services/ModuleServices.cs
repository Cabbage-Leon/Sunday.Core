using Sunday.Core.Domain.Entities;
using Sunday.Core.Persistence.Repositories.Base;

namespace Sunday.Core.Application.Services
{
    /// <summary>
    /// ModuleServices
    /// </summary>
    public class ModuleServices : BaseServices<Modules>, IModuleServices
    {
        private IBaseRepository<Modules> _dal;

        public ModuleServices(IBaseRepository<Modules> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}