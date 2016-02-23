using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Collections
{
  public class Coins
  {
    private int _id;
    private string _description;
    private int _categoryId;
    private DateTime _dateTime;

    public Coins(string Description, int CategoryId, DateTime DateTimes, int Id =0)
    {
      _id = Id;
      _description = Description;
      _categoryId = CategoryId;
      _dateTime = DateTimes;
    }

    public override bool Equals(System.Object otherCoins)
    {
      if (!(otherCoins is Coins)) {
        return false;
      }
      else
      {
        Coins newCoins = (Coins) otherCoins;
        bool idEquality = this.GetId() == newCoins.GetId();
        bool descriptionEquality = this.GetDescription() == newCoins.GetDescription();
        bool categoryEquality = this.GetCategoryId() == newCoins.GetCategoryId();

        bool dateEquality = this.GetDate() == newCoins.GetDate();
        return (idEquality && descriptionEquality && categoryEquality && dateEquality);
        //return (idEquality && descriptionEquality && categoryEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }

    public int GetCategoryId()
    {
      return _categoryId;
    }

    public void SetCategoryId(int newCategoryId)
    {
      _categoryId = newCategoryId;
    }

    public DateTime GetDate()
    {
      return _dateTime;
    }

    public void SetDate(DateTime newDateTimes)
    {
      _dateTime = newDateTimes;
    }

    public static List<Coins> GetAll()
    {
      List<Coins> allCoins = new List<Coins>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("Select * FROM  coins;",conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int coinId = rdr.GetInt32(0);
        string coinDescription = rdr.GetString(1);
        int coinsCategoryId = rdr.GetInt32(2);
        DateTime coinsDate = rdr.GetDateTime(3);
        Coins newCoins = new Coins(coinDescription, coinsCategoryId, coinsDate,coinId);
        allCoins.Add(newCoins);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allCoins;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("Insert INTO Coins (description, category_id, time) OUTPUT INSERTED.id VALUES (@CoinsDescription, @CoinsCategoryId, @CoinsDate);",conn);

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@CoinsDescription";
      descriptionParameter.Value = this.GetDescription();

      SqlParameter categoryIdParameter = new SqlParameter();
      categoryIdParameter.ParameterName = "@CoinsCategoryId";
      categoryIdParameter.Value = this.GetCategoryId();

      SqlParameter dateParameter = new SqlParameter();
      dateParameter.ParameterName = "@CoinsDate";
      dateParameter.Value = this.GetDate();

      cmd.Parameters.Add(categoryIdParameter);
      cmd.Parameters.Add(descriptionParameter);
      cmd.Parameters.Add(dateParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM Coins;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Coins Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM coins WHERE id= @CoinsId;", conn);
      SqlParameter CoinsIdParameter = new SqlParameter();
      CoinsIdParameter.ParameterName = "@CoinsId";
      CoinsIdParameter.Value = id.ToString();
      cmd.Parameters.Add(CoinsIdParameter);
      rdr = cmd.ExecuteReader();

      int foundCoinsId = 0;
      string foundCoinsDescription = null;
      int foundCoinsCategoryId = 0;
      DateTime foundCoinsDate = new DateTime (2016-02-23);

      while(rdr.Read())
      {
        foundCoinsId = rdr.GetInt32(0);
        foundCoinsDescription = rdr.GetString(1);
        foundCoinsCategoryId = rdr.GetInt32(2);
        foundCoinsDate = rdr.GetDateTime(3);
      }
      Coins foundCoins = new Coins(foundCoinsDescription,foundCoinsCategoryId, foundCoinsDate, foundCoinsId);

      if(rdr != null)
      {
        rdr.Close();
      }

      if(conn != null)
      {
        conn.Close();
      }

      return foundCoins;
    }
  }
}
