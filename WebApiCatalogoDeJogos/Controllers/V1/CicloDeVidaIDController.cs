using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCatalogoDeJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class CicloDeVidaIDController : ControllerBase
    {
        public readonly IExemploSingleton _exemplosingleton1;
        public readonly IExemploSingleton _exemplosingleton2;

        public readonly IExemploScoped _exemploscoped1;
        public readonly IExemploScoped _exemploscoped2;

        public readonly IExemploTransient _exemplotransient1;
        public readonly IExemploTransient _exemplotransient2;

        public CicloDeVidaIDController(IExemploSingleton exemplosingleton1,
                                    IExemploSingleton exemplosingleton2,
                                    IExemploScoped exemploscoped1,
                                    IExemploScoped exemploscoped2,
                                    IExemploTransient exemplotransient1,
                                    IExemploTransient exemplotransient2)
        {
            _exemploscoped1 = exemploscoped1;
            _exemploscoped2 = exemploscoped2;
            _exemplosingleton1 = exemplosingleton1;
            _exemplosingleton2 = exemplosingleton2;
            _exemplotransient1 = exemplotransient1;
            _exemplotransient2 = exemplotransient2;
        }

        [HttpGet]
        public Task<string> Get()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Singleton 1: {_exemplosingleton1.Id}");
            sb.AppendLine($"Singleton 2: {_exemplosingleton2.Id}");
            sb.AppendLine();
            sb.AppendLine($"Scoped 1: {_exemploscoped1.Id}");
            sb.AppendLine($"Scoped 2: {_exemploscoped2.Id}");
            sb.AppendLine();
            sb.AppendLine($"Transient 1: {_exemplotransient1.Id}");
            sb.AppendLine($"Transient 2: {_exemplotransient2.Id}");

            return Task.FromResult(sb.ToString());
        }

        public interface IExemploGeral
        {
            public Guid Id { get; }
        }
        public interface IExemploSingleton : IExemploGeral
        { }
        public interface IExemploScoped : IExemploGeral
        { }
        public interface IExemploTransient : IExemploGeral
        { }

        public class ExemploCiclodeVida : IExemploSingleton, IExemploScoped, IExemploTransient
        {
            private readonly Guid _guid;

            public ExemploCiclodeVida()
            {
                _guid = Guid.NewGuid();
            }

            public Guid Id => _guid;
        }
    }
}
