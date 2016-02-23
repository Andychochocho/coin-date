using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace Collections
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["index.cshtml", AllCategories];
      };

      Get["/coins"] = _ => {
        List<Coins> AllCoins = Coins.GetAll();
        return View["coins.cshtml", AllCoins];
      };
      Get["/categories"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["categories.cshtml", AllCategories];
      };

      Get["/categories/new"] = _ => {
        return View["categories_form.cshtml"];
      };
      Post["/categories/new"] = _ => {
        Category newCategory = new Category(Request.Form["category-name"]);
        newCategory.Save();
        return View["success.cshtml"];
      };
      Get["/coins/new"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["coins_form.cshtml", AllCategories];
      };
      Post["/coins/new"] = _ => {
        Coins newCoins = new Coins(Request.Form["coins-description"], Request.Form["category-id"], Request.Form["date"]);
        newCoins.Save();
        return View["success.cshtml"];
      };
      Post["/coins/delete"] = _ => {
        Coins.DeleteAll();
        return View["cleared.cshtml"];
      };
      Get["/categories/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var SelectedCategory = Category.Find(parameters.id);
        var CategoryCoins = SelectedCategory.GetCoins();
        model.Add("category", SelectedCategory);
        model.Add("coins", CategoryCoins);
        return View["category.cshtml", model];
      };
    }
  }
}
