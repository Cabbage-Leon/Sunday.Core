using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Core.Domain.Entities;
using Sunday.Core.Infrastructure.Attritube;
using Sunday.Core.Persistence.Repositories.Base;

namespace Sunday.Core.Application.Services
{
    public class TopicServices : BaseServices<Topic>, ITopicServices
    {
        private IBaseRepository<Topic> _dal;

        public TopicServices(IBaseRepository<Topic> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        /// <summary>
        /// 获取开Bug专题分类（缓存）
        /// </summary>
        /// <returns></returns>
        [Caching(AbsoluteExpiration = 60)]
        public async Task<List<Topic>> GetTopics()
        {
            return await base.Query(a => !a.tIsDelete && a.tSectendDetail == "tbug");
        }
    }
}