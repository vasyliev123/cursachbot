using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Driver;
using telegramBotproga.Models;

namespace telegramBotproga.Clients
{
    public class MongoDBClient
    {
        public MongoClient client;
        public IMongoDatabase db;
        public IMongoCollection<MongoDBModel> dbCollection;
        public MongoDBClient()
        {

            try
            {


                client = new MongoClient("mongodb+srv://CursachVasyliev:64287139Dima@cluster0.v56tybp.mongodb.net/?retryWrites=true&w=majority");
                db = client.GetDatabase("Favorites");
                dbCollection = db.GetCollection<MongoDBModel>("Favorites");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}
