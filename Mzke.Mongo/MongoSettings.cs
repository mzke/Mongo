using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzke.Mongo
{
    public  class MongoSettings
    {
        public MongoSettings() { }
        public  string DatabaseName { get; } = "databasename";
        public  string ConnectionString = "mongodb://localhost:27017";
    }
}
