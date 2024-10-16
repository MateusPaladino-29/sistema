using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema.Contexts;
using sistema.Models;

namespace sistema.Controllers
{
    public class ProfessorController : Controller
    {
        private readonly sistemaContext _context;
        public ProfessorController(sistemaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            int? professorId = HttpContext.Session.GetInt32("ProfessorId");

            if (professorId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var professor = _context.Professors.Find(professorId);

            var turmas = _context.Turmas.Where(t => t.ProfessorId == professorId).ToList();

            ViewBag.NomeProfessor = professor!.Nome;



            return View(turmas);
        }

        [HttpPost]
        public IActionResult CadastrarTurma(string nomeTurma)
        {
           int? professorId = HttpContext.Session.GetInt32("professorId");

            var turma = new Turma
            {
                Nome = nomeTurma,
                ProfessorId = professorId
            };

            _context.Turmas.Add(turma);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ExcluirTurma(int turmaId) 
        {
            var turma = _context.Turmas.Include(t => t.Atividades).FirstOrDefault( t => t.TurmaId == turmaId);

            if (turma.Atividades.Any())
            {
                TempData["Mensagem"] = "Vocë não pode exclui uma turma com atividades";

                return RedirectToAction("Index");
            }

            _context.Turmas.Remove(turma);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
