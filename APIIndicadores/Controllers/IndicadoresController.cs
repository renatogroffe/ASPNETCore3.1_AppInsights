using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using APIIndicadores.Data;
using APIIndicadores.Models;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace APIIndicadores.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IndicadoresController : ControllerBase
    {
        private static object syncObject = Guid.NewGuid();

        [HttpGet]
        public ContentResult Get(
            [FromServices]ApplicationDbContext context,
            [FromServices]IDistributedCache cache,
            [FromServices]TelemetryConfiguration telemetryConfig)
        {
            DateTimeOffset inicio = DateTime.Now;
            Stopwatch watch = new Stopwatch();
            watch.Start();

            string valorJSON = cache.GetString("Indicadores");
            if (valorJSON == null)
            {
                // Exemplo de implementação do pattern Double-checked locking
                // Para mais informações acesse:
                // https://en.wikipedia.org/wiki/Double-checked_locking
                lock (syncObject)
                {
                    valorJSON = cache.GetString("Indicadores");
                    if (valorJSON == null)
                    {
                        var indicadores = (from i in context.Indicadores
                                           select i).AsEnumerable();

                        DistributedCacheEntryOptions opcoesCache =
                            new DistributedCacheEntryOptions();
                        opcoesCache.SetAbsoluteExpiration(
                            TimeSpan.FromSeconds(30));

                        valorJSON = JsonSerializer.Serialize(indicadores);
                        cache.SetString("Indicadores", valorJSON, opcoesCache);
                    }
                }
            }

            watch.Stop();
            TelemetryClient client = new TelemetryClient(telemetryConfig);

            client.TrackDependency(
                "Redis", "Get", valorJSON, inicio, watch.Elapsed, true);

            return Content(valorJSON, "application/json");
        }
    }
}