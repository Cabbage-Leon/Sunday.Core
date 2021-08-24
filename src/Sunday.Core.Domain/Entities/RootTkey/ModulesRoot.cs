using SqlSugar;
using System;

namespace Sunday.Core.Domain
{
    /// <summary>
    /// 接口API地址信息表 
    /// 父类
    /// </summary>
    public class ModulesRoot<Tkey> : RootEntityTkey<Tkey> where Tkey : IEquatable<Tkey>
    {
        /// <summary>
        /// 父ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public Tkey ParentId { get; set; }
       
    }
}
