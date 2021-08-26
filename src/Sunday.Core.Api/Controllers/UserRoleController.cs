using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sunday.Core.Application.Services;
using Sunday.Core.Domain.Entities;
using Sunday.Core.Infrastructure;
using Sunday.Core.Model;

namespace Sunday.Core.Api.Controllers
{
    /// <summary>
    /// 用户角色关系
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Permissions.Name)]
    public class UserRoleController : Controller
    {
        private readonly ISysUserInfoServices _sysUserInfoServices;
        private readonly IUserRoleServices _userRoleServices;
        private readonly IRoleServices _roleServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysUserInfoServices"></param>
        /// <param name="userRoleServices"></param>
        /// <param name="roleServices"></param>
        public UserRoleController(ISysUserInfoServices sysUserInfoServices, IUserRoleServices userRoleServices, IRoleServices roleServices)
        {
            this._sysUserInfoServices = sysUserInfoServices;
            this._userRoleServices = userRoleServices;
            this._roleServices = roleServices;
        }

        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<SysUserInfo>> AddUser(string loginName, string loginPwd)
        {
            return new MessageModel<SysUserInfo>()
            {
                success = true,
                msg = "添加成功",
                response = await _sysUserInfoServices.SaveUserInfo(loginName, loginPwd)
            };
        }

        /// <summary>
        /// 新建Role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<Role>> AddRole(string roleName)
        {
            return new MessageModel<Role>()
            {
                success = true,
                msg = "添加成功",
                response = await _roleServices.SaveRole(roleName)
            };
        }

        /// <summary>
        /// 新建用户角色关系
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="rid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<UserRole>> AddUserRole(int uid, int rid)
        {
            return new MessageModel<UserRole>()
            {
                success = true,
                msg = "添加成功",
                response = await _userRoleServices.SaveUserRole(uid, rid)
            };
        }
    }
}