using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;

namespace YummyMummy.Models
{
	//ERD, one entity, one class
	//Review Class
	public class Inquiry
	{
		public int ID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public int Telephone { get; set; }
		public string Message { get; set; }

		public Dictionary<string, string> Dict
		{
			get
			{
				return this.formfields;
			}
		}
		private Dictionary<string, string> formfields;
		private void initDict()
		{
			this.formfields = new Dictionary<string, string>();
			this.formfields.Add("ID", "Review ID");
			this.formfields.Add("FirstName", "First Name");
			this.formfields.Add("LastName", "Last Name");
			this.formfields.Add("Email", "Email");
			this.formfields.Add("Telephone", "Telephone");
			this.formfields.Add("Message", "Message");
		}

		// Review constructor1 
		public Inquiry()
		{
			this.initDict();
		}

		// Review constructor2	
		public Inquiry(string FirstName, string LastName, string Email, int Telephone,string Message)
		{
			this.initDict();
			this.FirstName = FirstName;
			this.LastName = LastName;
			this.Email = Email;
			this.Telephone = Telephone;
			this.Message = Message;
		}
	}//end of Review Class	

	
}