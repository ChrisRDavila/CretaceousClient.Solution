using Microsoft.AspNetCore.Mvc;
using CretaceousClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using System.Diagnostics;

namespace CretaceousClient.Controllers;

public class AnimalsController : Controller
{
  public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
  {
    Animal animal = new Animal();
    List<Animal> animalList = new List<Animal> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"https://localhost:5000/api/Animals?page={page}&pagesize={pageSize}"))
      {
        var animalContent = await response.Content.ReadAsStringAsync();
        JArray animalArray = JArray.Parse(animalContent);
        animalList = animalArray.ToObject<List<Animal>>();
      }
    }

    ViewBag.TotalPages = animalList.Count();
    //page number inside the url
    ViewBag.CurrentPage = page;
    //amnt of items on the page
    ViewBag.PageSize = pageSize;
     //the amount of destinations returned from our database
    // ViewBag.Pages = pageCount;

    return View(animalList);
  }

  public IActionResult Details(int id)
  {
    Animal animal = Animal.GetDetails(id);
    return View(animal);
  }

  public ActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public ActionResult Create(Animal animal)
  {
    Animal.Post(animal);
    return RedirectToAction("Index");
  }

  public ActionResult Edit(int id)
  {
    Animal animal = Animal.GetDetails(id);
    return View(animal);
  }

  [HttpPost]
  public ActionResult Edit(Animal animal)
  {
    Animal.Put(animal);
    return RedirectToAction("Details", new { id = animal.AnimalId});
  }

  public ActionResult Delete(int id)
  {
    Animal animal = Animal.GetDetails(id);
    return View(animal);
  }

  [HttpPost, ActionName("Delete")]
  public ActionResult DeleteConfirmed(int id)
  {
    Animal.Delete(id);
    return RedirectToAction("Index");
  }

}