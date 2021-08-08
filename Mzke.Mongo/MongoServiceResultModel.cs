using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mzke.Mongo
{
    public class MongoServiceResultModel<T>
    {
        public bool Sucesso { get; set; }
        public List<T> Dados { get; set; } = new List<T>();
        public List<string> Mensagens { get; set; } = new List<string>();
    }
}
