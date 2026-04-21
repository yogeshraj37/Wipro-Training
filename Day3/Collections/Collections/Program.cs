 

class Customer
{
    public string Name;
    public int Id;

    public Customer(string name, int id)
    {
        Name = name;
        Id = id;
    }
}

class Order
{
    public int OrderId;
    public int CustomerId;
    public string ProductCategory;
    public Stack<string> StatusHistory;

    public Order(int orderId, int customerId,string productCategory)
    {
        OrderId = orderId;
        CustomerId = customerId;
        ProductCategory = productCategory;
        StatusHistory = new Stack<string>();
    }

}

class OrderSystem
{
      public List<Order> orders = new List<Order>();
      public Dictionary<int, Customer> customer= new Dictionary<int, Customer>();
    public HashSet<string> catcategories  = new HashSet<string>();
      public Queue<Order> orderQueue = new Queue<Order>();


}

