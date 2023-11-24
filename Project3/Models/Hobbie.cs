using Org.BouncyCastle.Bcpg;
using Project3.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project3.Models
{
	public class Hobbie : BaseEntity
	{
		[ForeignKey("User")]
		public int UserId {  get; set; }
		public string? detai {  get; set; }
        [ForeignKey("TypeHobbie")]
        public int TypeId { get; set; }
		public TypeHobbie? TypeHobbie { get; set; }


	}
}
