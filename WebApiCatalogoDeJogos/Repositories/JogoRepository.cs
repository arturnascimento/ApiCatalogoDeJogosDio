using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCatalogoDeJogos.Entities;
using WebApiCatalogoDeJogos.InputModel;
using WebApiCatalogoDeJogos.ViewModel;

namespace WebApiCatalogoDeJogos.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private static Dictionary<Guid, Jogo> jogos = new Dictionary<Guid, Jogo>()
        {
            {Guid.Parse("e0f58761-9fcd-46de-86fc-351700c26891"), new Jogo{ Id = Guid.Parse("e0f58761-9fcd-46de-86fc-351700c26891"), Nome = "Call Of Duty", Produtora = "Activision", Preco = 200} },
            {Guid.Parse("7cfd235a-4419-4313-81f4-ed89956dc52d"), new Jogo{ Id = Guid.Parse("7cfd235a-4419-4313-81f4-ed89956dc52d"), Nome = "Battlefield", Produtora = "EA", Preco = 190} },
            {Guid.Parse("bc0fa8e9-7159-467a-9bb7-e5031a14ba1c"), new Jogo{ Id = Guid.Parse("bc0fa8e9-7159-467a-9bb7-e5031a14ba1c"), Nome = "Fifa 22", Produtora = "EA", Preco = 200} },
            {Guid.Parse("1506599a-958d-461b-b270-2f9fa9ba1e17"), new Jogo{ Id = Guid.Parse("1506599a-958d-461b-b270-2f9fa9ba1e17"), Nome = "Final Fantasy", Produtora = "Square Enix", Preco = 250} },
            {Guid.Parse("1bfacd14-fd2e-4a51-8a3d-5de90dcdc6b9"), new Jogo{ Id = Guid.Parse("1bfacd14-fd2e-4a51-8a3d-5de90dcdc6b9"), Nome = "League of Legends", Produtora = "Riot", Preco = 10} },
            {Guid.Parse("715bb391-0b02-4dbe-9601-26172d57966c"), new Jogo{ Id = Guid.Parse("715bb391-0b02-4dbe-9601-26172d57966c"), Nome = "Warcraft", Produtora = "Blizzard", Preco = 100} }
        };

        public Task Atualizar(Jogo jogo)
        {
            jogos[jogo.Id] = jogo;
            return Task.CompletedTask;
        }

     
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task Inserir(Jogo jogo)
        {
            jogos.Add(jogo.Id, jogo);
            return Task.CompletedTask;
        }

        
        public Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            return Task.FromResult(jogos.Values.Skip((pagina - 1) * quantidade).Take(quantidade).ToList());
        }

        public Task<Jogo> Obter(Guid id)
        {
            if (!jogos.ContainsKey(id))
                return null;

            return Task.FromResult(jogos[id]);
        }

        public Task<List<Jogo>> Obter(string nome, string produtora)
        {
            return Task.FromResult(jogos.Values.Where(jogo => jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora)).ToList());
        }

        public Task<List<Jogo>> ObterSemLambda(string nome, string produtora)
        {
            var result = new List<Jogo>();

            foreach (var jogo in jogos.Values)
            {
                if (jogo.Nome.Equals(nome) && jogo.Produtora.Equals(produtora))
                    result.Add(jogo);
            }

            return Task.FromResult(result);
        }

        public Task Remover(Guid id)
        {
            jogos.Remove(id);
            return Task.CompletedTask;
        }

     
    }
} 
