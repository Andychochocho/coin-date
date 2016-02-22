using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Collections
{
  public class Coins
  {
    private int _id;
    private string _description;

    public Coins(string Description, int Id =0)
    {
      _id = Id;
      _description = Description;
    }

    public override bool Equals(System.Object otherCoins)
    {
      if (!(otherCoins is Coins)) {
        return false;
      }
      else
      {
        Coins newCoins = (Coins) otherCoins;
        bool idEquality = (this.GetId() == newCoins.GetId());
        bool descriptionEquality = (this.GetDescription() == newCoins.GetDescription());
        return (idEquality && descriptionEquality);
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
        Coins newCoins = new Coins(coinDescription, coinId);
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

      SqlCommand cmd = new SqlCommand("Insert INTO Coins (description) OUTPUT INSERTED.id VALUES (@CoinsDescription);",conn);

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@CoinsDescription";
      descriptionParameter.Value = this.GetDescription();
      cmd.Parameters.Add(descriptionParameter);
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

      int foundCoinsId =0;
      string foundCoinsDescription = null;
      while(rdr.Read())
      {
        foundCoinsId = rdr.GetInt32(0);
        foundCoinsDescription = rdr.GetString(1);
      }
      Coins foundCoins = new Coins(foundCoinsDescription, foundCoinsId);

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
