using makingWaves.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace makingWaves.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static HttpClient client = new HttpClient();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            //create aages in the meny nav when page is first loaded
            var respons = await GetProductAsync("1");
            ViewBag.Nr = respons.Total_pages;
            return View();
        }
        public async Task<IActionResult> ColorPage(int pageId)
        {
            //changes the page content
            var respons = await GetProductAsync(pageId.ToString());
            //Meny page links disapears otherwise
            ViewBag.Nr = respons.Total_pages;
            //ColorGroup splits the PantoneValue string to int and sorts the Color list
            List<Color> SortedList = ColorsGroup(respons);           
            return View(SortedList);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private List<Color> ColorsGroup(ColorModel resp)
        {
            List<Color> Group = new List<Color>();
            List<Color> GroupOne = new List<Color>();

            foreach (var color in resp.Data)
            {
               //split the string ex 17-1234 into and array and convert to an int = 17
               string[] pantone = color.Pantone_value.ToString().Split("-");
               int pantoneInt = int.Parse(pantone[0]);
                //If pantoneInt does not exist in GroupOne List and is divisible by 3 then save in groupOne
                if ( GroupOne.Exists(x => x.PantoneValue == pantoneInt) == false && pantoneInt % 3 == 0 )
                {
                    GroupOne.Add(new Color(color.Id, 1, color.Year, color.Color, pantoneInt));
                }
                else if(pantoneInt % 2 == 0 )
                {
                    //Save in Group and give it value of group: 2
                    Group.Add(new Color(color.Id, 2, color.Year, color.Color, pantoneInt));
                }
                else
                {
                    //if it is not divisible by 3 and 2 give then it group  3
                    Group.Add(new Color(color.Id, 3, color.Year, color.Color, pantoneInt));
                }
                   
            }
            //concant groupOne and group list. Sort by group 123 and year ASC
            var FinishedGroup = GroupOne.Concat(Group).ToList();
            var SortedGroup = FinishedGroup.OrderBy(x => x.Group ).ThenBy(e => e.Year).ToList();

            return SortedGroup;
        }
        static async Task<ColorModel> GetProductAsync(string path)
        {
            ColorModel model = null;

            try
            {
                //make a Call to https://reqres.in/api/example?per_page=2&page=1 and put the json response into ColorModel, Data and Support classes in models
                HttpResponseMessage response = await client.GetAsync("https://reqres.in/api/example?per_page=2&page=" + path);

                //Disclaimer** because  the assignment is to Use ASP.NET MVC and no JS is used I choose to make direct calls with
                //HttpClient(). It is not good practice to make many  frequent (for every page) calls with HttpClient() as I do here. In production I would use fetch or ajax or load everything once and put in session or something else.
                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<ColorModel>();

                }
            }
            catch (Exception e)
            {
                //if there is no response then thow and error
                Console.WriteLine(e.Message);
            }
            return model;
        }
    }
}
