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
    public class IntentsController : Controller
    {
        private lmr db = new lmr();

        // GET: Intents
        public async Task<ActionResult> Index()
        {
            return View(await db.Intents.ToListAsync());
        }

        // GET: Intents/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Intent intent = await db.Intents.FindAsync(id);
            if (intent == null)
            {
                return HttpNotFound();
            }
            return View(intent);
        }

        // GET: Intents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Intents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,content")] Intent intent)
        {
            if (ModelState.IsValid)
            {
                db.Intents.Add(intent);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(intent);
        }

        // GET: Intents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Intent intent = await db.Intents.FindAsync(id);
            if (intent == null)
            {
                return HttpNotFound();
            }
            return View(intent);
        }

        // POST: Intents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,content")] Intent intent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(intent).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(intent);
        }

        // GET: Intents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Intent intent = await db.Intents.FindAsync(id);
            if (intent == null)
            {
                return HttpNotFound();
            }
            return View(intent);
        }

        // POST: Intents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Intent intent = await db.Intents.FindAsync(id);
            db.Intents.Remove(intent);
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
