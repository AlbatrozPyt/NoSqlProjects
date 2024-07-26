using MongoDB.Driver;

namespace MinimalApi.Services
{
    public class MongoDbService
    {
        // Armazena  a configuração da aplicação
        private readonly IConfiguration? _configuration;

        // Armazena uma referencia ao MongoDB
        private readonly IMongoDatabase? _database;

        /// <summary>
        /// Contém a configurção necessaria para acesso ao MongoDb
        /// </summary>
        /// <param name="configuration"> Um objeto contendo toda a configuração da aplicação </param>
        public MongoDbService(IConfiguration configuration)
        {
            // Atribui a configuracao em _configuration
            _configuration = configuration;

            // Atribui o valor da String de Conexao
            var connectionString = _configuration.GetConnectionString("DbConnection");

            // Cria uma url com a string de conexao
            var mongoUrl = MongoUrl.Create(connectionString);

            // Cria um client com a url
            var mongoClient = new MongoClient(mongoUrl);

            // recebe o nome do banco de dados
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        // Propieda para acessar o banco de dados e retorna os dados em "_database"

        public IMongoDatabase GetDatabase => _database!;
    }
}
