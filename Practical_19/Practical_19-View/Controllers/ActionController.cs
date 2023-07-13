using DataAccess.Models;
using DataAccess.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Practical_19.Data;
using System.Net;

namespace Practical_19_View.Controllers
{
    public class ActionController : Controller
    {
        private readonly HttpClient _httpClient;
        public ActionController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("api");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Users users)
        {
            if (ModelState.IsValid)
            {
                var result = await _httpClient.PostAsJsonAsync("Action/Register", users);
                var msg = result.Content.ReadAsStringAsync();
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("Login");

                }
                ModelState.AddModelError("", msg.Result);
                return View(users);
            }
            return View(users);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7088/");
                HttpResponseMessage result = await client.PostAsJsonAsync<LoginViewModel>("api/Action", model);
                //var result = await _httpClient.PostAsJsonAsync("Action/Login", model);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    UserManagerResponse userManager = JsonConvert.DeserializeObject<UserManagerResponse>(content);

                    Response.Cookies.Append("userToken", userManager.Message);
                    Response.Cookies.Append("Email", userManager.Email);
                    return RedirectToAction("Index", "Student");

                }
                ModelState.AddModelError("", "Data can't added Try again!");
                return View(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            LogoutViewModel logout = new LogoutViewModel();
            string data = Request.Cookies["Email"].ToString();
            Response.Cookies.Delete("Email");
            Response.Cookies.Delete("userToken");

            logout.Email = data;
            var result = await _httpClient.PostAsJsonAsync("Action/Logout", logout);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
