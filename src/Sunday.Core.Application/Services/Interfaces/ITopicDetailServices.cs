using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Core.Domain.Entities;

namespace Sunday.Core.Application.Services
{
    public interface ITopicDetailServices : IBaseServices<TopicDetail>
    {
        Task<List<TopicDetail>> GetTopicDetails();
    }
}