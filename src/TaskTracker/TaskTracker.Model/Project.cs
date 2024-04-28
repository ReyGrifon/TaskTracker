using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Model
{
    public class Project
    {
        public List<Task> Tasks { get; set; } = new();

        /// <summary>
        /// Поиск контакта по подстроке одного из полей.
        /// </summary>
        /// <param name="query">Записываемая пользователем строка</param>
        /// <returns></returns>
        public List<Task> FindContacts(string query)
        {
            if (query == null)
            {
                return Tasks;
            }
            var options = StringComparison.OrdinalIgnoreCase;
            return Tasks.Where(c =>
                c.Name.Contains(query, options)).ToList();
        }
    }
}
