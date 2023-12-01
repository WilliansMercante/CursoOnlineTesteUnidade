namespace CursoOnline.Dominio.Cursos.Interfaces
{
    public interface ICursoRepositorio
    {
        void Adicionar(Curso curso);
        Curso ObterPeloNome(string nome);
    }
}
