using Portal.Models.DTOs;
using System.Threading.Tasks;

namespace Portal.Services.Interfaces
{
    public interface ITimeService
    {
        Task<TimeResponseDTO> GetDateTime();
    }
}
