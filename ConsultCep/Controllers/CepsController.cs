using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConsultCep.Models;
using System.Net.Http;

namespace ConsultCep.Controllers
{
    public class CepsController : Controller
    {
        private ConsultCepContext db = new ConsultCepContext();

        // GET: Ceps
        public async Task<ActionResult> Index()
        {
            return View(await db.Ceps.ToListAsync());
        }

        // GET: Ceps/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ceps ceps = await db.Ceps.FindAsync(id);
            if (ceps == null)
            {
                return HttpNotFound();
            }
            return View(ceps);
        }

        // GET: Ceps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ceps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Cep")] Ceps ceps)
        {
            /*if (ModelState.IsValid)
            {*/


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://viacep.com.br/ws/" + ceps.Cep + "/json/");

                //HTTP GET
                var responseTask = client.GetAsync("");
                responseTask.Wait();
                var result = responseTask.Result;
                Ceps res = new Ceps();

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Ceps>();
                    readTask.Wait();
                    res = readTask.Result;

                    db.Ceps.Add(res);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    //contatos = Enumerable.Empty<ContatoViewModel>();
                    ModelState.AddModelError(string.Empty, "Erro no servidor. Contate o Administrador.");
                }
                return View(ceps);
            }


            
           /* }
            */
        }

        // GET: Ceps/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ceps ceps = await db.Ceps.FindAsync(id);
            if (ceps == null)
            {
                return HttpNotFound();
            }
            return View(ceps);
        }

        // POST: Ceps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Cep,Logradouro,Complemento,Bairro,Localidade,Uf,Ibge,Gia,Ddd,Siafi")] Ceps ceps)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ceps).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ceps);
        }

        // GET: Ceps/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ceps ceps = await db.Ceps.FindAsync(id);
            if (ceps == null)
            {
                return HttpNotFound();
            }
            return View(ceps);
        }

        // POST: Ceps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Ceps ceps = await db.Ceps.FindAsync(id);
            db.Ceps.Remove(ceps);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
