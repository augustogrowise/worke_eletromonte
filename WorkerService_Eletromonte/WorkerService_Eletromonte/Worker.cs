using WorkerService_Eletromonte.Infraestructure.http;
using WorkerService_Eletromonte.Infraestructure.Persistence;

namespace WorkerService_Eletromonte
{
    public class Worker : BackgroundService
    {
    private readonly ILogger<Worker> _logger;
    private readonly ApiService _apiService;
    private readonly SupabaseService _supabaseService;

    public Worker(ILogger<Worker> logger, ApiService apiService, SupabaseService supabaseService)
    {
      _logger = logger;
      _apiService = apiService;
      _supabaseService = supabaseService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
          while (!stoppingToken.IsCancellationRequested)
          {
            try
            {
              var imagens = await _apiService.BuscarTodasImagensAsync();
              await _supabaseService.SalvarImagensAsync(imagens);
              _logger.LogInformation($"{imagens.Count} imagens salvas.");
            }
            catch (Exception ex)
            {
              _logger.LogError(ex, "Erro ao processar as imagens.");
            }

             await Task.Delay(TimeSpan.FromDays(7), stoppingToken);

          }
    }
    }
}
