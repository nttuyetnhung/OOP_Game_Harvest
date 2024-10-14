using System;
using System.Collections.Generic;
using System.Text;

namespace oop_game_harvest
{
    // Exception khi không đủ điểm thưởng
    class NotEnoughRewardException : Exception
    {
        public NotEnoughRewardException(string message) : base(message) { }
    }

    // Lớp trừu tượng cho sản phẩm
    abstract class Product
    {
        public double Cost { get; set; }
        public double Value { get; set; }
        public int Start { get; set; } // Ngày bắt đầu gieo trồng
        public int Duration { get; set; } // Thời gian chờ thu hoạch
        public double FertilizerCost { get; set; }
        public double WaterCost { get; set; }
        public int NumFertilizer { get; set; }
        public int NumWater { get; set; }

        // Phương thức trừu tượng gieo trồng và thu hoạch
        public abstract void Seed(int currentDay);
        public abstract bool Harvest(int currentDay);

        // Tính lợi nhuận
        public double Profit()
        {
            double totalCost = (FertilizerCost * NumFertilizer) + (WaterCost * NumWater);
            return Value - totalCost;
        }
    }

    // Lớp lúa mì kế thừa từ Product
    class Wheat : Product
    {
        public Wheat()
        {
            Cost = 10;
            Value = 30;
            Duration = 3; // Thời gian chờ thu hoạch là 3 ngày
            FertilizerCost = 5;
            WaterCost = 3;
            NumFertilizer = 2;
            NumWater = 3;
        }

        public override void Seed(int currentDay)
        {
            Start = currentDay;
            Console.WriteLine($"Gieo trồng lúa mì vào ngày {currentDay}.");
        }

        public override bool Harvest(int currentDay)
        {
            if (currentDay >= Start + Duration)
            {
                Console.WriteLine($"Thu hoạch lúa mì vào ngày {currentDay}.");
                return true; // Có thể thu hoạch
            }
            else
            {
                Console.WriteLine($"Chưa đến thời gian thu hoạch lúa mì. Cần chờ thêm {Start + Duration - currentDay} ngày.");
                return false; // Chưa thể thu hoạch
            }
        }
    }

    // Lớp cà chua kế thừa từ Product
    class Tomato : Product
    {
        public Tomato()
        {
            Cost = 15;
            Value = 30;
            Duration = 5; // Thời gian chờ thu hoạch là 5 ngày
            FertilizerCost = 2;
            WaterCost = 1;
            NumFertilizer = 2;
            NumWater = 1;
        }

        public override void Seed(int currentDay)
        {
            Start = currentDay;
            Console.WriteLine($"Gieo trồng cà chua vào ngày {currentDay}.");
        }

        public override bool Harvest(int currentDay)
        {
            if (currentDay >= Start + Duration)
            {
                Console.WriteLine($"Thu hoạch cà chua vào ngày {currentDay}.");
                return true; // Có thể thu hoạch
            }
            else
            {
                Console.WriteLine($"Chưa đến thời gian thu hoạch cà chua. Cần chờ thêm {Start + Duration - currentDay} ngày.");
                return false; // Chưa thể thu hoạch
            }
        }
    }

    // Lớp hoa hướng dương kế thừa từ Product
    class Sunflower : Product
    {
        public Sunflower()
        {
            Cost = 20;
            Value = 40;
            Duration = 7; // Thời gian chờ thu hoạch là 7 ngày
            FertilizerCost = 7;
            WaterCost = 5;
            NumFertilizer = 4;
            NumWater = 5;
        }

        public override void Seed(int currentDay)
        {
            Start = currentDay;
            Console.WriteLine($"Gieo trồng hoa hướng dương vào ngày {currentDay}.");
        }

        public override bool Harvest(int currentDay)
        {
            if (currentDay >= Start + Duration)
            {
                Console.WriteLine($"Thu hoạch hoa hướng dương vào ngày {currentDay}.");
                return true; // Có thể thu hoạch
            }
            else
            {
                Console.WriteLine($"Chưa đến thời gian thu hoạch hoa hướng dương. Cần chờ thêm {Start + Duration - currentDay} ngày.");
                return false; // Chưa thể thu hoạch
            }
        }
    }

    // Lớp người chơi
    class Player
    {
        public string UserName { get; set; }
        public double Reward { get; set; }
        public List<Product> Inventory { get; set; }

        public Player(string userName, double reward)
        {
            UserName = userName;
            Reward = reward;
            Inventory = new List<Product>();
        }

        // Mua sản phẩm
        public void BuyProduct(Product product)
        {
            if (Reward >= product.Cost)
            {
                Reward -= product.Cost;
                Inventory.Add(product);
                Console.WriteLine($"\nĐã mua {product.GetType().Name}. \n--> Điểm thưởng còn lại: {Reward}.");
            }
            else
            {
                throw new NotEnoughRewardException($"\nKhông đủ điểm thưởng để mua {product.GetType().Name}.\n--> Điểm thưởng hiện tại: {Reward}.");
            }
        }

        // Gieo trồng và thu hoạch sản phẩm
        public void SeedAndHarvestProduct(Product product, int currentDay)
        {
            if (product.Harvest(currentDay))
            {
                double profit = product.Profit();
                Reward += profit;
                Console.WriteLine($"Lợi nhuận thu được từ {product.GetType().Name}: {profit}.\n--> Điểm thưởng sau khi thu hoạch: {Reward}");
            }
        }

        // Hiển thị danh sách sản phẩm đã mua
        public void ShowInventory()
        {
            Console.WriteLine("\nDanh sách sản phẩm đã mua:");
            foreach (Product product in Inventory)
            {
                Console.WriteLine($"- {product.GetType().Name} (Giá trị: {product.Value})");
            }
        }
    }

    // Chương trình chính
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Tạo người chơi với 100 điểm thưởng
            Player player = new Player("Sheri", 50);

            // Tạo sản phẩm
            Product wheat = new Wheat();
            Product tomato = new Tomato();
            Product sunflower = new Sunflower();

            int currentDay = 1;

            Console.WriteLine($"Điểm thưởng ban đầu của {player.UserName}: {player.Reward}\n");

            try
            {
                // Mua lúa mì và gieo trồng
                player.BuyProduct(wheat);
                wheat.Seed(currentDay);

                // Mua cà chua và gieo trồng
                player.BuyProduct(tomato);
                tomato.Seed(currentDay);

                // Tăng ngày (sau 2 ngày)
                currentDay += 5;
                Console.WriteLine($"\nNgày hiện tại: {currentDay}");

                // Cố gắng thu hoạch lúa mì (chưa đến thời gian)
                if (wheat.Harvest(currentDay))
                {
                    player.SeedAndHarvestProduct(wheat, currentDay);
                }

                //// Tăng thêm 2 ngày nữa
                currentDay += 2;
                Console.WriteLine($"\nNgày hiện tại: {currentDay}");

                // Thu hoạch lúa mì (đã đủ thời gian)
                if (wheat.Harvest(currentDay))
                {
                    player.SeedAndHarvestProduct(wheat, currentDay);
                }

                // Cố gắng thu hoạch cà chua (cũng đã đến thời gian)
                if (tomato.Harvest(currentDay))
                {
                    player.SeedAndHarvestProduct(tomato, currentDay);
                }

                //// Thử mua hoa hướng dương
                player.BuyProduct(sunflower);
                sunflower.Seed(currentDay);
            }
            catch (NotEnoughRewardException ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Hiển thị danh sách sản phẩm đã mua
            player.ShowInventory();
            Console.ReadKey();
        }
    }
}