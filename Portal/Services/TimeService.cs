using Portal.Models.DTOs;
using Portal.Services.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace Portal.Services
{
    public class TimeService : ITimeService
    {
        private IHttpClientFactory _httpClient;

        public TimeService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        //Metodo criado para lidar com o request na timeApi
        public async Task<TimeResponseDTO> GetDateTime()
        {
            var httpClient = _httpClient.CreateClient();

            var result = await httpClient.GetAsync(Program.API_ADDRESS);
            if (result.IsSuccessStatusCode)
            {
                //Atribui data no formato desejado
                string date = await result.Content.ReadAsStringAsync();
                
                TimeResponseDTO response = new TimeResponseDTO()
                {
                    // Atribui data formatada.
                    Date = date,

                    // Converte retorno para DateTime e extrai Day
                    Day = int.Parse(date.Split(',')[1].Split(' ')[1].ToString())
                };

                return response;
            }

            return null;
        }
    }
}
