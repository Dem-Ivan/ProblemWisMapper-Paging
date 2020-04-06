using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationAPI15_SecondStageTS_.Models
{
	public class User
	{
		public int Id {get; set;}

		[Required(ErrorMessage = "Укажите имя пользователя")]
		public string Name { get; set; }
		public List<Announcement> Announcements { get; set; } = new List<Announcement>();
	}
}
