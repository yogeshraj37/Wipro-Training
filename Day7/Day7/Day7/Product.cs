namespace Day7
{
    class Product
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        public Product(string name, int price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public void Display()
        {
            Console.WriteLine("Name: " + Name + " Price: " + Price + " Quantity: " + Quantity);
        }
    }

    class ProductList
    {
        List<Product> Temp = new List<Product>();

        public void Add(Product product)
        {
            Temp.Add(product);
        }

        public Product this[int index]
        {
            get
            {
                if (index >= 0 && index < Temp.Count)
                    return Temp[index];
                else
                    throw new Exception("index out of bound");
            }
            set
            {
                if (index >= 0 && index < Temp.Count)
                    Temp[index] = value;
                else
                    throw new Exception("index out of bound");
            }
        }

        public void DisplayAll()
        {
            foreach (Product product in Temp)
            {
                product.Display();
            }
        }

         
        public static void solve()
        {
            ProductList obj = new ProductList();

            obj.Add(new Product("Oil", 10, 10));
            obj.Add(new Product("Shoes", 200, 11));

            Console.WriteLine("Products");
            obj.DisplayAll();

            Console.WriteLine("Accessing with index");
            Product p = obj[1];
            p.Display();

            Console.WriteLine("Updating with index");
            obj[1] = new Product("Shirt", 1, 1);

            obj.DisplayAll();
        }
    }
}