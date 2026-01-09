using CenterBackend.common;
using CenterBackend.Constant;
using CenterBackend.Dto;
using CenterBackend.Exceptions;
using CenterBackend.IReportServices;
using CenterReport.Repository;
using CenterReport.Repository.Models;
using Mapster;
using Masuit.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Text.Json;

namespace CenterBackend.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository<SourceData> SourceDatas;
        private readonly IReportRepository<HourlyDataStatistic> HourlyDataStatistics;
        private readonly IReportUnitOfWork reportUnitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ReportService(IReportRepository<SourceData> SourceDatas, IReportRepository<HourlyDataStatistic> HourlyDataStatistics, IReportUnitOfWork reportUnitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            this.SourceDatas = SourceDatas;
            this.HourlyDataStatistics = HourlyDataStatistics;
            this.reportUnitOfWork = reportUnitOfWork;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> DeleteReport( long id, AddReportDto_1 AddReportDto)
        {
            switch (AddReportDto.Target)
            {
                case "SourceData":
                    var source = await SourceDatas.GetByIdAsync(id);
                    if (source == null) return false;
                    await SourceDatas.DeleteByIdAsync(id);
                    break;
                case "HourlyDataStatistic":
                    var hour = await HourlyDataStatistics.GetByIdAsync(id);
                    if (hour == null) return false;
                    await HourlyDataStatistics.DeleteByIdAsync(id);
                    break;
                default:
                    throw new ArgumentException("未知的删除目标", AddReportDto.Target);
            }
                await reportUnitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddReport(AddReportDto_1 AddReportDto)
        {
            switch (AddReportDto.Target)
            {
                case "HourlyDataStatistic":
                    // 查询 source 数据（接口目前返回列表，但业务上只有一条）
                    var sources = await SourceDatas.GetByDataTimeAsync(AddReportDto.Datetime);
                    var source = sources?.FirstOrDefault();
                    if (source == null) return false; // 未找到数据，按需可抛异常或返回 false
                    var stat = source.Adapt<HourlyDataStatistic>();// 映射单条 SourceData 到 HourlyDataStatistic 并入库
                    stat.Id = 0; // 确保 EF 分配新 Id

                    await HourlyDataStatistics.AddAsync(stat);
                    break;
                default:
                    throw new ArgumentException("未知的添加目标", AddReportDto.Target);
            }
            await reportUnitOfWork.SaveChangesAsync();
            return true;
        }

        //public async Task<long> UserRegister(RegisterDto registerDto)
        //{
        //    // 1.校验
        //    if (registerDto.UserAccount.IsNullOrEmpty() || registerDto.UserPassword.IsNullOrEmpty() || registerDto.CheckPassword.IsNullOrEmpty())
        //    {
        //        throw new BusinessException(ErrorCode.PARAMS_ERROR, "参数为空");
        //    }
        //    if (await userRepository.db.SingleOrDefaultAsync(s => s.UserAccount == registerDto.UserAccount) != null)
        //    {
        //        throw new BusinessException(ErrorCode.PARAMS_ERROR, "用户名已经存在");
        //    }
        //    if (!registerDto.UserPassword.Equals(registerDto.CheckPassword))
        //    {
        //        throw new BusinessException(ErrorCode.PARAMS_ERROR, "两次输入的密码不一致");
        //    }

        //    // 2.添加用户
        //    var user = new User();
        //    user.UserAccount = registerDto.UserAccount;
        //    // todo: 密码暂时未加密
        //    user.UserPassword = registerDto.UserPassword;
        //    user.UserStatus = 0;
        //    //默认注册的是普通用户
        //    user.Role = 0;
        //    await userRepository.AddAsync(user);
        //    await unitOfWork.SaveChangesAsync();
        //    return user.Id;
        //}

        //public async Task<UserDto> Login(LoginDto loginDto)
        //{
        //    // 1.校验
        //    if (loginDto.UserAccount.IsNullOrEmpty())
        //    {
        //        throw new BusinessException(ErrorCode.PARAMS_ERROR, "用户账号不能为空");
        //    }
        //    if (loginDto.UserPassword.IsNullOrEmpty())
        //    {
        //        throw new BusinessException(ErrorCode.PARAMS_ERROR, "用户密码不能为空");
        //    }

        //    // 查询用户是否存在
        //    var user = await userRepository.db.SingleOrDefaultAsync(s => s.UserAccount == loginDto.UserAccount && s.UserPassword == loginDto.UserPassword);
        //    // 用户不存在
        //    if (user == null)
        //    {
        //        throw new BusinessException(ErrorCode.NULL_ERROR, "用户不存在");
        //    }
        //    // 2.用户信息脱敏
        //    var safetyUser = user.Adapt<UserDto>();
        //    // 3.记录用户登录态
        //    if (httpContextAccessor.HttpContext != null)
        //    {
        //        var session = httpContextAccessor.HttpContext.Session;
        //        session.SetString(UserConstant.USER_LOGIN_STATE, JsonSerializer.Serialize(safetyUser));
        //    }
        //    return safetyUser;
        //}



        //public async Task<List<UserDto>> SearchUsers(string userName)
        //{
        //    var result = await userRepository.db.WhereIf(!userName.IsNullOrEmpty(), s => s.UserName.Contains(userName)).ToListAsync();
        //    return result.Adapt<List<UserDto>>();
        //}

        //public int UserLogout()
        //{
        //    if (httpContextAccessor.HttpContext != null)
        //    {
        //        var session = httpContextAccessor.HttpContext.Session;
        //        session.Remove(UserConstant.USER_LOGIN_STATE);
        //    }
        //    return 1;
        //}
    }
}
