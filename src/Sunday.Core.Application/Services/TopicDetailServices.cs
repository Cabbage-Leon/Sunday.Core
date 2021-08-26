using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Core.Domain.Entities;
using Sunday.Core.Infrastructure.Attritube;
using Sunday.Core.Persistence.Repositories.Base;

namespace Sunday.Core.Application.Services
{
    public class TopicDetailServices : BaseServices<TopicDetail>, ITopicDetailServices
    {
        private IBaseRepository<TopicDetail> _dal;

        public TopicDetailServices(IBaseRepository<TopicDetail> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        /// <summary>
        /// 获取开Bug数据（缓存）
        /// </summary>
        /// <returns></returns>
        [Caching(AbsoluteExpiration = 10)]
        public async Task<List<TopicDetail>> GetTopicDetails()
        {
            return await base.Query(a => !a.tdIsDelete && a.tdSectendDetail == "tbug");
        }
    }
}