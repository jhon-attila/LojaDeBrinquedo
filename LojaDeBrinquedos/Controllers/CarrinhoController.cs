using Microsoft.AspNetCore.Mvc;

namespace LojaDeBrinquedos.Controllers
{
    public class CarrinhoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
