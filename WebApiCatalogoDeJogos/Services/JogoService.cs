using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCatalogoDeJogos.Exceptions;
using WebApiCatalogoDeJogos.InputModel;
using WebApiCatalogoDeJogos.Repositories;
using WebApiCatalogoDeJogos.ViewModel;
using WebApiCatalogoDeJogos.Entities;

namespace WebApiCatalogoDeJogos.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogorepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogorepository = jogoRepository;
        }


        public async Task<List<JogoViewModel>> Obter(int pagina, int quantidade)
        {
            var result = await _jogorepository.Obter(pagina, quantidade);

            return result.Select(result => new JogoViewModel //Pesquisar sobre o AUTOMAP para diminuir esse código
            {
                Id = result.Id,
                Nome = result.Nome,
                Produtora = result.Produtora,
                Preco = result.Preco
            }).ToList();
        }

        public async Task<JogoViewModel> Obter(Guid id)
        {
            var result = await _jogorepository.Obter(id);

            if (result == null)
                return null;

            return new JogoViewModel
            {
                Id = result.Id,
                Nome = result.Nome,
                Produtora = result.Produtora,
                Preco = result.Preco
            };
        }

        public async Task<JogoViewModel> Inserir(JogoInputModel jogo)
        {
            var entidadeJogo = await _jogorepository.Obter(jogo.Nome, jogo.Produtora);

            if (entidadeJogo.Count > 0)

                throw new JogoJaCadastradoException();

            var jogoInsert = new Jogo
            {
                Id = Guid.NewGuid(),
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };

            await _jogorepository.Inserir(jogoInsert);

            return new JogoViewModel
            {
                Id = jogoInsert.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }

        public async Task Atualizar(Guid id, JogoInputModel jogo)
        {
            var entidadeJogo = await _jogorepository.Obter(id);

            if (entidadeJogo == null)
                throw new JogoNaoCadastradoException();

            entidadeJogo.Nome = jogo.Nome;
            entidadeJogo.Produtora = jogo.Produtora;
            entidadeJogo.Preco = jogo.Preco;

            await _jogorepository.Atualizar(entidadeJogo);
        }

        public async Task Atualizar(Guid id, double preco)
        {
            var entidadeJogo = await _jogorepository.Obter(id);

            if (entidadeJogo == null)
                throw new JogoNaoCadastradoException();

            entidadeJogo.Preco = preco;

            await _jogorepository.Atualizar(entidadeJogo);
        }

        public async Task Remover(Guid id)
        {
            var result = await _jogorepository.Obter(id);

            if (result == null)
                throw new JogoNaoCadastradoException();

            await _jogorepository.Remover(id);
        }

        public void Dispose()
        {
            _jogorepository?.Dispose(); // Necessário para não deixar conexão aberta com o Banco de Dados
        }
    }
}
