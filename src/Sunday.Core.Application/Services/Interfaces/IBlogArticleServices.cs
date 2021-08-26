using System.Collections.Generic;
using System.Threading.Tasks;
using Sunday.Core.Domain.Entities;
using Sunday.Core.Model.ViewModels;

namespace Sunday.Core.Application.Services
{
    public interface IBlogArticleServices : IBaseServices<BlogArticle>
    {
        Task<List<BlogArticle>> GetBlogs();

        Task<BlogViewModels> GetBlogDetails(int id);
    }
}