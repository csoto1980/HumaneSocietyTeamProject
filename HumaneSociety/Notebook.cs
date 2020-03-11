using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    class Notebook
    {
        internal static void CreateNewEmployee()
        {
            public static void DisplayAnimalInfo(Animal animal)
            {
                Room animalRoom = Query.GetRoom(animal.AnimalId);
                List<string> info = new List<string>() { "ID: " + animal.AnimalId, animal.Name, animal.Age + "years old", "Demeanour: " + animal.Demeanor, "Kid friendly: " + BoolToYesNo(animal.KidFriendly), "pet friendly: " + BoolToYesNo(animal.PetFriendly), $"Location: " + animalRoom.RoomId, "Weight: " + animal.Weight.ToString(), "Food amoumnt in cups:" + animal.DietPlan.FoodAmountInCups };
                DisplayUserOptions(info);
                Console.ReadLine();

            }
            internal static Client GetClient(string userName, string password)
            {
                Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

                return client;
            }

        }
    }
 }
