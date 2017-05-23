using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LmrBot;

namespace LmrBot.Controllers
{
    public class ResponsController : Controller
    {
        private lmr db = new lmr();

        // GET: Respons
        public async Task<ActionResult> Index()
        {
            var responses = db.Responses.Include(r => r.Intent);
            return View(await responses.ToListAsync());
        }

        // GET: Respons/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respons respons = await db.Responses.FindAsync(id);
            if (respons == null)
            {
                return HttpNotFound();
            }
            return View(respons);
        }

        // GET: Respons/Create
        public ActionResult Create()
        {
            ViewBag.intentId = new SelectList(db.Intents, "id", "content");
            return View();
        }

        // POST: Respons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,content,intentId")] Respons respons)
        {
            if (ModelState.IsValid)
            {
                db.Responses.Add(respons);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.intentId = new SelectList(db.Intents, "id", "content", respons.intentId);
            return View(respons);
        }

        // GET: Respons/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respons respons = await db.Responses.FindAsync(id);
            if (respons == null)
            {
                return HttpNotFound();
            }
            ViewBag.intentId = new SelectList(db.Intents, "id", "content", respons.intentId);
            return View(respons);
        }

        // POST: Respons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,content,intentId")] Respons respons)
        {
            if (ModelState.IsValid)
            {
                db.Entry(respons).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.intentId = new SelectList(db.Intents, "id", "content", respons.intentId);
            return View(respons);
        }

        // GET: Respons/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respons respons = await db.Responses.FindAsync(id);
            if (respons == null)
            {
                return HttpNotFound();
            }
            return View(respons);
        }

        // POST: Respons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Respons respons = await db.Responses.FindAsync(id);
            db.Responses.Remove(respons);
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
