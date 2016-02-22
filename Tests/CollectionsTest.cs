using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace Collections
{
  public class Collections : IDisposable
  {
    public Collections()
    {
      DBConfiguration.ConnectionString = "Data Source = (localdb)\\mssqllocaldb;Initial Catalog=collections_test; Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Coins.GetAll().Count;
      Assert.Equal(0,result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionAreTheSame()
    {
      Coins firstCoin = new Coins("American Quarters");
      Coins secondCoin = new Coins("American Quarters");

      Assert.Equal(firstCoin, secondCoin);
    }
    [Fact]
    public void Test_Save_SavestoDatabase()
    {
      Coins testCoins = new Coins("American Quarters");
      testCoins.Save();
      List<Coins> result = Coins.GetAll();
      List<Coins> testList = new List<Coins> {testCoins};
      Assert.Equal( testList,result);
    }
    [Fact]
    public void Test_Save_AssignsIdToObjects()
    {
      Coins testCoins = new Coins("American Quarters");
      testCoins.Save();

      Coins savedCoins = Coins.GetAll()[0];

      int result = savedCoins.GetId();
      int testId = testCoins.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsCoinsInDatabase()
    {
      Coins testCoins = new Coins("American Quarters");
      testCoins.Save();

      Coins foundCoins = Coins.Find(testCoins.GetId());
      Assert.Equal(testCoins, foundCoins);
    }

    public void Dispose()
    {
      Coins.DeleteAll();
    }
  }
}
