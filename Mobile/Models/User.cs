using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Models
{
    //[Table("usuario")]
    public class User
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Column("id")]
        //public int Id { get; set; }

        //[Column("nome")]
        public string Name { get; set; }

        //[Column("telefone")]
        public string Phone { get; set; }

        //[Column("longitude")]
        public double Longitude { get; set; }

        //[Column("latitude")]
        public double Latitude { get; set; }

        //[Column("servico")]
        public string Service { get; set; }

        //[Column("prestador")]
        public bool Servant { get; set; }

        //[Column("atualizado")]
        public DateTime Updated { get; set; }
    }
}
