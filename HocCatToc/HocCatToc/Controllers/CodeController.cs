using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HocCatToc.Models;

namespace HocCatToc.Controllers
{
    public class CodeController : Controller
    {
        private hoccattocEntities db = new hoccattocEntities();

        // GET: Code
        public ActionResult Index()
        {
            if (Config.getCookie("is_admin") != "1") return RedirectToAction("Login", "Admin", new { message = "Bạn không được cấp quyền truy cập chức năng này" });
            return View(db.frees.ToList());
        }

        // GET: Code/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            free free = db.frees.Find(id);
            if (free == null)
            {
                return HttpNotFound();
            }
            return View(free);
        }

        // GET: Code/Create
        public ActionResult Create()
        {
            if (Config.getCookie("is_admin") != "1") return RedirectToAction("Login", "Admin", new { message = "Bạn không được cấp quyền truy cập chức năng này" });
            return View();
        }

        // POST: Code/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,code")] free free)
        {
            if (ModelState.IsValid)
            {
                db.frees.Add(free);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(free);
        }

        // GET: Code/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Config.getCookie("is_admin") != "1") return RedirectToAction("Login", "Admin", new { message = "Bạn không được cấp quyền truy cập chức năng này" });
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            free free = db.frees.Find(id);
            if (free == null)
            {
                return HttpNotFound();
            }
            return View(free);
        }

        // POST: Code/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,code")] free free)
        {
            if (ModelState.IsValid)
            {
                db.Entry(free).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(free);
        }

        // GET: Code/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Config.getCookie("is_admin") != "1") return RedirectToAction("Login", "Admin", new { message = "Bạn không được cấp quyền truy cập chức năng này" });
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            free free = db.frees.Find(id);
            if (free == null)
            {
                return HttpNotFound();
            }
            return View(free);
        }

        // POST: Code/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            free free = db.frees.Find(id);
            db.frees.Remove(free);
            db.SaveChanges();
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
