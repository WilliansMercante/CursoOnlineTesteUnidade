
using Bogus;

using CursoOnline.Dominio.Cursos;
using CursoOnline.Dominio.Cursos.Enums;
using CursoOnline.DominioTest._Utils;

using Moq;

namespace CursoOnline.DominioTest.Cursos
{
    public class ArmazenadorDeCursoTest
    {
        private readonly CursoDto _cursoDto;
        private readonly Mock<ICursoRepositorio> _cursoRepositorioMock;
        private readonly ArmazenadorDeCurso _armazenadorDeCurso;
        public ArmazenadorDeCursoTest()
        {
            var fake = new Faker();

            _cursoDto = new CursoDto()
            {
                Nome = fake.Name.FullName(),
                Descricao = fake.Lorem.Paragraph(),
                CargaHoraria = fake.Random.Double(5, 100),
                PublicoAlvo = "Estudante",
                Valor = fake.Random.Double(1, 100000),
            };

            _cursoRepositorioMock = new Mock<ICursoRepositorio>();
            _armazenadorDeCurso = new ArmazenadorDeCurso(_cursoRepositorioMock.Object);
        }

        [Fact]
        public void DeveAdicionarCurso()
        {
            _armazenadorDeCurso.Armazenar(_cursoDto);

            _cursoRepositorioMock.Verify(p => p.Adicionar(
                It.Is<Curso>
                (
                    c => c.Nome == _cursoDto.Nome &&
                    c.Descricao == _cursoDto.Descricao &&
                    c.CargaHoraria == _cursoDto.CargaHoraria &&
                    c.PublicoAlvo == PublicoAlvo.Estudante &&
                    c.Valor == _cursoDto.Valor
                )
                ));
        }

        [Fact]
        public void NaoDeveInformarPublicoAlvoInvalido()
        {
            var publicoAlvoInvalido = "Médico";
            _cursoDto.PublicoAlvo = publicoAlvoInvalido;
            Assert.Throws<ArgumentException>(() => _armazenadorDeCurso.Armazenar(_cursoDto)).ComMensagem("Publico alvo inválido");
        }
    }

    public interface ICursoRepositorio
    {
        void Adicionar(Curso curso);
    }

    public class ArmazenadorDeCurso
    {
        private readonly ICursoRepositorio _cursoRepositorio;

        public ArmazenadorDeCurso(ICursoRepositorio cursoRepositorio)
        {
            _cursoRepositorio = cursoRepositorio;
        }

        internal void Armazenar(CursoDto cursoDto)
        {
            Enum.TryParse(typeof(PublicoAlvo), cursoDto.PublicoAlvo, out var publicoAlvo);

            if (publicoAlvo == null)
            {
                throw new ArgumentException("Publico alvo inválido");
            }

            var curso = new Curso(cursoDto.Nome, cursoDto.CargaHoraria, (PublicoAlvo)publicoAlvo, cursoDto.Valor, cursoDto.Descricao);
            _cursoRepositorio.Adicionar(curso);
        }
    }

    public class CursoDto
    {     

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double CargaHoraria { get; set; }
        public string PublicoAlvo { get; set; }
        public double Valor { get; set; }
    }
}
