using Sunday.Core.Domain.Entities;
using Sunday.Core.Persistence.Repositories.Base;

namespace Sunday.Core.Application.Services
{
    public partial class PasswordLibServices : BaseServices<PasswordLib>, IPasswordLibServices
    {
        private IBaseRepository<PasswordLib> _dal;

        public PasswordLibServices(IBaseRepository<PasswordLib> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}