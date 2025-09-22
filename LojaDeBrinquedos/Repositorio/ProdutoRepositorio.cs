using Dapper;
using LojaDeBrinquedos.Models;
using MySql.Data.MySqlClient;

namespace LojaDeBrinquedos.Repositorio
{
    public class ProdutoRepositorio
    {
        private readonly string _connectionString;

        public ProdutoRepositorio(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Produto>> TodosProdutos()
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = "SELECT Id, Nome, Descricao, Preco, ImageUrl, Estoque FROM produto";
            return await connection.QueryAsync<Produto>(sql);
        }


        public async Task<Produto?> ProdutosPorId(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = "SELECT Id, Nome, Descricao, Preco, ImageUrl, Estoque FROM produto WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id });
        }


    }

}
