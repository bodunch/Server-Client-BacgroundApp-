namespace Server.Data
{
    public static class ClientHelper
    {
        private static readonly object _lockObj = new();

        public static int GetOrAddClient(AppDbContext context, string machineName)
        {
            lock (_lockObj)
            {
                if (string.IsNullOrEmpty(machineName))
                    machineName = "Unknown-PC";

                string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                var client = context.Clients.FirstOrDefault(c => c.MachineName == machineName);

                if (client == null)
                {
                    client = new Entities.ClientsEntity
                    {
                        MachineName = machineName,
                        FirstConnected = currentTime,
                        LastSeen = currentTime
                    };
                    context.Clients.Add(client);
                    context.SaveChanges();
                }
                else
                {
                    client.LastSeen = currentTime;
                    context.SaveChanges();
                }

                return client.Id;
            }
        }
    }
}
