using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationAPI15_SecondStageTS_.Models
{
	public class Announcement
	{
		//указать атрибут на первичный ключ
		public int Id { get; set; }
		public int OrderNumber { get; set; }//обезательный

		//указать атрибут на внений ключ, проверить какие поля обязательные 
		public int UserId { get; set; }// !
		public User user { get; set; }//!
		public string Text { get; set; }// задать длину поля
		public string Image { get; set; }//!какк??
		public int Rating { get; set; }
		public DateTime CreationDate { get; set; }
	}
}
