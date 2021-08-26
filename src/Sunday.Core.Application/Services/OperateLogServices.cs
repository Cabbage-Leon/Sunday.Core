using Sunday.Core.Domain.Entities;
using Sunday.Core.Persistence.Repositories.Base;

namespace Sunday.Core.Application.Services
{
    public partial class OperateLogServices : BaseServices<OperateLog>, IOperateLogServices
    {
        private IBaseRepository<OperateLog> _dal;

        public OperateLogServices(IBaseRepository<OperateLog> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}