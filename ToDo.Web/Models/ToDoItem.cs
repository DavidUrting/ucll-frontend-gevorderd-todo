using System;

namespace ToDo.Web.Models
{
    public class ToDoItem
    {
        private static int _nexId = 0;

        /// <summary>
        /// Met deze 'helper' methode kan de controller unieke ID's genereren voor ToDoItem's
        /// die via de POST binnenkomen.
        /// </summary>
        /// <returns>Unieke ID. </returns>
        public static int GetNextId()
        {
            return _nexId++;
        }

        public int Id { get; set; }

        public string Titel { get; set; }
        public string Omschrijving { get; set; }
        public string ToegewezenAan { get; set; }
        public DateTime? Deadline { get; set; }
    }
}
