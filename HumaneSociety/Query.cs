using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();

            return allStates;
        }

        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }
        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = null;

            try
            {
                clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("No clients have a ClientId that matches the Client passed in.");
                Console.WriteLine("No update have been made.");
                return;
            }

            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;

            // submit changes
            db.SubmitChanges();
        }

        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }
        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName != null;
        }
        internal static void RunEmployeeQueries(Employee employee, string crudOperation)
        {
            switch (crudOperation)
            {
                case "Create":
                    CreateNewEmployee();
                    break;
                case "Read":
                    Console.WriteLine(employee.FirstName, employee.LastName, employee.UserName, employee.Email);
                    db.SubmitChanges();
                    break;
                case "Update":
                    UpdateEmployee(employee);
                    break;
                case "Delete":
                    db.Employees.DeleteOnSubmit(employee);
                    db.SubmitChanges();
                    break;
            } 
        }
        internal static void CreateNewEmployee()
        {
            Employee newEmployee = new Employee();
            newEmployee.FirstName = UserInterface.GetStringData("first name", "the employee's");
            newEmployee.LastName = UserInterface.GetStringData("last name", "the employee's");
            newEmployee.EmployeeNumber = int.Parse(UserInterface.GetStringData("employee number", "the employee's"));
            newEmployee.Email = UserInterface.GetStringData("email", "the employee's");
            db.Employees.InsertOnSubmit(newEmployee);
            db.SubmitChanges();
        }
        internal static void UpdateEmployee(Employee employee)
        {
            Employee employeeFromDb = null;
            try
            {
                employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).Single();
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Invalid EmployeeId");
                Console.WriteLine("No update have been made.");
            }
            employeeFromDb.FirstName = employee.FirstName;
            employeeFromDb.LastName = employee.LastName;
            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.UserName;
            employeeFromDb.Email = employee.Email;
            db.SubmitChanges();
        }
        internal static void AddAnimal(Animal animal)
        {
            string animalCategoryName = UserInterface.GetStringData("category/breed", "the name of the animal's");
            string animalDietPlanName = UserInterface.GetStringData("diet plan", "the name of the animal's");

            Animal newAnimal = new Animal();
            animal.CategoryId = Query.GetCategoryId(animalCategoryName);
            animal.Name = UserInterface.GetStringData("name", "the animal's");
            animal.Age = UserInterface.GetIntegerData("age", "the animal's");
            animal.Demeanor = UserInterface.GetStringData("demeanor", "the animal's");
            animal.KidFriendly = UserInterface.GetBitData("the animal", "child friendly");
            animal.PetFriendly = UserInterface.GetBitData("the animal", "pet friendly");
            animal.Weight = UserInterface.GetIntegerData("the animal", "the weight of the");
            animal.DietPlanId = Query.GetDietPlanId(animalDietPlanName);
            Query.AddAnimal(animal);
            db.SubmitChanges();
        }
        internal static Animal GetAnimalByID(int id)
        {
            return db.Animals.SingleOrDefault(b => b.AnimalId == id);
        }
        internal static void UpdateAnimal(int animalId, Dictionary<int, string> updates)
        {
            Animal animalFromDb = null;
            try
            {
                animalFromDb = db.Animals.Where(g => g.AnimalId == animalId).SingleOrDefault();
            }
            catch (InvalidOperationException g)
            {
                Console.WriteLine("No clients have a ClientId that matches the Client passed in.");
                Console.WriteLine("No update have been made.");
                return;
            }
            foreach (KeyValuePair<int, string> value in updates)
            {
                switch (value.Key)
                {
                    case 1:
                        animalFromDb.Category.Name = value.Value;
                        break;
                    case 2:
                        animalFromDb.Name = value.Value;
                        break;
                    case 3:
                        animalFromDb.Age = int.Parse(value.Value);
                        break;
                    case 4:
                        animalFromDb.Demeanor = value.Value;
                            break;
                    case 5:
                        animalFromDb.KidFriendly = Convert.ToBoolean(value.Value);
                        break;
                    case 6:
                        animalFromDb.PetFriendly = Convert.ToBoolean(value.Value);
                        break;
                   case 7:
                        animalFromDb.Weight = int.Parse(value.Value);
                        break;
                    default:
                        break;
                }
            }
            db.SubmitChanges();
        }
        internal static void RemoveAnimal(Animal animal)
        {
            db.Animals.DeleteOnSubmit(animal);
            db.SubmitChanges();
        }
        internal static List<Animal> SearchForAnimalsByMultipleTraits(Dictionary<int, string> updates) // parameter(s)?
        {
            List<Animal> animals = db.Animals.ToList();
            foreach (KeyValuePair<int, string> update in updates)
            {
                switch (update.Key)
                {
                    case 1:
                        animals = animals.Where(s => s.CategoryId == Convert.ToInt32(update.Value)).ToList();
                        break;
                    case 2:
                        animals = animals.Where(s => s.Name == (update.Value)).ToList();
                        break;
                    case 3:
                        animals = animals.Where(s => s.Age == Convert.ToInt32(update.Value)).ToList();
                        break;
                    case 4:
                        animals = animals.Where(s => s.Demeanor == (update.Value)).ToList();
                        break;
                    case 5:
                        animals = animals.Where(s => s.KidFriendly == Convert.ToBoolean(update.Value)).ToList();
                        break;
                    case 6:
                        animals = animals.Where(s => s.PetFriendly == Convert.ToBoolean(update.Value)).ToList();
                        break;
                    case 7:
                        animals = animals.Where(s => s.Weight == Convert.ToInt32(update.Value)).ToList();
                        break;
                    case 8:
                        animals = animals.Where(s => s.AnimalId == Convert.ToInt32(update.Value)).ToList();
                        break;
                    default:
                        break;
                }
            }
            return animals;
        }
        internal static int GetCategoryId(string categoryName)
        {
            var theintneeded = db.Categories.Where(a => a.Name == categoryName).Select(a => a.CategoryId).FirstOrDefault();
            return theintneeded;
        }
        internal static Room GetRoom(int animalId)
        {
            Room room = db.Rooms.Where(r => r.AnimalId == animalId).Single();
            return room;
        }
        internal static int GetDietPlanId(string dietPlanName)
        {
            throw new NotImplementedException();
        }
        internal static void Adopt(Animal animal, Client client)
        {
            throw new NotImplementedException();
        }
        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            throw new NotImplementedException();
        }
        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            throw new NotImplementedException();
        }
        internal static void RemoveAdoption(int animalId, int clientId)
        {
            throw new NotImplementedException();
        }
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
            throw new NotImplementedException();
        }
        internal static void UpdateShot(string shotName, Animal animal)
        {
            throw new NotImplementedException();
        }
    }
}