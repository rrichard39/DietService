using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DietService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDietService" in both code and config file together.
    [ServiceContract]
    public interface IDietService
    {
        [OperationContract]
        string GetFoodName(int id);

        [OperationContract]
        int GetFoodCalories(int id);

        [OperationContract]
        int GetNextRecNum(string table);

        [OperationContract]
        ServerData GetFoodList();

        [OperationContract]
        ServerData GetWeight();

        [OperationContract]
        double GetDailyTotal(string date);

        [OperationContract]
        ServerData GetMeals(string date);

        [OperationContract]
        ServerData GetPersonalData();

        [OperationContract]
        void AddDailyFoodItem(string foodName, double qty);

        [OperationContract]
        void AddDailyFood(string foodName, double qty);

        [OperationContract]
        void AddNewFood(string foodName, int calories);

        [OperationContract]
        void AddWeight(string date, double newWeight);

        [OperationContract]
        void AddPersonalData(string name, float height, float iWeight, float tWeight, string ssid);

        [OperationContract]
        void DeletePersonalData(string name);
    }

    [DataContract]
    public class ServerData
    {
        [DataMember]
        public List<Food> FoodList;
        [DataMember]
        public List<Weight> WeightList;
        [DataMember]
        public List<MealItem> MealItems;
        [DataMember]
        public List<User> User;
    }

    [DataContract]
    public class MealItem
    {
        [DataMember]
        public string Food { get; set; }
        [DataMember]
        public int Calories { get; set; }
        [DataMember]
        public double Quantity { get; set; }
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "Diet_Web_Service.ContractType".
    //
    // This would allow you to return a class object - EXAMPLE
    //
    // [DataContract]
    // public class Product
    // {
    //     [DataMember]
    //     public int ProductID { get; set; }
    //     [DataMember]
    //     public string ProductName { get; set; }
    //     [DataMember]
    //     public string QuantityPerUnit { get; set; }
    //     [DataMember]
    //     public decimal UnitPrice { get; set; }
    //     [DataMember]
    //     public bool Discontinued { get; set; }
    // }

}
