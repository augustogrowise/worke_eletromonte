using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService_Eletromonte.Domain.Entities
{
  public class Imagem
  {
    public int ProdutoId { get; set; }
    public string Path { get; set; }
    public string Description { get; set; }
    public bool ImagemPrincipal { get; set; }
  }

}
