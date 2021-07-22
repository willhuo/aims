using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Aims.AppService.Interfaces;
using Aims.Dto.Accounts;
using Aims.Dto.Users;
using Aims.Option;
using Aims.Repo;
using AIMS.Util;
using AutoMapper;
using Dijing.Common.Core.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Aims.AppService
{
    public class AccountAppService: IAccountAppService
    {
        private JwtOption _jwtOption { get; set; }
        private AimsDbContext _db { get; set; }
        private IMapper _mapper { get; set; }
        IUserContext _userContext { get; set; }


        public AccountAppService(IOptions<JwtOption> jwtOption,AimsDbContext db,IMapper mapper,IUserContext userContext)
        {
            this._jwtOption = jwtOption.Value;
            this._db = db;
            this._mapper = mapper;
            this._userContext = userContext;
        }

        /// <summary>
        /// JWT方式用户名密码登录
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public LoginDto JwtLoginByPwd(LoginByPwdArgs args)
        {
            //TODO:表示验证通过，需要增加数据库验证逻辑


            var claims = new Claim[]
            {
                new Claim("uname",args.Uname),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOption.Issuer,
                audience: _jwtOption.Audience,
                claims: claims,
                notBefore:DateTime.Now,
                expires: DateTime.Now.AddMinutes(_jwtOption.AccessExpiration),
                signingCredentials: credentials
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginDto()
            {
                Token = jwtToken
            };
        }

        /// <summary>
        /// 简单方式的用户名密码登录
        /// </summary>
        /// <param name="args"></param>
        /// <param name="remoteIP"></param>
        /// <returns></returns>
        public async Task<LoginDto> SimplgLoginByPwd(LoginByPwdArgs args,string remoteIP)
        {
            args.Pwd = EncryptHelper.Default.MD5Encode32(args.Pwd).ToLower();
            var entity = await _db.Users.FirstOrDefaultAsync(x => x.Uname == args.Uname && x.Pwd == args.Pwd);
            if (entity == null)
                throw new ValidationException("账号或密码异常");

            entity.LastLoginIP = remoteIP;
            entity.LastLoginTime=DateTime.Now;
            _db.Update(entity);
            await _db.SaveChangesAsync();

            var dto = _mapper.Map<UserDto>(entity);
            var res = new LoginDto()
            {
                UserDto = dto,
                Token = Guid.NewGuid().ToString("N")
            };

            await RedisHelper.SetAsync(IdentityConsts.TokenPrefix + res.Token, dto, TimeSpan.FromHours(1));
            RedisHelper.SAdd(IdentityConsts.UserPrefix + dto.Id, res.Token);
            await RedisHelper.ExpireAsync(IdentityConsts.UserPrefix + dto.Id, TimeSpan.FromHours(1));

            return res;
        }

        /// <summary>
        /// 用户缓存
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<UserDto> UserCache(string token)
        {
            if (await RedisHelper.ExistsAsync(IdentityConsts.TokenPrefix + token))
            {
                var dto = await RedisHelper.GetAsync<UserDto>(IdentityConsts.TokenPrefix + token);
                return dto;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 用户缓存
        /// </summary>
        /// <returns></returns>
        public async Task<UserDto> UserCache()
        {
            var token = _userContext.Token;
            if (await RedisHelper.ExistsAsync(IdentityConsts.TokenPrefix + token))
            {
                var dto = await RedisHelper.GetAsync<UserDto>(IdentityConsts.TokenPrefix + token);
                return dto;
            }
            else
            {
                return null;
            }
        }
    }
}
