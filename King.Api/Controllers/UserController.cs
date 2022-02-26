using AutoMapper;
using King.Api.Models;
using King.Data;
using King.Helper;
using King.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace King.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController, Authorize]

    public class UserController : ControllerBase
    {
        private ICacheService _cache;
        private IMapper _mapper;
        private IBllService<User> _userService;


        public UserController(IMapper mapper, ICacheService cache, IBllService<User> userService)
        {
            _cache = cache;
            _mapper = mapper;
            _userService = userService;
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(UserInput user)
        {
            if (ModelState.IsValid)
            {
                var code = _cache.Get(Common.CACHE_ValidateKey + user.ValidateKey) ?? "";
                if (user.ValidateValue.ToLower() != code.ToString().ToLower() || string.IsNullOrEmpty(user.ValidateValue))
                {
                    return BadRequest("验证码错误");
                }

                var u = _userService.GetOne(p => p.Telphone == user.Telphone);
                if (u != null)
                {
                    return BadRequest("手机号已存在");
                }

                var model = new User()
                {
                    Telphone = user.Telphone,
                    Password = StringHelper.ToMD5(user.Password),
                };

                var query = await _userService.AddAsync(model);
                return Ok(query.Id);
            }

            return BadRequest("参数错误");
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>       
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetUserInfo()
        {
            var user = Utils.LoginUser(User);
            var query = await _userService.GetOneAsync(p => p.Telphone == user.UserName);

            if (query != null)
            {
                var data = _mapper.Map(query, new UserDto());
                return Ok(data);
            }
            else
            {
                return BadRequest("用户不存在");
            }
        }

        
    }
}
