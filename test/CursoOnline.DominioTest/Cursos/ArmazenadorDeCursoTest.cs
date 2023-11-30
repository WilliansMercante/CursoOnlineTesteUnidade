
using CursoOnline.Dominio.Cursos;
using CursoOnline.Dominio.Cursos.Enums;

using Moq;

namespace CursoOnline.DominioTest.Cursos
{
    public class ArmazenadorDeCursoTest
    {
        [Fact]
        public void DeveAdicionarCurso()
        {
            var cursoDto = new CursoDto()
            {
                Nome = "Curso A",
                Descricao = "Descrição",
                CargaHoraria = 80,
                IdPublicoAlvo = 1,
                Valor = 850.00
            };

            var cursoRepositorioMock = new Mock<ICursoRepositorio>();

            var armazenadorDeCurso = new ArmazenadorDeCurso(cursoRepositorioMock.Object);
            armazenadorDeCurso.Armazenar(cursoDto);

            cursoRepositorioMock.Verify(p => p.Adicionar(It.IsAny<Curso>()));
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
            var curso = new Curso(cursoDto.Nome,cursoDto.CargaHoraria,PublicoAlvo.Estudante,cursoDto.Valor,cursoDto.Descricao);
            _cursoRepositorio.Adicionar(curso);
        }
    }

    public class CursoDto
    {
        public CursoDto()
        {
        }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int CargaHoraria { get; set; }
        public int IdPublicoAlvo { get; set; }
        public double Valor { get; set; }
    }
}
