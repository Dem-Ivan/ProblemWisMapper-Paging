using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationAPI15_SecondStageTS_.dto
{
	public class AnnouncementDTO
	{
		public int Id { get; set; }
		public int OrderNumber { get; set; }//обезательный
		public int UserId { get; set; }// !		
		public string Text { get; set; }// задать длину поля
		public string Image { get; set; }//!какк??
		public int Rating { get; set; }
		public DateTime CreationDate { get; set; }
		public UserDTO user { get; set; }//!
	}
}
