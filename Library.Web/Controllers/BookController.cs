using Library.Domain.Entities;
using Library.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Library.Web.Controllers
{
    public class BookController : Controller
    {
        private Uri baseAddress = new Uri("https://localhost:7164");

        private readonly HttpClient _client;

        public BookController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }



        public IActionResult Index()
        {
            List<Book> booklist = new List<Book>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "Books").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                booklist = JsonConvert.DeserializeObject<List<Book>>(data);
            }
            return View(booklist);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookCreateDto book)
        {
            HttpResponseMessage response = _client.PostAsJsonAsync(_client.BaseAddress + "Books", book).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string errorMessage = "An error occurred while creating the book.";
                ViewBag.ErrorMessage = errorMessage;
                return View("Create", book);
            }
        }


        public async Task<IActionResult> DeleteBook(Guid id)
        {
            HttpResponseMessage response = await _client.DeleteAsync(_client.BaseAddress + "Books/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string errorMessage = "An error occurred while deleting the book.";
                ViewBag.ErrorMessage = errorMessage;
                return View("Index");
            }
        }


    }
}
