
using Bogus;

using CursoOnline.Dominio.Cursos;
using CursoOnline.Dominio.Cursos.Enums;
using CursoOnline.Dominio.Cursos.Interfaces;
using CursoOnline.DominioTest._Builders;
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
                ))
            );
        }

        [Fact]
        public void NaoDeveInformarPublicoAlvoInvalido()
        {
            var publicoAlvoInvalido = "Médico";
            _cursoDto.PublicoAlvo = publicoAlvoInvalido;
            Assert.Throws<ArgumentException>(() => _armazenadorDeCurso.Armazenar(_cursoDto)).ComMensagem("Publico alvo inválido");
        }

        [Fact]
        public void NaoDeveAdicionarCursoComMesmoNomeDeOutroJaSalvo()
        {
            var cursojaSalvo = CursoBuilder.Novo().ComNome(_cursoDto.Nome).Build();
            _cursoRepositorioMock.Setup(c => c.ObterPeloNome(_cursoDto.Nome)).Returns(cursojaSalvo);
            Assert.Throws<ArgumentException>(() => _armazenadorDeCurso.Armazenar(_cursoDto)).ComMensagem("Nome do curso já consta no banco de dados");
        }

    }






}
