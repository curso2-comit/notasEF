namespace Notas.Models
{
    public class Nota
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Cuerpo { get; set; }
        public Usuario Creador { get; set; }
    }
}