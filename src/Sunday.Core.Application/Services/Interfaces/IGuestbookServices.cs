using System.Threading.Tasks;
using Sunday.Core.Domain.Entities;
using Sunday.Core.Model;

namespace Sunday.Core.Application.Services
{
    public partial interface IGuestbookServices : IBaseServices<Guestbook>
    {
        Task<MessageModel<string>> TestTranInRepository();

        Task<bool> TestTranInRepositoryAOP();
    }
}