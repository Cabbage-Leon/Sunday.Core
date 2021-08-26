using System.Linq;
using System.Threading.Tasks;
using Sunday.Core;
using Sunday.Core.Application.Services;
using Sunday.Core.Domain.Entities;
using Sunday.Core.Persistence.Repositories.Base;

namespace Blog.Core.FrameWork.Services
{
    /// <summary>
    /// sysUserInfoServices
    /// </summary>
    public class SysUserInfoServices : BaseServices<SysUserInfo>, ISysUserInfoServices
    {
        private readonly IBaseRepository<SysUserInfo> _dal;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        private readonly IBaseRepository<Role> _roleRepository;

        public SysUserInfoServices(IBaseRepository<SysUserInfo> dal, IBaseRepository<UserRole> userRoleRepository, IBaseRepository<Role> roleRepository)
        {
            this._dal = dal;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            base.BaseDal = dal;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public async Task<SysUserInfo> SaveUserInfo(string loginName, string loginPwd)
        {
            SysUserInfo sysUserInfo = new SysUserInfo(loginName, loginPwd);
            SysUserInfo model = new SysUserInfo();
            var userList = await base.Query(a => a.uLoginName == sysUserInfo.uLoginName && a.uLoginPWD == sysUserInfo.uLoginPWD);
            if (userList.Count > 0)
            {
                model = userList.FirstOrDefault();
            }
            else
            {
                var id = await base.Add(sysUserInfo);
                model = await base.QueryById(id);
            }

            return model;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        public async Task<string> GetUserRoleNameStr(string loginName, string loginPwd)
        {
            string roleName = "";
            var user = (await base.Query(a => a.uLoginName == loginName && a.uLoginPWD == loginPwd)).FirstOrDefault();
            var roleList = await _roleRepository.Query(a => a.IsDeleted == false);
            if (user != null)
            {
                var userRoles = await _userRoleRepository.Query(ur => ur.UserId == user.uID);
                if (userRoles.Count > 0)
                {
                    var arr = userRoles.Select(ur => ur.RoleId.ObjToString()).ToList();
                    var roles = roleList.Where(d => arr.Contains(d.Id.ObjToString()));

                    roleName = string.Join(',', roles.Select(r => r.Name).ToArray());
                }
            }
            return roleName;
        }
    }
}

//----------sysUserInfo结束----------