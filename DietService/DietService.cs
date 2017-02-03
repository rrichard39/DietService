using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace DietService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DietService" in both code and config file together.
    public class DietService : IDietService
    {
        Timer KeepAliveTimer;
        public DietService()
        {
            KeepAliveTimer = new Timer(60000);
            KeepAliveTimer.Elapsed += new ElapsedEventHandler(_timerElapsed);
            KeepAliveTimer.Enabled = true;
            KeepAliveTimer.Start();
        }

        public void _timerElapsed(object sender, ElapsedEventArgs e)
        {
            DietEntities db = new DietEntities();
            int count = (from item in db.Foods
                         where item.calories > 0
                         select item).Count();
            db.Dispose();
        }

        public string GetFoodName(int id)
        {
            // TODO: retrieve the real product info from DB using EF
            DietEntities db = new DietEntities();

            string item = (from items in db.Foods
                           where items.recNum == id
                           select items.food1).First();
            db.Dispose();
            return item;
        }

        public int GetFoodCalories(int id)
        {
            // TODO: retrieve the real product info from DB using EF
            DietEntities db = new DietEntities();

            int item = (from items in db.Foods
                        where items.recNum == id
                        select items.calories).FirstOrDefault().Value;
            db.Dispose();
            return item;
        }

        private int GetFoodIndex(string food)
        {
            DietEntities db = new DietEntities();

            int item = (from items in db.Foods
                           where items.food1 == food
                           select items.recNum).FirstOrDefault();
            db.Dispose();
            return item;
        }

        public int GetNextRecNum(string table)
        {
            DietEntities db = new DietEntities();
            int highest = 0;

            switch (table)
                {
                case "Food":
                    {
                        var records = (from items in db.Foods
                                       group items by true into r
                                       select new
                                       {
                                           max = r.Max(z => z.recNum)
                                       });
                        foreach (var r in records)
                        {
                            highest = r.max;
                        }
                        break;
                    }
                case "Daily":
                    {
                        var records = (from items in db.Dailies
                                       group items by true into r
                                       select new
                                       {
                                           max = r.Max(z => z.recNum)
                                       });
                        foreach (var r in records)
                        {
                            highest = (int)r.max;
                        }
                        break;
                    }
                case "Weight":
                    {
                        var records = (from items in db.Weights
                                       group items by true into r
                                       select new
                                       {
                                           max = r.Max(z => z.recNum)
                                       });
                        foreach (var r in records)
                        {
                            highest = r.max;
                        }
                        break;
                    }
                default:
                    break;

            }

            db.Dispose();
            return highest + 1;
        }

        public ServerData GetFoodList()
        {
            DietEntities db = new DietEntities();

            ServerData sd = new ServerData();
            sd.FoodList = new List<Food>();
            IEnumerable<Food> foodList = (from items in db.Foods
                            orderby items.food1 
                            select items);
            foreach (Food item in foodList)
                if (!(item == null))
                    sd.FoodList.Add(item);
            return sd;
        }

        public double GetDailyTotal(string date)
        {
            DietEntities db = new DietEntities();
            double total = 0;

            DateTime sqlDate = new DateTime();

            sqlDate = DateTime.Parse(date);

            var rec = (from item in db.Dailies
                       where item.day_of_week == sqlDate
                       select item);

            foreach (var item in rec)
            {
                total += (Convert.ToDouble(GetFoodCalories((int)item.food)) * (double)item.qty);
            }


                //foreach (var item in rec)
                //{
                //    if (item.food1.HasValue)
                //    {
                //        total += (Convert.ToDouble(GetFoodCalories(item.food1.Value)) * item.qty1.Value);
                //    }
                //    else
                //    {
                //        break;
                //    }
                //    if (item.food2.HasValue)
                //    {
                //        total += (Convert.ToDouble(GetFoodCalories(item.food2.Value)) * item.qty2.Value);
                //    }
                //    else
                //    {
                //        break;
                //    }
                //    if (item.food3.HasValue)
                //    {
                //        total += (Convert.ToDouble(GetFoodCalories(item.food3.Value)) * item.qty3.Value);
                //    }
                //    else
                //    {
                //        break;
                //    }
                //    if (item.food4.HasValue)
                //    {
                //        total += (Convert.ToDouble(GetFoodCalories(item.food4.Value)) * item.qty4.Value);
                //    }
                //    else
                //    {
                //        break;
                //    }
                //    if (item.food5.HasValue)
                //    {
                //        total += (Convert.ToDouble(GetFoodCalories(item.food5.Value)) * item.qty5.Value);
                //    }
                //    else
                //    {
                //        break;
                //    }
                //    if (item.food6.HasValue)
                //    {
                //        total += (Convert.ToDouble(GetFoodCalories(item.food6.Value)) * item.qty6.Value);
                //    }
                //    else
                //    {
                //        break;
                //    }
                //    if (item.food7.HasValue)
                //    {
                //        total += (Convert.ToDouble(GetFoodCalories(item.food7.Value)) * item.qty7.Value);
                //    }
                //    else
                //    {
                //        break;
                //    }
                //    if (item.food8.HasValue)
                //    {
                //        total += (Convert.ToDouble(GetFoodCalories(item.food8.Value)) * item.qty8.Value);
                //    }
                //    else
                //    {
                //        break;
                //    }
                //    if (item.food9.HasValue)
                //    {
                //        total += (Convert.ToDouble(GetFoodCalories(item.food9.Value)) * item.qty9.Value);
                //    }
                //    else
                //    {
                //        break;
                //    }
                //    if (item.food10.HasValue)
                //    {
                //        total += (Convert.ToDouble(GetFoodCalories(item.food10.Value)) * item.qty10.Value);
                //    }
                //    else
                //    {
                //        break;
                //    }
                //}
                db.Dispose();
            return total;
        }

        public ServerData GetMeals(string date) //change to Datetime
        {
            DietEntities db = new DietEntities();
            ServerData sd = new ServerData();
            DateTime sqlDate = new DateTime();

            sqlDate = DateTime.Parse(date);
            sd.MealItems = new List<MealItem>();

            var mealItems = (from items in db.Dailies
                            where items.day_of_week == sqlDate
                            select items);

            foreach (Daily item in mealItems)
            {
                MealItem mi = new MealItem();
                mi.Food = GetFoodName((int)item.food);
                mi.Calories = GetFoodCalories((int)item.food);
                mi.Quantity = (double)item.qty;
                sd.MealItems.Add(mi);
            }


                //foreach (Daily item in mealItems)
                //{
                //    if (item.food1.HasValue)
                //    {
                //        MealItem mi = new MealItem();
                //        mi.Food = GetFoodName(item.food1.Value);
                //        mi.Calories = GetFoodCalories(item.food1.Value);
                //        mi.Quantity = item.qty1.Value;
                //        sd.MealItems.Add(mi);
                //    }
                //    if (item.food2.HasValue)
                //    {
                //        MealItem mi = new MealItem();
                //        mi.Food = GetFoodName(item.food2.Value);
                //        mi.Calories = GetFoodCalories(item.food2.Value);
                //        mi.Quantity = item.qty2.Value;
                //        sd.MealItems.Add(mi);
                //    }
                //    if (item.food3.HasValue)
                //    {
                //        MealItem mi = new MealItem();
                //        mi.Food = GetFoodName(item.food3.Value);
                //        mi.Calories = GetFoodCalories(item.food3.Value);
                //        mi.Quantity = item.qty3.Value;
                //        sd.MealItems.Add(mi);
                //    }
                //    if (item.food4.HasValue)
                //    {
                //        MealItem mi = new MealItem();
                //        mi.Food = GetFoodName(item.food4.Value);
                //        mi.Calories = GetFoodCalories(item.food4.Value);
                //        mi.Quantity = item.qty4.Value;
                //        sd.MealItems.Add(mi);
                //    }
                //    if (item.food5.HasValue)
                //    {
                //        MealItem mi = new MealItem();
                //        mi.Food = GetFoodName(item.food5.Value);
                //        mi.Calories = GetFoodCalories(item.food5.Value);
                //        mi.Quantity = item.qty5.Value;
                //        sd.MealItems.Add(mi);
                //    }
                //    if (item.food6.HasValue)
                //    {
                //        MealItem mi = new MealItem();
                //        mi.Food = GetFoodName(item.food6.Value);
                //        mi.Calories = GetFoodCalories(item.food6.Value);
                //        mi.Quantity = item.qty6.Value;
                //        sd.MealItems.Add(mi);
                //    }
                //    if (item.food7.HasValue)
                //    {
                //        MealItem mi = new MealItem();
                //        mi.Food = GetFoodName(item.food7.Value);
                //        mi.Calories = GetFoodCalories(item.food7.Value);
                //        mi.Quantity = item.qty7.Value;
                //        sd.MealItems.Add(mi);
                //    }
                //    if (item.food8.HasValue)
                //    {
                //        MealItem mi = new MealItem();
                //        mi.Food = GetFoodName(item.food8.Value);
                //        mi.Calories = GetFoodCalories(item.food8.Value);
                //        mi.Quantity = item.qty8.Value;
                //        sd.MealItems.Add(mi);
                //    }
                //    if (item.food9.HasValue)
                //    {
                //        MealItem mi = new MealItem();
                //        mi.Food = GetFoodName(item.food9.Value);
                //        mi.Calories = GetFoodCalories(item.food9.Value);
                //        mi.Quantity = item.qty9.Value;
                //        sd.MealItems.Add(mi);
                //    }
                //    if (item.food10.HasValue)
                //    {
                //        MealItem mi = new MealItem();
                //        mi.Food = GetFoodName(item.food10.Value);
                //        mi.Calories = GetFoodCalories(item.food10.Value);
                //        mi.Quantity = item.qty10.Value;
                //        sd.MealItems.Add(mi);
                //    }
                //}
                return sd;
        }

        public ServerData GetWeight()
        {
            DietEntities db = new DietEntities();
            ServerData sd = new ServerData();
            sd.WeightList = new List<Weight>();
            var weightList = (from items in db.Weights
                            orderby items.recNum
                            select items);
            foreach (Weight item in weightList)
                if (!(item == null))
                    sd.WeightList.Add(item);
            return sd;
        }

        public ServerData GetPersonalData()
        {
            DietEntities db = new DietEntities();
            ServerData sd = new ServerData();
            
            sd.User = new List<User>();

            var personalData = (from items in db.Users
                                select items);
            foreach (User item in personalData)
            {
                if (item.name != null)
                {
                    sd.User.Add(item);
                }
            }
                return sd;
        }

        public void AddDailyFoodItem(string foodName, double qty)
        {
            DietEntities db = new DietEntities();
            Daily daily = new Daily();

            DateTime date;
            date = DateTime.Today.Date;

            int foodIndex = GetFoodIndex(foodName);

            //string today = now.ToString("MM/dd/yyyy");
            int nextRecNum = 0;

            //var count = (from item in db.Dailies
            //             select item);
            //foreach (var item in count)
            //{
            //    if (item.recNum > nextRecNum)
            //        nextRecNum = (int)item.recNum;
            //}

            nextRecNum = int.Parse(db.Dailies
                                        .OrderByDescending(p => p.recNum)
                                        .Select(r => r.recNum)
                                        .First().ToString());


            nextRecNum++;
            daily.day_of_week = date;
            daily.recNum = nextRecNum;
            daily.food = foodIndex;
            daily.qty = (float)qty;


            //var rec = (from item in db.Dailies
            //           where item.day_of_week == sqlDate
            //           select item);

            //foreach (var item in rec)
            //{
            //    daily.recNum = item.recNum;
            //    daily.day_of_week = item.day_of_week;
            //    daily = item;
            //        daily.food = foodIndex;
            //        daily.qty = (float)qty;
            //        break;
            //}



            //foreach (var item in rec)
            //{
            //    daily.recNum = item.recNum;
            //    daily.day_of_week = item.day_of_week;
            //    daily = item;
            //    if (!item.food1.HasValue)
            //    {
            //        daily.food1 = foodIndex;
            //        daily.qty1 = (float)qty;
            //        break;
            //    }
            //    if (!item.food2.HasValue)
            //    {
            //        daily.food2 = foodIndex;
            //        daily.qty2 = (float)qty;
            //        break;
            //    }
            //    if (!item.food3.HasValue)
            //    {
            //        daily.food3 = foodIndex;
            //        daily.qty3 = (float)qty;
            //        break;
            //    }
            //    if (!item.food4.HasValue)
            //    {
            //        daily.food4 = foodIndex;
            //        daily.qty4 = (float)qty;
            //        break;
            //    }
            //    if (!item.food5.HasValue)
            //    {
            //        daily.food5 = foodIndex;
            //        daily.qty5 = (float)qty;
            //        break;
            //    }
            //    if (!item.food6.HasValue)
            //    {
            //        daily.food6 = foodIndex;
            //        daily.qty6 = (float)qty;
            //        break;
            //    }
            //    if (!item.food7.HasValue)
            //    {
            //        daily.food7 = foodIndex;
            //        daily.qty7 = (float)qty;
            //        break;
            //    }
            //    if (!item.food8.HasValue)
            //    {
            //        daily.food8 = foodIndex;
            //        daily.qty8 = (float)qty;
            //        break;
            //    }
            //    if (!item.food9.HasValue)
            //    {
            //        daily.food9 = foodIndex;
            //        daily.qty9 = (float)qty;
            //        break;
            //    }
            //    if (!item.food10.HasValue)
            //    {
            //        daily.food10 = foodIndex;
            //        daily.qty10 = (float)qty;
            //        break;
            //    }
            //}

            try
            {
                db.Dailies.Add(daily);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException);
            }

            //var original = db.Dailies.Find(daily.recNum);

            //if (original != null)
            //{
            //    db.Entry(original).CurrentValues.SetValues(daily);
            //    db.SaveChanges();
            //}
            //else
            //{
            //    daily = new Daily();
            //    daily.recNum = nextRecNum;
            //    daily.day_of_week = DateTime.Parse(date).Date;
            //    daily.food = foodIndex;
            //    daily.qty = (float)qty;
            //    try
            //    {
            //        db.Dailies.Add(daily);
            //        db.SaveChanges();
            //    }
            //    catch (Exception e)
            //    {
            //        Debug.WriteLine(e.InnerException);
            //    }
            //}
            db.Dispose();
            AddDailyFood(foodName, qty);
        }

        public void AddDailyFood(string foodName, double qty)
        {
            DietEntities db = new DietEntities();
            DailyMeal dailyMeal = new DailyMeal();

            DateTime date;
            date = DateTime.Today.Date;

            string currentDate;
            string yesterday;
            bool sameday;
            double dailyTotal;
            int lastRecNum;

            DateTime now = DateTime.Now;
            currentDate = now.ToString("MM/dd/yyyy");
            yesterday = now.AddDays(-1).ToString("MM/dd/yyyy");

            int foodIndex = GetFoodIndex(foodName);

            sameday = db.DailyMeals.Any(entry => entry.dayDate.Equals(date));
            // if any record's column 'daydate' equals 'date' from this method's arguement list
            // 'entry' is a local LINQ arguement referring to a record in the table
            // see https://www.dotnetperls.com/any

            lastRecNum = db.WeeklyCalories.Count();

            if (!sameday)
            {
                // total all calories for week and enter in WeeklyCalories
                if (now.ToString("dddd") == "Monday")
                {
                    dailyTotal = SumDailyCalories();

                    WeeklyCalory wc = new WeeklyCalory();
                    wc.recNum = lastRecNum + 1;
                    wc.weekEnding = yesterday;
                    wc.totalCalories = (float)dailyTotal;
                    
                    try
                    {
                        db.WeeklyCalories.Add(wc);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.InnerException);
                    }

                    // clear DailyMeals

                    // Alternate Method
                    // **************************************************
                    //string connstr = "Provider=SQLOLEDB;"
                    //    + "Data Source = .\\SQLEXPRESS;"
                    //    + "Initial Catalog =Diet;"
                    //    + "Integrated Security=SSPI";
                    //DataContext context = new DataContext(connstr);
                    //context.ExecuteCommand("TRUNCATE TABLE DailyMeals");
                    // **************************************************

                    var allRecords = (from item in db.DailyMeals
                                      select item);
                    foreach (DailyMeal item in allRecords)
                    {
                        db.DailyMeals.Remove(item);
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.InnerException);
                    }
                    lastRecNum = 0;
                }
            }

            // now add the latest entry
            dailyMeal.recNum = lastRecNum + 1;
            dailyMeal.dayDate = date;
            dailyMeal.food = foodIndex;
            dailyMeal.quantity = (float)qty;
            try
            {
                db.DailyMeals.Add(dailyMeal);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException);
            }
            db.Dispose();
        }

        public void AddNewFood(string foodName, int calories)
        {
            DietEntities db = new DietEntities();
            int nextRecNum = 0;
            var records = (from items in db.Foods
                           select items);

            foreach (var item in records)
            {
                if (item.recNum > nextRecNum)
                    nextRecNum = item.recNum;
            }
            nextRecNum++;

            Food food = new Food();
            food.recNum = nextRecNum;
            food.food1 = foodName;
            food.calories = calories;
            try
            {
                db.Foods.Add(food);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException);
            }
            db.Dispose();
        }

        public void AddWeight(string date, double newWeight)
        {
            DietEntities db = new DietEntities();
            int nextRecNum = 0;
            var records = (from items in db.Weights
                           select items);

            foreach (var item in records)
            {
                if (item.recNum > nextRecNum)
                    nextRecNum = item.recNum;
            }
            nextRecNum++;

            Weight weight = new Weight();
            weight.recNum = nextRecNum;
            weight.measureDate = date;
            weight.weight1 = (float)newWeight;
            try
            {
                db.Weights.Add(weight);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException);
            }
            db.Dispose();
        }

        public void AddPersonalData(string name, float height, float iWeight, float tWeight, string ssid)
        {
            DietEntities db = new DietEntities();

            User pd = new User();
            pd.name = name;
            pd.height = height;
            pd.initialWeight = iWeight;
            pd.targetWeight = tWeight;
            pd.SSID = ssid;
            try
            {
                db.Users.Add(pd);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException);
            }
            db.Dispose();
        }

        public void DeletePersonalData(string name){
            DietEntities db = new DietEntities();

            //db.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.Users");
            var rec =
                from records in db.Users
                where records.name == name
                select records;

            foreach (var delrec in rec)
            {
                db.Users.Remove(delrec);
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Provide for exceptions.
            }
        }

        private double SumDailyCalories()
        {
            DietEntities db = new DietEntities();
            double calories = 0;

            var items = (from item in db.DailyMeals
                         select item);

            foreach (DailyMeal i in items)
            {
                calories += GetFoodCalories((int)i.food) * (double)i.quantity; 
            }

            return calories;
        }

        private void TEST()
        {
            DietEntities db = new DietEntities();
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE User");

        }
    }
}
