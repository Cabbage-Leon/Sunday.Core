using Sunday.Core.Domain.IDS4DbModels;
using Sunday.Core.Persistence.Repositories.Base;

namespace Sunday.Core.Application.Services
{
    public class ApplicationUserServices : BaseServices<ApplicationUser>, IApplicationUserServices
    {
        private IBaseRepository<ApplicationUser> _dal;

        public ApplicationUserServices(IBaseRepository<ApplicationUser> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}