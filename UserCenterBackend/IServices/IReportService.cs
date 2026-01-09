using CenterBackend.Dto;
using CenterReport.Repository;
using CenterReport.Repository.Models;
using static CenterBackend.Services.ReportService;

namespace CenterBackend.IReportServices
{
    public interface IReportService
    {

        Task<bool> DeleteReport(long id, AddReportDto_1 AddReportDto);
        Task<bool> AddReport( AddReportDto_1 AddReportDto);

        //Task<long> UserRegister(RegisterDto registerDto);

        //Task<UserDto> Login(LoginDto loginDto);

        //Task<List<UserDto>> SearchUsers(string userName);

        //Task<bool> DeleteUser(long id);

        //Task<UserDto> GetSafetyUser(long id);

        //int UserLogout();
    }
}
