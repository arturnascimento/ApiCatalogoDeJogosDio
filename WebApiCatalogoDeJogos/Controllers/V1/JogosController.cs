using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiCatalogoDeJogos.Exceptions;
using WebApiCatalogoDeJogos.InputModel;
using WebApiCatalogoDeJogos.Services;
using WebApiCatalogoDeJogos.ViewModel;

namespace WebApiCatalogoDeJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoservice;

        public JogosController(IJogoService jogoService)
        {
            _jogoservice = jogoService;
        }

        /// <summary>
        /// Buscar todos os jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível retornar os jogos sem paginação
        /// </remarks>
        /// <param name="pagina">Indica qual página está sendo consultada. Minimo 1</param>
        /// <param name="quantidade">Indica a quantidade de registros por página. Minimo 1 e máximo 50</param
        /// <response code="200">Retorna a Lista de Jogos</response>
        /// <response code="204">Caso não haja jogos</response>
        /// <returns>Retorna a Lista de Jogos ou vazio</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5) //ActionResult retorna o status HTTP (200, 400, etc)
        {
            var result = await _jogoservice.Obter(pagina, quantidade);

            if (result.Count() == 0)
                return NoContent();

            return Ok(result);
        }

        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute] Guid idJogo) //Guid é uma forma de identificar as variaveis que gera um valor aleatório e unico (semelhante ao SharedPreferences)
        {
            var result = await _jogoservice.Obter(idJogo);

            if (result == null)
                return NoContent();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody] JogoInputModel jogo)
        {
            try
            {
                var result = await _jogoservice.Inserir(jogo);

                return Ok(result);
            }
            catch (JogoJaCadastradoException ex)
            {
                return UnprocessableEntity("Já existe um jogo com esse nome para essa produtora");
            }
        }

        [HttpPut("{idJogo:guid}")]  // No put, vc atualiza o recurso inteiro
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromBody] JogoInputModel jogo)
        {
            try
            {
                await _jogoservice.Atualizar(idJogo, jogo);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo");
            }
        }

        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]  // No patch, você atualiza algum dado especifico do recurso
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromRoute] double preco)
        {
            try
            {
                await _jogoservice.Atualizar(idJogo, preco);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo");
            }
        }

        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult> ApagarJogo([FromRoute] Guid idJogo)
        {
            try
            {
                await _jogoservice.Remover(idJogo);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo");
            }
        }
    }
}
