using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzke.Mongo
{
    

    public class MongoSettings : IMongoSettings
    {
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "tivus";
    }

    public interface IMongoSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
