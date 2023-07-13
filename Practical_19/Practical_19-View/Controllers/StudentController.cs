using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace Practical_19_View.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly HttpClient _httpClient;

        public StudentController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient.CreateClient("api/");

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["Email"] == null)
            {
                return RedirectToAction("Login", "Action");
            }
            var token = Request.Cookies["userToken"].ToString();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("Action/Student");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var data = await response.Content.ReadAsStringAsync();
                List<Students> users = JsonConvert.DeserializeObject<List<Students>>(data);
                return View(users);
            }
            return View(new List<Students>());
            //IEnumerable<Students>? students = await _httpClient.GetFromJsonAsync<IEnumerable<Students>>("Students");
            //return View(students);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();  
        }
        [HttpPost]
        public async Task<IActionResult> Create(Students student)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7088/");
                var response = await client.PostAsJsonAsync<Students>("api/Students", student);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private static async Task<Students> GetStudentById(int id)
        {
            Students student = new Students();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7088/");
            HttpResponseMessage response = await client.GetAsync($"api/Students/{id}");
            if (response.IsSuccessStatusCode)
            {
                var res = response.Content.ReadAsStringAsync().Result;
                student = JsonConvert.DeserializeObject<Students>(res);
            }

            return student;
        }
        public async Task<IActionResult> Detail(int id)
        {
            Students student = await GetStudentById(id);
            return View(student);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Students student = new Students();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7054/");
            HttpResponseMessage response = await client.DeleteAsync($"api/Students/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Students student = await GetStudentById(id);
            return View(student);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Students student)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7088/");
            var response = await client.PutAsJsonAsync<Students>($"api/Students/{student.Id}", student);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
