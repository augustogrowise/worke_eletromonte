using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService_Eletromonte.Domain.Entities;

namespace WorkerService_Eletromonte.Infraestructure.http
{
  public class ApiService
  {
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    public async Task<List<Imagem>> BuscarTodasImagensAsync()
    {
      var todasImagens = new List<Imagem>();
      int pagina = 1;

      while (true)
      {
        string url = $"https://smart.sgisistemas.com.br/api_produto/eletromonte/consultas/produtos?page={pagina}&codigo=&descricao=&ativo=true&token=dev-eletro";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var resultado = JsonConvert.DeserializeObject<ProdutoResponse>(json);

        if (resultado.Produtos == null || resultado.Produtos.Count == 0)
          break;

        foreach (var produto in resultado.Produtos)
        {
          if (produto.Imagens != null)
          {
            foreach (var img in produto.Imagens)
            {
              todasImagens.Add(new Imagem
              {
                ProdutoId = produto.Id,
                Path = img.Path,
                Description = img.Description,
                ImagemPrincipal = img.ImagemPrincipal
              });
            }
          }
        }

        pagina++;
      }

      return todasImagens;
    }

  }

  public class ProdutoResponse
  {
    [JsonProperty("produtos")]
    public List<Produto> Produtos { get; set; }
  }

  public class Produto
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("imagens")]
    public List<ImagemJson> Imagens { get; set; }
  }

  public class ImagemJson
  {
    [JsonProperty("path")]
    public string Path { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("imagem_principal")]
    public bool ImagemPrincipal { get; set; }
  }

}
