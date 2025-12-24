using Cassandra;

namespace AgroSense.Services
{
    public class SessionManager
    {
        public static Cassandra.ISession session;

        public static Cassandra.ISession GetSession()
        {
            if(session == null)
            {
                Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
                session = cluster.Connect("agrosense");
            }
            return session;
        }
    }
}
