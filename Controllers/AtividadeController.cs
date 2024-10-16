using Microsoft.AspNetCore.Mvc;
using sistema.Contexts;
using sistema.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http; // Para manipular a sessão

namespace sistema.Controllers
{
    public class AtividadeController : Controller
    {
        private readonly sistemaContext _context;

        // Construtor correto
        public AtividadeController(sistemaContext context)
        {
            _context = context;
        }

        // Método Index
        public IActionResult Index(int turmaId)
        {
            int? professorId = HttpContext.Session.GetInt32("ProfessorId");

            if (professorId == null)
            {
                return RedirectToAction("Login"); // Se o professor não está logado, redireciona para o login
            }

            List<Atividade> atividades = _context.Atividades.Where(atv => atv.TurmaId == turmaId).ToList();

            // Passando o turmaId para a ViewBag
            ViewBag.TurmaId = turmaId;

            if (atividades.Any())
            {
                return View(atividades);
            }

            return RedirectToAction("Index", "Professor"); // Se não houver atividades, redireciona para o controller do professor
        }

        // Método para cadastrar atividade
        [HttpPost]
        public IActionResult CadastrarAtividade(string nomeAtividade, int turmaId)
        {
            var turma = _context.Turmas.FirstOrDefault(t => t.TurmaId == turmaId);

            if (turma == null)
            {
                ModelState.AddModelError(string.Empty, "Turma não encontrada.");
                return View();
            }

            var newAtividade = new Atividade
            {
                TurmaId = turmaId,
                Descricao = nomeAtividade
            };

            _context.Atividades.Add(newAtividade);
            _context.SaveChanges();

            return RedirectToAction("Index", new { turmaId });
        }
    }
}
