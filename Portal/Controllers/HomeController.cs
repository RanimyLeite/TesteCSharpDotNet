using Humanizer.Bytes;
using Microsoft.AspNetCore.Mvc;
using Portal.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Portal.Extensions;

namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        public static int _requestsCounter = 0;
        private ITimeService _timeService;

        // Criado construtor da controller para injetar dependencias
        public HomeController(ITimeService timeService)
        {
            _timeService = timeService;
        }

        public async Task<IActionResult> Index()
        {
            // Incrementa o contador de requisições
            _requestsCounter++;

            // Verifica se a API tem recursos para atender a requisição
            if (!Program.CheckIfThereIsThreadAvailable())
                return BadRequest(Messages.THREAD_IS_NO_THREAD_AVAILABLE);

            // Busca as informações de data da API TimeAPI
            var result = await _timeService.GetDateTime();

            if (result == null)
                return BadRequest(Messages.TIME_API_ERROR);

            // Calcula e gera a chave
            var key = "";

            var random = new Random();
            for (int i = 0; i < 4096; i++)
                key = string.Concat(key, result.Day * i * random.Next(100, 9999));

            // Obtem os números ímpares gerados na chave
            var oddNumbers = new List<int>();

            // Adiciona os números impar da key gerada;
            foreach (var c in key.ToArray())
            {
                if (c.IsOddNumber())
                    oddNumbers.Add(c.ToInt());
            }

            // Aplica valores para View
            ViewData["DateTimeNow"] = result.Date;
            ViewData["Key"] = key;
            ViewData["Sum"] = oddNumbers.Sum(x => x);//soma dos numeros impares da chave gerada
            ViewData["VirtualMachines"] = Program.NUMBER_OF_VIRTUAL_MACHINES;
            ViewData["RequestsCounter"] = _requestsCounter;

            // Aplica informações de memória para View
            ViewData["gc0"] = GC.CollectionCount(0);
            ViewData["gc1"] = GC.CollectionCount(1);
            ViewData["gc2"] = GC.CollectionCount(2);
            ViewData["currentMemory"] = ByteSize.FromBytes(GC.GetTotalMemory(false)).ToString();
            ViewData["privateBytes"] = ByteSize.FromBytes(Process.GetCurrentProcess().WorkingSet64);

            // Retorna View
            return View();
        }

        public IActionResult Information()
        {
            return View();
        }
    }
}