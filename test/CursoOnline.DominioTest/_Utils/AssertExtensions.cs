namespace CursoOnline.DominioTest._Utils
{
    internal static class AssertExtensions
    {
        public static void ComMensagem(this ArgumentException exception,string mensagem)
        {
            if (exception.Message == mensagem) 
            {
                Assert.True(true);
            }
            else
            {
                Assert.False(true,"Esperava a mensagem: **" + mensagem + "**, porém veio a mensagem: **" + exception.Message + "**");
            }
        }
    }
}
