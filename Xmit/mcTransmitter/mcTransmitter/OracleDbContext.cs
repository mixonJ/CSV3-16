using System.Data.Entity;

namespace OracleFirewall {
    public class OracleDbContext : DbContext {

        public OracleDbContext () {
            //TODO: Do initialization here.
        }

        public OracleDbContext (string connectionName) : base(connectionName) {
            
        }
    }
}