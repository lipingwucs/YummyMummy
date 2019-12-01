using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YummyMummy.Data.Enums;
using YummyMummy.Models;


namespace YummyMummy.Services
{
	public interface IMenu
	{
		void CreateMenu(Menu menu);
		Menu GetById(int ID);
		IEnumerable<Menu> GetByUserId(string userId);
		IEnumerable<Menu> GetAll();
		IEnumerable<Menu> GetFilteredMenus(
			string userId = null,
			OrderBy orderBy = OrderBy.None,
			int offset = 0,
			int limit = 10,
			double? minimalPrice = null,
			double? maximalPrice = null,
			DateTime? minDate = null,
			DateTime? maxDate = null,
			string description = null
			);
	}
}