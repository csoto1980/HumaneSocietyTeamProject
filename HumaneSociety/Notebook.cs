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
            private void AddAnimal()
            Console.Clear();

            string animalCategoryName = UserInterface.GetStringData("category/breed", "the name of the animal's");
            string animalDietPlanName = UserInterface.GetStringData("diet plan", "the name of the animal's");

            Animal animal = new Animal();
            animal.CategoryId = Query.GetCategoryId(animalCategoryName);
            animal.Name = UserInterface.GetStringData("name", "the animal's");
            animal.Age = UserInterface.GetIntegerData("age", "the animal's");
            animal.Demeanor = UserInterface.GetStringData("demeanor", "the animal's");
            animal.KidFriendly = UserInterface.GetBitData("the animal", "child friendly");
            animal.PetFriendly = UserInterface.GetBitData("the animal", "pet friendly");
            animal.Weight = UserInterface.GetIntegerData("the animal", "the weight of the");
            animal.DietPlanId = Query.GetDietPlanId(animalDietPlanName);
            Query.AddAnimal(animal);
        }
    }
 }
