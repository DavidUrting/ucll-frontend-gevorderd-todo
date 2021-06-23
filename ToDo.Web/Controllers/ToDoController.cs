using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ToDo.Web.Models;

namespace ToDo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        // Beter (ipv static variabelen) is natuurlijk een database gebruiken (via Entity Framework).
        // Hieronder wordt een 'Dictionary' gebruikt: dat is een structuur waarin je met een sleutel
        // op zoek kan gaan naar een overeenkomstige waarde. Je kan het zien als een woordenboek
        // waarbij de 'key' het woord is, en de 'value' is het de verklaring van dat woord.
        // In het geval van de ToDo applicatie is de sleutel een integer (= de sleutel) en een ToDoItem de value.
        private static Dictionary<int, ToDoItem> _toDoItems = new Dictionary<int, ToDoItem>();

        /// <summary>
        /// Dit is een 'static' constructor. 
        /// Een 'static' constructor wordt éénmalig (bij het laden van de class) uitgevoerd.
        /// Deze static constructor zorgt voor het toevoegen van een aantal dummy ToDo items...
        /// </summary>
        static ToDoController()
        {
            int id = ToDoItem.GetNextId();
            _toDoItems.Add(id, new ToDoItem()
            {
                Id = id,
                Titel = "ToDo opdracht implementeren",
                Omschrijving = "De herkaningsingsopdracht voor front end gevorderd implementeren",
                Deadline = new System.DateTime(2021, 8, 10, 23,59,59),
                ToegewezenAan = Environment.UserName,
                Afgewerkt = false
            });

            id = ToDoItem.GetNextId();
            _toDoItems.Add(id, new ToDoItem()
            {
                Id = id,
                Titel = "Verslag uitschrijven",
                Omschrijving = "Een verslag schrijven over je ervaring(en) met deze herkaningsopdracht.",
                Deadline = new System.DateTime(2021, 8, 10, 23, 59, 59),
                ToegewezenAan = Environment.UserName,
                Afgewerkt = false
            });

            id = ToDoItem.GetNextId();
            _toDoItems.Add(id, new ToDoItem()
            {
                Id = id,
                Titel = "Presentatie/demo voorbereiden",
                Omschrijving = "De herkaningsingsopdracht zal ook moeten gedemonstreerd worden.",
                Deadline = new System.DateTime(2021, 8, 10, 23, 59, 59),
                ToegewezenAan = Environment.UserName,
                Afgewerkt = false
            });
        }

        /// <summary>
        /// Ophalen van alle ToDo items via een GET naar /url/todo
        /// </summary>
        /// <returns>JSON met array van ToDoItem objecten.</returns>
        [HttpGet]
        public IEnumerable<ToDoItem> Get()
        {
            return _toDoItems.Values;
        }

        /// <summary>
        /// Ophalen van één specifieke ToDo item via een GET naar /url/todo/{id}.
        /// </summary>
        /// <param name="id">Een getal vanaf 0.</param>
        /// <returns>JSON met een ToDoItem object</returns>
        [HttpGet("{id}")]
        public ToDoItem Get(int id)
        {
            if (_toDoItems.ContainsKey(id))
            {
                return _toDoItems[id];
            }
            else return null;
        }

        /// <summary>
        /// Aanmaken (onthouden) van een nieuwe ToDo item komende van de front end.
        /// </summary>
        /// <param name="toDoItem">JSON met ToDoItem object. De Id van het object stel je best in op -1, maar eigenlijk doet het er niet echt toe.</param>
        /// <returns>JSON met daarin een number (het toegekende id).</returns>
        [HttpPost]
        public int Post([FromBody] ToDoItem toDoItem)
        {
            // Zoals je ziet is de backend verantwoordelijk voor het genereren van een ID.
            toDoItem.Id = ToDoItem.GetNextId(); // Unieke sleutel toekennen (normaal doet de database dat, of gebruik je een GUID).
            _toDoItems.Add(toDoItem.Id, toDoItem);
            return toDoItem.Id; // id terugsturen zodat de browser te weten kan komen welke ID werd toegekend.
        }

        /// <summary>
        /// Bijwerken van een reeds bestaande ToDoItem object.
        /// De backend zal op basis van het 'Id' op zoek gaan naar het overeenkomstige object in de dictionary.
        /// </summary>
        /// <param name="toDoItem">ToDoItem dat moet bijgewerkt worden. Indien het Id niet gevonden wordt gebeurt er niets.</param>
        [HttpPut()]
        public void Put([FromBody] ToDoItem toDoItem)
        {
            // PUT: bijwerken
            if (_toDoItems.ContainsKey(toDoItem.Id))
            {
                _toDoItems[toDoItem.Id].Titel = toDoItem.Titel;
                _toDoItems[toDoItem.Id].Omschrijving = toDoItem.Omschrijving;
                _toDoItems[toDoItem.Id].ToegewezenAan = toDoItem.ToegewezenAan;
                _toDoItems[toDoItem.Id].Deadline = toDoItem.Deadline;
                _toDoItems[toDoItem.Id].Afgewerkt = toDoItem.Afgewerkt;
            }            
        }

        /// <summary>
        /// Verwijderen van een ToDoItem met de specifieke Id.
        /// Indien er geen ToDoItem werd gevonden gebeurt er niets.
        /// </summary>
        /// <param name="id">JSON met daarin een number (het id).</param>
        [HttpDelete()]
        public void Delete([FromBody] int id)
        {
            if (_toDoItems.ContainsKey(id))
            {
                _toDoItems.Remove(id);
            }
        }
    }
}
