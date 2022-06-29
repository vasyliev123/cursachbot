using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace telegramBotproga.Models
{
    internal class ArtistModel
    {
        public List<Items>? Items {get;set;}

       
    }
    public class Items
    {
        public string Name { get;set;}
    }
}
