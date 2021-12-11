using AutoMapper;
using Sunday.Core.Domain.Entities;
using Sunday.Core.Model.ViewModels;

namespace Sunday.Core.Extensions.AutoMapper
{
    public class DomainToModelMappingProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public DomainToModelMappingProfile()
        {
            //示例
            //CreateMap<Student, StudentViewModel>()
            //   .ForMember(d => d.County, o => o.MapFrom(s => s.Address.County))
            //   .ForMember(d => d.Province, o => o.MapFrom(s => s.Address.Province))
            //   .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City))
            //   .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street))
            //   ;

            CreateMap<BlogArticle, BlogViewModels>();
            CreateMap<BlogViewModels, BlogArticle>();

            #region 基础数据

            //CreateMap<HotelBrand_Dto, HotelBrand>()
            //  .ForMember(a => a.Id, o => o.MapFrom(s => s.Code))
            //  .ForMember(a => a.Name, o => o.MapFrom(s => s.Name))
            //  .ForMember(a => a.EnName, o => o.MapFrom(s => s.NameEn))
            //  .ForMember(a => a.HomsomId, o => o.MapFrom(s => s.HsId))
            //  .ReverseMap();//允许反向映射

            #endregion
        }
    }
}
