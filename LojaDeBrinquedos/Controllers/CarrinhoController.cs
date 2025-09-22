using Microsoft.AspNetCore.Mvc;
using LojaDeBrinquedos.Repositorio;

namespace LojaDeBrinquedos.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly CarrinhoRepositorio _carrinhoRepositorio;
        private readonly ProdutoRepositorio _produtoRepositorio;

        public CarrinhoController(CarrinhoRepositorio carrinhoRepositorio, ProdutoRepositorio produtoRepositorio)
        {
            _carrinhoRepositorio = carrinhoRepositorio;
            _produtoRepositorio = produtoRepositorio;
        }
        public async Task<IActionResult> Index()
        {
            var cartItems = _carrinhoRepositorio.CarrinhoItems(HttpContext.Session);
            // Iterar sobre os itens do carrinho e buscar os detalhes do produto
            foreach (var item in cartItems)
            {
                // Certifique-se de que _productRepository está retornando um Product ou null
                item.Produto = await _produtoRepositorio.ProdutosPorId(item.ProdutoId);

                // Opcional: Lógica para lidar com produtos que não foram encontrados (removidos do DB, etc.)
                if (item.Produto == null)
                {
                    // Poderia remover o item do carrinho ou marcá-lo como indisponível
                    // Exemplo: item.Product = new Product { Name = "Produto Indisponível", Price = 0, ImageUrl = "/images/default_unavailable.jpg" };
                }
            }
            ViewBag.TotalCarrinho = _carrinhoRepositorio.TotalCarrinho(HttpContext.Session);
            return View(cartItems);
        }


        [HttpPost]
        public async Task<IActionResult> AdicionarCarrinho(int produtoId, int quantidade = 1)
        {
            var produto = await _produtoRepositorio.ProdutosPorId(produtoId);
            if (produto == null)
            {
                TempData["Message"] = "Produto não encontrado."; // Use TempData para mensagens
                return RedirectToAction("Index", "Home");
            }

            _carrinhoRepositorio.AdicionarCarrinho(HttpContext.Session, produto, quantidade);
            return RedirectToAction("Index", "Carrinho");
        }

        [HttpPost]
        public IActionResult AlterarQuantidadeItem(int produtoId, int novaQuantidade)
        {
            _carrinhoRepositorio.AlterarQuantidadeItem(HttpContext.Session, produtoId, novaQuantidade);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int produtoId)
        {
            _carrinhoRepositorio.RemoverItemCarrinho(HttpContext.Session, produtoId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult LimparCarrinho()
        {
            _carrinhoRepositorio.LimparCarrinho(HttpContext.Session);
            return RedirectToAction("Index");
        }
    }
}
