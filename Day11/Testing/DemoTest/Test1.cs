namespace DemoTest;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void TestMethod1()
    {
         var solve = new Solve();
         int result = solve.Add(2,3);
         Assert.AreEqual(5,result);
        //  result = solve.Add(10,12);
        //  Assert.AreEqual(12,result);

    }
}
