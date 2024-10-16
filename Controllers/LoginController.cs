using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sistema.Contexts;

namespace sistema.Controllers
{
    public class LoginController : Controller
    {
        private readonly sistemaContext _context;
        public LoginController(sistemaContext context)
        {
           _context =  context;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logar(string email, string senha)
        {
         var professorBuscado = _context.Professors.FirstOrDefault(p => p.Email == email && p.Senha == senha);

            if (professorBuscado != null)
            {
                HttpContext.Session.SetInt32("ProfessorId", professorBuscado.ProfessorId);

                HttpContext.Session.SetString("Nome", professorBuscado.Nome!);

                return RedirectToAction("Index", "Professor");
            }

            TempData["Mensagem"] = "Email ou senha incorretos";

            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult Logout() { 
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Login");
        }
    }
}
