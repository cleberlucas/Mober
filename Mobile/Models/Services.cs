namespace Mobile.Models
{
    public static class Services
    {
        public static readonly string Borracheiro = "Borracheiro - 2 Disponíveis";
        public static readonly string Eletricista = "Eletricista - 3 Disponíveis";
        public static readonly string Encanador = "Encanador  - 1 Disponíveis";
        public static readonly string Diarista = "Diarista - 0 Disponíveis";
        public static readonly string Pintor = "Pintor - 1 Disponíveis";

        public static readonly List<string> List = new List<string>
        {
            Borracheiro,
            Eletricista,
            Encanador,
            Diarista,
            Pintor,
        };
    }
}