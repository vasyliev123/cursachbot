using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace telegramBotproga.Models
{
    public class MongoDBModel
    {
        public ObjectId Id { get; set; }


        public string ID { get; set; }
        //public Dictionary<string,string> Favorites { get; set; }
        public string artist { get; set; }

        public string title { get; set; }
    }
}
