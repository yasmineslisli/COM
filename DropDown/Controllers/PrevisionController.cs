using DropDown.Data;
using System;
using System.Collections.Generic;
using DropDown.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
namespace DropDown.Controllers;

using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using Remotion.FunctionalProgramming;
using System.Data;
using System.Data.OleDb;
using System.Data;
using System.Drawing.Printing;
using System.Linq;



public class PrevisionController : Controller
{
    private readonly DropDownContext context;
    private readonly IConfiguration configuration;
    public PrevisionController(DropDownContext context, IConfiguration configuration)
    {
        this.context = context;
        this.configuration = configuration;
    }

    [HttpGet]
    public IActionResult Prevision()
    {
        ViewBag.Date = DateTime.Now;
        var id = TempData["ObjectifId"];
        ViewBag.ObjectifId = id;
        var req = context.Objectifs
            
                            .Include(x => x.Exercice)
                            .Include(x => x.Dr)
                            .Include(x => x.ActionProj)
                            .Include(x => x.ActionProj.Projet)
                            .Include(x => x.ActionProj.Projet.Programme)
                            .Where(m => m.Id.Equals(id));
        ViewBag.Programme = req.First().ActionProj.Projet.Programme.Name;
        ViewBag.Projet = req.First().ActionProj.Projet.Name;
        ViewBag.ActionProj = req.First().ActionProj.Name;
        ViewBag.Exercice = req.First().Exercice.Annee;
        ViewBag.Dr = req.First().Dr.Name;


        var st=context.Stocks.Where(x => x.ObjectifId.Equals(id));
        ViewBag.StockSup=st.First().Superficie;
        ViewBag.StockNombre = st.First().Nombre;
        ViewBag.StockValeur = st.First().Valeur;

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Prevision(int? ObjectifId, [Bind("ObjectifId,Nombre,Superficie,Name,Valeur,Date")] Prévision prévision)
    {
        if (ModelState.IsValid)
        {
            context.Add(prévision);
            await context.SaveChangesAsync();
            TempData["PrevisionId"] = prévision.Id;
            return RedirectToAction(nameof(ListPrevision));
        }

        

        if (ObjectifId == null || context.Prévisions == null)
        {
            return NotFound();
        }
        var act = await context.Prévisions
                                    .Include(x => x.Objectif)
                                    .Include(x => x.Objectif.ActionProj)
                                    .Include(x => x.Objectif.ActionProj.Projet)
                                    .Include(x => x.Objectif.ActionProj.Projet.Programme)
                                    .FirstOrDefaultAsync(m => m.Id == ObjectifId);

        if (act == null)
        {
            return NotFound();
        }

        return View(act);
    }

    public async Task<IActionResult> ListPrevision(string sortOrder, string currentFilter,
        string? SearchString, int? pageNumber)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["ProgrammeSortParm"] = sortOrder == "Programme" ? "Programme_desc" : "Programme";
        ViewData["ExerciceSortParm"] = sortOrder == "Exercice" ? "Exercice_desc" : "Exercice";
        ViewData["ProjetSortParm"] = sortOrder == "Projet" ? "Projet_desc" : "Projet";
        ViewData["ActionSortParm"] = sortOrder == "Action" ? "Action_desc" : "Action";
        ViewData["DrSortParm"] = sortOrder == "Dr" ? "Dr_desc" : "Dr";
        ViewData["DateSortParm"] = sortOrder == "Date" ? "Date_desc" : "Date";


        if (HttpContext.Session.GetString("Profil") == "DR")
        {
            IQueryable<Prévision> prev = context.Prévisions
                                                .Include(x => x.Objectif.Dr)
                                                .Include(x => x.Objectif.Exercice)
                                                .Include(x => x.Objectif.ActionProj)
                                                .Include(x => x.Objectif.ActionProj.Projet)
                                                .Include(x => x.Objectif.ActionProj.Projet.Programme)
                                                .Where(x => x.Objectif.Drid == HttpContext.Session.GetInt32("StructureId"))
                                                ;

            if (!String.IsNullOrEmpty(SearchString))
            {
                prev = prev
                    .Where(s => s.Objectif.ActionProj.Name.Contains(SearchString)
                                       || s.Objectif.ActionProj.Projet.Name.Contains(SearchString)
                                       || s.Objectif.ActionProj.Projet.Programme.Name.Contains(SearchString)
                                       || s.Objectif.Dr.Name.Contains(SearchString)
                                       || s.Objectif.Exercice.Annee.Contains(SearchString));
            }
            switch (sortOrder)
            {
                case "Programme_desc": //Programme
                    prev = prev.OrderByDescending(s => s.Objectif.ActionProj.Projet.Programme.Name);
                    break;
                case "Projet"://Projet
                    prev = prev.OrderBy(s => s.Objectif.ActionProj.Projet.Name);
                    break;
                case "Projet_desc"://Projet
                    prev = prev.OrderByDescending(s => s.Objectif.ActionProj.Projet.Name);
                    break;
                case "Action"://Action
                    prev = prev.OrderBy(s => s.Objectif.ActionProj.Name);
                    break;
                case "Action_desc"://Action
                    prev = prev.OrderByDescending(s => s.Objectif.ActionProj.Name);
                    break;
                case "Exercice": //Exercice
                    prev = prev.OrderBy(s => s.Objectif.Exercice.Annee);
                    break;
                case "Exercice_desc": //Exercice
                    prev = prev.OrderByDescending(s => s.Objectif.Exercice.Annee);
                    break;
                case "Dr": //Dr
                    prev = prev.OrderBy(s => s.Objectif.Dr.Name);
                    break;
                case "Dr_desc": //Dr
                    prev = prev.OrderByDescending(s => s.Objectif.Dr.Name);
                    break;
                case "Date": //Dr
                    prev = prev.OrderBy(s => s.Date);
                    break;
                case "Date_desc": //Dr
                    prev = prev.OrderByDescending(s => s.Date);
                    break;
                default:
                    prev = prev.OrderBy(s => s.Objectif.ActionProj.Projet.Programme.Name);
                    break;
            }

            int pageSize = 4;
            return View(await PaginatedList<Prévision>.CreateAsync(prev, pageNumber ?? 1, pageSize));

        }
        else
        {
            IQueryable<Prévision> prev = context.Prévisions
                                                .Include(x => x.Objectif.Dr)
                                                .Include(x => x.Objectif.Exercice)
                                                .Include(x => x.Objectif.ActionProj)
                                                .Include(x => x.Objectif.ActionProj.Projet)
                                                .Include(x => x.Objectif.ActionProj.Projet.Programme)

                                                ;
            if (!String.IsNullOrEmpty(SearchString))
            {
                prev = prev
                    .Where(s => s.Objectif.ActionProj.Name.Contains(SearchString)
                                       || s.Objectif.ActionProj.Projet.Name.Contains(SearchString)
                                       || s.Objectif.ActionProj.Projet.Programme.Name.Contains(SearchString)
                                       || s.Objectif.Dr.Name.Contains(SearchString)
                                       || s.Objectif.Exercice.Annee.Contains(SearchString));
            }
            switch (sortOrder)
            {
                case "Programme_desc": //Programme
                    prev = prev.OrderByDescending(s => s.Objectif.ActionProj.Projet.Programme.Name);
                    break;
                case "Projet"://Projet
                    prev = prev.OrderBy(s => s.Objectif.ActionProj.Projet.Name);
                    break;
                case "Projet_desc"://Projet
                    prev = prev.OrderByDescending(s => s.Objectif.ActionProj.Projet.Name);
                    break;
                case "Action"://Action
                    prev = prev.OrderBy(s => s.Objectif.ActionProj.Name);
                    break;
                case "Action_desc"://Action
                    prev = prev.OrderByDescending(s => s.Objectif.ActionProj.Name);
                    break;
                case "Exercice": //Exercice
                    prev = prev.OrderBy(s => s.Objectif.Exercice.Annee);
                    break;
                case "Exercice_desc": //Exercice
                    prev = prev.OrderByDescending(s => s.Objectif.Exercice.Annee);
                    break;
                case "Dr": //Dr
                    prev = prev.OrderBy(s => s.Objectif.Dr.Name);
                    break;
                case "Dr_desc": //Dr
                    prev = prev.OrderByDescending(s => s.Objectif.Dr.Name);
                    break;
                case "Date": //Dr
                    prev = prev.OrderBy(s => s.Date);
                    break;
                case "Date_desc": //Dr
                    prev = prev.OrderByDescending(s => s.Date);
                    break;
                default:
                    prev = prev.OrderBy(s => s.Objectif.ActionProj.Projet.Programme.Name);
                    break;
            }

            int pageSize = 4;
            return View(await PaginatedList<Prévision>.CreateAsync(prev, pageNumber ?? 1, pageSize));

        }



    }
    
    public async Task<IActionResult> Edit(int? id)
    {
        ViewBag.Date = DateTime.Now;

        ViewBag.Programme = TempData["Programme"];
        ViewBag.Projet = TempData["Projet"];
        ViewBag.Action = TempData["Action"];
        ViewBag.Dr = TempData["Dr"];

        //ViewData["ProfilId"] = new SelectList(context.Profils, "Id", "Name");
        //ViewData["StructureId"] = new SelectList(context.Structures, "Id", "Name");
        if (id == null || context.Prévisions == null)
        {
            return NotFound();
        }

        var prog = await context.Prévisions.FindAsync(id);
        if (prog == null)
        {
            return NotFound();
        }
        return View(prog);
    }

    //// POST: Indicateurs/Edit/5
    //// To protect from overposting attacks, enable the specific properties you want to bind to.
    //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int Id, [Bind("Id,ObjectifId,Nombre,Superficie,Valeur,Date")] Prévision prevision)
    {
        if (Id != prevision.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(prevision);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrévisionsExists(prevision.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(ListPrevision));
        }
        return View(prevision);
    }

    private bool PrévisionsExists(int id)
    {
        return (context.Prévisions?.Any(e => e.Id == id)).GetValueOrDefault();
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || context.Prévisions == null)
        {
            return NotFound();
        }

        var prevision = await context.Prévisions
            .Include(i => i.Objectif)
            .ThenInclude(i => i.ActionProj)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (prevision == null)
        {
            return NotFound();
        }

        return View(prevision);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (context.Prévisions == null)
        {
            return Problem("Entity set 'projetCOMContext.Indicateurs'  is null.");
        }
        var prevision = await context.Prévisions.FindAsync(id);

        if (prevision != null)
        {
            context.Prévisions.Remove(prevision);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(ListPrevision));
    }

    private bool IndicateurExists(int id)
    {
        return (context.Prévisions?.Any(e => e.Id == id)).GetValueOrDefault();
    }


    [HttpGet]
    public async Task<IActionResult> Details(int? Id)
    {
        var id = await context.Prévisions

                               .FirstOrDefaultAsync(i => i.Id == Id);
        ViewData["Ddid"] = new SelectList(context.Dds, "Id", "Name");
        ViewBag.PrévisionId = id;
        
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Details(int? Id, [Bind("NumDossier,Trn,IndiceTrn,NumTrn,Superficie,Valeur,Ddid,PrévisionId")] Detail detail)
    {
        if (ModelState.IsValid)
        {
            detail.IndiceTrn = detail.IndiceTrn.ToUpper();
            context.Add(detail);
            await context.SaveChangesAsync();
            TempData["DetailId"] = detail.Id;
            return RedirectToAction(nameof(ListPrevision));
        }
        if (Id == null || context.Details == null)
        {
            return NotFound();
        }
        var act = await context.Details.FirstOrDefaultAsync(m => m.Id == Id);

        if (act == null)
        {
            return NotFound();
        }

        return View(act);
    }
    public async Task<IActionResult> ListDetails(int? id,string sortOrder, string currentFilter,
        string? SearchString, int? pageNumber)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["ActionSortParm"] = sortOrder == "Action" ? "Action_desc" : "Action";
        ViewData["DdSortParm"] = sortOrder == "Dd" ? "Dd_desc" : "Dd";
        ViewData["NumDossierSortParm"] = sortOrder == "NumDossier" ? "NumDossier_desc" : "NumDossier";
        
            
            IQueryable<Detail> prev = context.Details.Include(i => i.Dd)
                                    .Include(j => j.Prévision)
                                    .Include(l => l.Prévision.Objectif)
                                    .Include(k => k.Prévision.Objectif.ActionProj)
                                    .Include(m => m.Prévision.Objectif.ActionProj.Projet)
                                    .Include(n => n.Prévision.Objectif.ActionProj.Projet.Programme)
                                    .Where(m => m.PrévisionId == id);
            if (!String.IsNullOrEmpty(SearchString))
            {
                prev = prev
                    .Where(s => s.Prévision.Objectif.ActionProj.Name.Contains(SearchString)
                                       || s.Dd.Name.Contains(SearchString)
                                       || s.NumDossier.Equals(SearchString));
            }
            switch (sortOrder)
            {
                case "Action": //Programme
                    prev = prev.OrderByDescending(s => s.Prévision.Objectif.ActionProj.Name);
                    break;
                case "Dd"://Projet
                    prev = prev.OrderBy(s => s.Dd);
                    break;
                case "Dd_desc"://Projet
                    prev = prev.OrderByDescending(s => s.Dd);
                    break;
                case "NumDossier"://Action
                    prev = prev.OrderBy(s => s.NumDossier);
                    break;
                case "NumDossier_desc"://Action
                    prev = prev.OrderByDescending(s => s.NumDossier);
                break;
                default:
                    prev = prev.OrderBy(s => s.Prévision.Objectif.ActionProj.Name);
                    break;
            }

            int pageSize = 4;
            return View(await PaginatedList<Detail>.CreateAsync(prev, pageNumber ?? 1, pageSize));

        
        
        
    }


    public IActionResult Unfound()
    {
        return View();
    }
    public IActionResult Found()
    {
        return View();
    }

    public async Task<IActionResult> DeleteDetails(int? id)
    {
        if (id == null || context.Details == null)
        {
            return NotFound();
        }

        var detail = await context.Details

            .FirstOrDefaultAsync(m => m.Id == id);

        if (detail == null)
        {
            return NotFound();
        }

        return View(detail);
    }

    [HttpPost, ActionName("DeleteDetails")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmedDetails(int id)
    {
        if (context.Details == null)
        {
            return Problem("Entity set 'projetCOMContext.Indicateurs'  is null.");
        }
        var detail = await context.Details.FindAsync(id);

        if (detail != null)
        {
            context.Details.Remove(detail);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(ListPrevision));
    }



    /// <summary>
    /// ---------------Valider---------------------
    /// </summary>
    /// <param name="Valider"></param>
    /// <returns></returns>
    public IActionResult Valider(int? id)
    {
        ViewBag.Date = DateTime.Now;
        ViewBag.Motif = null;


        //ViewData["ProfilId"] = new SelectList(context.Profils, "Id", "Name");
        //ViewData["StructureId"] = new SelectList(context.Structures, "Id", "Name");
        if (id == null || context.Prévisions == null)
        {
            return NotFound();
        }

        var prog = context.Prévisions
            .Include(x => x.Objectif.ActionProj.Projet.Programme)
            .Include(x => x.Objectif.ActionProj.Projet)
            .Include(x => x.Objectif.ActionProj)
            .Include(x => x.Objectif.Exercice)
            .Include(x => x.Objectif.Dr)
            .FirstOrDefault(x => x.Id == id);
        ;
        if (prog == null)
        {
            return NotFound();
        }
        return View(prog);
    }

    //// POST: Indicateurs/Edit/5
    //// To protect from overposting attacks, enable the specific properties you want to bind to.
    //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Valider(int Id, [Bind("Id,ObjectifId,Nombre,Superficie,Valeur,Date,Etat,MotifRejet")] Prévision prevision)
    {
        if (Id != prevision.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(prevision);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrévisionsExists(prevision.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(ListPrevision));
        }
        return View(prevision);
    }




    /// <summary>
    /// ---------------Rejeter---------------------
    /// </summary>
    /// <param name="Rejeter"></param>
    /// <returns></returns>
    public IActionResult Rejeter(int? id)
    {
        ViewBag.Date = DateTime.Now;



        //ViewData["ProfilId"] = new SelectList(context.Profils, "Id", "Name");
        //ViewData["StructureId"] = new SelectList(context.Structures, "Id", "Name");
        if (id == null || context.Prévisions == null)
        {
            return NotFound();
        }

        var prog = context.Prévisions
            .Include(x => x.Objectif.ActionProj.Projet.Programme)
            .Include(x => x.Objectif.ActionProj.Projet)
            .Include(x => x.Objectif.ActionProj)
            .Include(x => x.Objectif.Exercice)
            .Include(x => x.Objectif.Dr)
            .FirstOrDefault(x => x.Id == id);
        ;
        if (prog == null)
        {
            return NotFound();
        }
        return View(prog);
    }

    //// POST: Indicateurs/Edit/5
    //// To protect from overposting attacks, enable the specific properties you want to bind to.
    //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Rejeter(int Id, [Bind("Id,ObjectifId,Nombre,Superficie,Valeur,Date,Etat,MotifRejet")] Prévision prevision)
    {
        if (Id != prevision.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(prevision);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrévisionsExists(prevision.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(ListPrevision));
        }
        return View(prevision);
    }



    

    public async Task<IActionResult> ListRejet(string sortOrder, string currentFilter,
        string? SearchString, int? pageNumber)
    {


        ViewData["CurrentSort"] = sortOrder;
        ViewData["ProgrammeSortParm"] = sortOrder == "Programme" ? "Programme_desc" : "Programme";
        ViewData["ExerciceSortParm"] = sortOrder == "Exercice" ? "Exercice_desc" : "Exercice";
        ViewData["ProjetSortParm"] = sortOrder == "Projet" ? "Projet_desc" : "Projet";
        ViewData["ActionSortParm"] = sortOrder == "Action" ? "Action_desc" : "Action";
        ViewData["DrSortParm"] = sortOrder == "Dr" ? "Dr" : "Dr_desc";
        ViewData["DateSortParm"] = sortOrder == "Date" ? "Date_desc" : "Date";


        if (HttpContext.Session.GetString("Profil") == "DR")
        {
            IQueryable<Prévision> prevbad = context.Prévisions
                                        .Include(x => x.Objectif.Dr)
                                        .Include(x => x.Objectif.Exercice)
                                        .Include(x => x.Objectif.ActionProj)
                                        .Include(x => x.Objectif.ActionProj.Projet)
                                        .Include(x => x.Objectif.ActionProj.Projet.Programme)
                                        .Where(x => x.Objectif.Drid == HttpContext.Session.GetInt32("StructureId"))
                                        .Where(x => x.Etat == false)
                                        ;

            if (!String.IsNullOrEmpty(SearchString))
            {
                prevbad = prevbad
                        .Where(s => s.Objectif.ActionProj.Name.Contains(SearchString)
                                           || s.Objectif.ActionProj.Projet.Name.Contains(SearchString)
                                           || s.Objectif.ActionProj.Projet.Programme.Name.Contains(SearchString)
                                           || s.Objectif.Dr.Name.Contains(SearchString)
                                           || s.Objectif.Exercice.Annee.Contains(SearchString));
            }
            switch (sortOrder)
            {
                case "Programme_desc": //Programme
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.ActionProj.Projet.Programme.Name);
                    break;
                case "Projet"://Projet
                    prevbad = prevbad.OrderBy(s => s.Objectif.ActionProj.Projet.Name);
                    break;
                case "Projet_desc"://Projet
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.ActionProj.Projet.Name);
                    break;
                case "Action"://Action
                    prevbad = prevbad.OrderBy(s => s.Objectif.ActionProj.Name);
                    break;
                case "Action_desc"://Action
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.ActionProj.Name);
                    break;
                case "Exercice": //Exercice
                    prevbad = prevbad.OrderBy(s => s.Objectif.Exercice.Annee);
                    break;
                case "Exercice_desc": //Exercice
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.Exercice.Annee);
                    break;
                case "Dr": //Dr
                    prevbad = prevbad.OrderBy(s => s.Objectif.Dr.Name);
                    break;
                case "Dr_desc": //Dr
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.Dr.Name);
                    break;
                case "Date": //Dr
                    prevbad = prevbad.OrderBy(s => s.Date);
                    break;
                case "Date_desc": //Dr
                    prevbad = prevbad.OrderByDescending(s => s.Date);
                    break;
                default:
                    prevbad = prevbad.OrderBy(s => s.Objectif.ActionProj.Projet.Programme.Name);
                    break;
            }
            int pageSize = 4;

            return View(await PaginatedList<Prévision>.CreateAsync(prevbad, pageNumber ?? 1, pageSize));
        }
        else
        {
            IQueryable<Prévision> prevbad = context.Prévisions
                                        .Include(x => x.Objectif.Dr)
                                        .Include(x => x.Objectif.Exercice)
                                        .Include(x => x.Objectif.ActionProj)
                                        .Include(x => x.Objectif.ActionProj.Projet)
                                        .Include(x => x.Objectif.ActionProj.Projet.Programme)
                                        
                                        .Where(x => x.Etat == false)
                                        ;

            if (!String.IsNullOrEmpty(SearchString))
            {
                prevbad = prevbad
                        .Where(s => s.Objectif.ActionProj.Name.Contains(SearchString)
                                           || s.Objectif.ActionProj.Projet.Name.Contains(SearchString)
                                           || s.Objectif.ActionProj.Projet.Programme.Name.Contains(SearchString)
                                           || s.Objectif.Dr.Name.Contains(SearchString)
                                           || s.Objectif.Exercice.Annee.Contains(SearchString));
            }
            switch (sortOrder)
            {
                case "Programme_desc": //Programme
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.ActionProj.Projet.Programme.Name);
                    break;
                case "Projet"://Projet
                    prevbad = prevbad.OrderBy(s => s.Objectif.ActionProj.Projet.Name);
                    break;
                case "Projet_desc"://Projet
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.ActionProj.Projet.Name);
                    break;
                case "Action"://Action
                    prevbad = prevbad.OrderBy(s => s.Objectif.ActionProj.Name);
                    break;
                case "Action_desc"://Action
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.ActionProj.Name);
                    break;
                case "Exercice": //Exercice
                    prevbad = prevbad.OrderBy(s => s.Objectif.Exercice.Annee);
                    break;
                case "Exercice_desc": //Exercice
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.Exercice.Annee);
                    break;
                case "Dr": //Dr
                    prevbad = prevbad.OrderBy(s => s.Objectif.Dr.Name);
                    break;
                case "Dr_desc": //Dr
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.Dr.Name);
                    break;
                case "Date": //Dr
                    prevbad = prevbad.OrderBy(s => s.Date);
                    break;
                case "Date_desc": //Dr
                    prevbad = prevbad.OrderByDescending(s => s.Date);
                    break;
                default:
                    prevbad = prevbad.OrderBy(s => s.Objectif.ActionProj.Projet.Programme.Name);
                    break;
            }
            int pageSize = 4;

            return View(await PaginatedList<Prévision>.CreateAsync(prevbad, pageNumber ?? 1, pageSize));

        }

    }
    public async Task<IActionResult> ListValide(string sortOrder, string currentFilter, string? SearchString, int? pageNumber)
    {


        ViewData["CurrentSort"] = sortOrder;
        ViewData["ProgrammeSortParm"] = sortOrder == "Programme" ? "Programme_desc" : "Programme";
        ViewData["ExerciceSortParm"] = sortOrder == "Exercice" ? "Exercice_desc" : "Exercice";
        ViewData["ProjetSortParm"] = sortOrder == "Projet" ? "Projet_desc" : "Projet";
        ViewData["ActionSortParm"] = sortOrder == "Action" ? "Action_desc" : "Action";
        ViewData["DrSortParm"] = sortOrder == "Dr" ? "Dr" : "Dr_desc";
        ViewData["DateSortParm"] = sortOrder == "Date" ? "Date_desc" : "Date";


        if (HttpContext.Session.GetString("Profil") == "DR")
        {
            IQueryable<Prévision> prevbad = context.Prévisions
                                        .Include(x => x.Objectif.Dr)
                                        .Include(x => x.Objectif.Exercice)
                                        .Include(x => x.Objectif.ActionProj)
                                        .Include(x => x.Objectif.ActionProj.Projet)
                                        .Include(x => x.Objectif.ActionProj.Projet.Programme)
                                        .Where(x => x.Objectif.Drid == HttpContext.Session.GetInt32("StructureId"))
                                        .Where(x => x.Etat == true)
                                        ;

            if (!String.IsNullOrEmpty(SearchString))
            {
                prevbad = prevbad
                        .Where(s => s.Objectif.ActionProj.Name.Contains(SearchString)
                                           || s.Objectif.ActionProj.Projet.Name.Contains(SearchString)
                                           || s.Objectif.ActionProj.Projet.Programme.Name.Contains(SearchString)
                                           || s.Objectif.Dr.Name.Contains(SearchString)
                                           || s.Objectif.Exercice.Annee.Contains(SearchString));
            }
            switch (sortOrder)
            {
                case "Programme_desc": //Programme
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.ActionProj.Projet.Programme.Name);
                    break;
                case "Projet"://Projet
                    prevbad = prevbad.OrderBy(s => s.Objectif.ActionProj.Projet.Name);
                    break;
                case "Projet_desc"://Projet
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.ActionProj.Projet.Name);
                    break;
                case "Action"://Action
                    prevbad = prevbad.OrderBy(s => s.Objectif.ActionProj.Name);
                    break;
                case "Action_desc"://Action
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.ActionProj.Name);
                    break;
                case "Exercice": //Exercice
                    prevbad = prevbad.OrderBy(s => s.Objectif.Exercice.Annee);
                    break;
                case "Exercice_desc": //Exercice
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.Exercice.Annee);
                    break;
                case "Dr": //Dr
                    prevbad = prevbad.OrderBy(s => s.Objectif.Dr.Name);
                    break;
                case "Dr_desc": //Dr
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.Dr.Name);
                    break;
                case "Date": //Dr
                    prevbad = prevbad.OrderBy(s => s.Date);
                    break;
                case "Date_desc": //Dr
                    prevbad = prevbad.OrderByDescending(s => s.Date);
                    break;
                default:
                    prevbad = prevbad.OrderBy(s => s.Objectif.ActionProj.Projet.Programme.Name);
                    break;
            }
            int pageSize = 4;

            return View(await PaginatedList<Prévision>.CreateAsync(prevbad, pageNumber ?? 1, pageSize));
        }
        else
        {
            IQueryable<Prévision> prevbad = context.Prévisions
                                        .Include(x => x.Objectif.Dr)
                                        .Include(x => x.Objectif.Exercice)
                                        .Include(x => x.Objectif.ActionProj)
                                        .Include(x => x.Objectif.ActionProj.Projet)
                                        .Include(x => x.Objectif.ActionProj.Projet.Programme)

                                        .Where(x => x.Etat == true)
                                        ;

            if (!String.IsNullOrEmpty(SearchString))
            {
                prevbad = prevbad
                        .Where(s => s.Objectif.ActionProj.Name.Contains(SearchString)
                                           || s.Objectif.ActionProj.Projet.Name.Contains(SearchString)
                                           || s.Objectif.ActionProj.Projet.Programme.Name.Contains(SearchString)
                                           || s.Objectif.Dr.Name.Contains(SearchString)
                                           || s.Objectif.Exercice.Annee.Contains(SearchString));
            }
            switch (sortOrder)
            {
                case "Programme_desc": //Programme
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.ActionProj.Projet.Programme.Name);
                    break;
                case "Projet"://Projet
                    prevbad = prevbad.OrderBy(s => s.Objectif.ActionProj.Projet.Name);
                    break;
                case "Projet_desc"://Projet
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.ActionProj.Projet.Name);
                    break;
                case "Action"://Action
                    prevbad = prevbad.OrderBy(s => s.Objectif.ActionProj.Name);
                    break;
                case "Action_desc"://Action
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.ActionProj.Name);
                    break;
                case "Exercice": //Exercice
                    prevbad = prevbad.OrderBy(s => s.Objectif.Exercice.Annee);
                    break;
                case "Exercice_desc": //Exercice
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.Exercice.Annee);
                    break;
                case "Dr": //Dr
                    prevbad = prevbad.OrderBy(s => s.Objectif.Dr.Name);
                    break;
                case "Dr_desc": //Dr
                    prevbad = prevbad.OrderByDescending(s => s.Objectif.Dr.Name);
                    break;
                case "Date": //Dr
                    prevbad = prevbad.OrderBy(s => s.Date);
                    break;
                case "Date_desc": //Dr
                    prevbad = prevbad.OrderByDescending(s => s.Date);
                    break;
                default:
                    prevbad = prevbad.OrderBy(s => s.Objectif.ActionProj.Projet.Programme.Name);
                    break;
            }
            int pageSize = 4;

            return View(await PaginatedList<Prévision>.CreateAsync(prevbad, pageNumber ?? 1, pageSize));

        }
    }

        [HttpPost]
        public IActionResult ExportPrévisionData()
        {
            var builder = WebApplication.CreateBuilder();
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

            // Query to retrieve Prévisions data and related tables
            string query = @"
                SELECT PV.[Nombre], PV.[Superficie], PV.[Valeur], PV.[date], PV.[objectifId], PV.[Etat], PV.[MotifRejet],
                       D.[numDossier], D.[TRN], D.[indiceTRN], D.[numTRN], D.[Superficie] AS DetailSuperficie, D.[Valeur] AS DetailValeur,
                       DD.[Name] AS DDName, O.[Id] AS ObjectifId, O.[ExerciceId], O.[DRId],
                       E.[Annee] AS ExerciceName, DR.[Name] AS DRName, PR.[Name] AS ProgrammeName, P.[Name] AS ProjetName, AP.[Name] AS ActionProjName
                FROM Prévisions AS PV
                INNER JOIN Objectifs AS O ON PV.objectifId = O.Id
                INNER JOIN Exercices AS E ON O.ExerciceId = E.Id
                INNER JOIN DRs AS DR ON O.DRId = DR.Id
                INNER JOIN ActionProjs AS AP ON O.ActionProjId = AP.Id
                INNER JOIN Projets AS P ON AP.ProjetId = P.Id
                INNER JOIN Programmes AS PR ON P.ProgrammeId = PR.Id
                LEFT JOIN Details AS D ON PV.Id = D.PrévisionId
                LEFT JOIN DDs AS DD ON D.DDId = DD.Id";

            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Create a new Excel package
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                // Create the worksheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Prévisions Data");

                // Write column headers
                worksheet.Cells[1, 1].Value = "Nombre";
                worksheet.Cells[1, 2].Value = "Superficie";
                worksheet.Cells[1, 3].Value = "Valeur";
                worksheet.Cells[1, 4].Value = "Date";
                worksheet.Cells[1, 5].Value = "objectifId";
                worksheet.Cells[1, 6].Value = "Etat";
                worksheet.Cells[1, 7].Value = "MotifRejet";
                worksheet.Cells[1, 8].Value = "numDossier";
                worksheet.Cells[1, 9].Value = "TRN";
                worksheet.Cells[1, 10].Value = "indiceTRN";
                worksheet.Cells[1, 11].Value = "numTRN";
                worksheet.Cells[1, 12].Value = "DetailSuperficie";
                worksheet.Cells[1, 13].Value = "DetailValeur";
                worksheet.Cells[1, 14].Value = "DDName";
                worksheet.Cells[1, 15].Value = "ObjectifId";
                worksheet.Cells[1, 16].Value = "ExerciceId";
                worksheet.Cells[1, 17].Value = "DRId";
                worksheet.Cells[1, 18].Value = "ExerciceName";
                worksheet.Cells[1, 19].Value = "DRName";
                worksheet.Cells[1, 20].Value = "ProgrammeName";
                worksheet.Cells[1, 21].Value = "ProjetName";
                worksheet.Cells[1, 22].Value = "ActionProjName";

                // Write data rows
                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    worksheet.Cells[row + 2, 1].Value = dataTable.Rows[row]["Nombre"];
                    worksheet.Cells[row + 2, 2].Value = dataTable.Rows[row]["Superficie"];
                    worksheet.Cells[row + 2, 3].Value = dataTable.Rows[row]["Valeur"];
                    worksheet.Cells[row + 2, 4].Value = dataTable.Rows[row]["date"];
                    worksheet.Cells[row + 2, 5].Value = dataTable.Rows[row]["Etat"];
                    worksheet.Cells[row + 2, 6].Value = dataTable.Rows[row]["MotifRejet"];
                    worksheet.Cells[row + 2, 7].Value = dataTable.Rows[row]["numDossier"];
                    worksheet.Cells[row + 2, 8].Value = dataTable.Rows[row]["TRN"];
                    worksheet.Cells[row + 2, 9].Value = dataTable.Rows[row]["indiceTRN"];
                    worksheet.Cells[row + 2, 10].Value = dataTable.Rows[row]["numTRN"];
                    worksheet.Cells[row + 2, 11].Value = dataTable.Rows[row]["DetailSuperficie"];
                    worksheet.Cells[row + 2, 12].Value = dataTable.Rows[row]["DetailValeur"];
                    worksheet.Cells[row + 2, 13].Value = dataTable.Rows[row]["DDName"];
                    worksheet.Cells[row + 2, 14].Value = dataTable.Rows[row]["ExerciceName"];
                    worksheet.Cells[row + 2, 15].Value = dataTable.Rows[row]["DRName"];
                    worksheet.Cells[row + 2, 16].Value = dataTable.Rows[row]["ProgrammeName"];
                    worksheet.Cells[row + 2, 17].Value = dataTable.Rows[row]["ProjetName"];
                    worksheet.Cells[row + 2, 18].Value = dataTable.Rows[row]["ActionProjName"];
                }
                // Generate a unique file name
                string fileName = "PrévisionData_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string filePath = Path.Combine(folderPath, fileName);
                    byte[] excelBytes = excelPackage.GetAsByteArray();
                    System.IO.File.WriteAllBytes(filePath, excelBytes);

                    // Return the file path and name as JSON response
                    if (System.IO.File.Exists(filePath))
                    {
                        // File download successful
                        var result = new
                        {
                            success = true,
                            message = "Fichier téléchargé avec succès dans le répertoire 'Mes Documents' ",
                            filePath = filePath,
                            fileName = fileName
                        };
                        return Ok(result);
                    }
                    else
                    {
                        // File download failed
                        var result = new
                        {
                            success = false,
                            message = "Échec du téléchargement du fichier."
                        };
                        return BadRequest(result);
                    }
                }
            }
        }

        [HttpGet]
        public IActionResult ExportStockDataStatus()
        {

            var result = new
            {
                success = true,
                message = "File download complete",

            };

            return Json(result);
        }


    



}