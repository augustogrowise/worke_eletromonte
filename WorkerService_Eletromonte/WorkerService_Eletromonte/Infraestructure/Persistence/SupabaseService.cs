using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService_Eletromonte.Domain.Entities;

namespace WorkerService_Eletromonte.Infraestructure.Persistence
{
  public class SupabaseService
  {
    private readonly string _connectionString;

    public SupabaseService(string connectionString)
    {
      _connectionString = connectionString;
    }

    public async Task SalvarImagensAsync(List<Imagem> imagens)
    {
      if (imagens == null || imagens.Count == 0)
        return;

      await using var conn = new NpgsqlConnection(_connectionString);
      await conn.OpenAsync();

      // Buscar paths e produto_id já existentes
      var existentes = new HashSet<(int ProdutoId, string Path)>();

      var cmdSelect = new NpgsqlCommand("SELECT produto_id, path FROM imagens_produto", conn);
      await using (var reader = await cmdSelect.ExecuteReaderAsync())
      {
        while (await reader.ReadAsync())
        {
          var produtoId = reader.GetInt32(0);
          var path = reader.GetString(1);
          existentes.Add((produtoId, path));
        }
      }

      //  Filtrar apenas as imagens novas
      var novasImagens = imagens
          .Where(img => !existentes.Contains((img.ProdutoId, img.Path)))
          .ToList();

      if (novasImagens.Count == 0)
        return;

      //  Inserir novas imagens
      foreach (var img in novasImagens)
      {
        var cmdInsert = new NpgsqlCommand(
            @"INSERT INTO imagens_produto (produto_id, path, description, imagem_principal)
              VALUES (@produto_id, @path, @description, @imagem_principal)", conn);

        cmdInsert.Parameters.AddWithValue("produto_id", img.ProdutoId);
        cmdInsert.Parameters.AddWithValue("path", img.Path);
        cmdInsert.Parameters.AddWithValue("description", img.Description ?? "");
        cmdInsert.Parameters.AddWithValue("imagem_principal", img.ImagemPrincipal);

        await cmdInsert.ExecuteNonQueryAsync();
      }
    }


  }

}
