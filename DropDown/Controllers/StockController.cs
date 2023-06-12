using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using Castle.Core.Resource;
using DropDown.Data;
using DropDown.Models;
using DropDown.Models.Cascade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Microsoft.Extensions.Configuration;




namespace DropDown.Controllers
{
    public class StockController : Controller
    {
        private readonly DropDownContext context;
        public StockController(DropDownContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            
            return View();
        }
        [HttpGet]
        public IActionResult Stock()
        {
            ViewBag.Date = DateTime.Now;

           var id = TempData["ObjectifId"];
            ViewBag.ObjectifId = id;

            var req =context.Objectifs
                
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
            


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Stock(int? ObjectifId, [Bind("ObjectifId,Nombre,Superficie,Name,Valeur,Date")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                context.Add(stock);
                await context.SaveChangesAsync();
                TempData["ObjectifId"] = stock.Id;
                return RedirectToAction(nameof(ListStock));
            }
            if (ObjectifId == null || context.Stocks == null)
            {
                return NotFound();
            }
            var act = await context.Stocks.Include(x=>x.Objectif)
                .Include(x=>x.Objectif.Exercice)
                .Include(x => x.Objectif.Dr)
                .Include(x=>x.Objectif.ActionProj)
                .Include(x => x.Objectif.ActionProj.Projet)
                .Include(x => x.Objectif.ActionProj.Projet.Programme)
                 .FirstOrDefaultAsync(m => m.ObjectifId == ObjectifId);

            ViewBag.Programme = act.Objectif.ActionProj.Projet.Programme.Name;
            if (act == null)
            {
                return NotFound();
            }

            return View(act);
        }
        public async Task<IActionResult> ListStock(string sortOrder, string currentFilter,
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
                IQueryable<Stock> prevbad = context.Stocks.Include(i => i.Objectif)
                                                .Include(m => m.Objectif.Dr)
                                                .Include(o => o.Objectif.Exercice)
                                                .Include(p => p.Objectif.ActionProj)
                                                .Include(l => l.Objectif.ActionProj.Projet)
                                                .Include(k => k.Objectif.ActionProj.Projet.Programme)
                                                .Where(x => x.Objectif.Drid == HttpContext.Session.GetInt32("StructureId"));


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

                return View(await PaginatedList<Stock>.CreateAsync(prevbad, pageNumber ?? 1, pageSize));
            }
            else
            {
                IQueryable<Stock> prevbad = context.Stocks.Include(i => i.Objectif)
                                                .Include(m => m.Objectif.Dr)
                                                .Include(o => o.Objectif.Exercice)
                                                .Include(p => p.Objectif.ActionProj)
                                                .Include(l => l.Objectif.ActionProj.Projet)
                                                .Include(k => k.Objectif.ActionProj.Projet.Programme)
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

                return View(await PaginatedList<Stock>.CreateAsync(prevbad, pageNumber ?? 1, pageSize));

            }
            
            
        }

        // GET: Indicateurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            ViewBag.Date = DateTime.Now;

            ViewBag.Programme = TempData["Programme"];
            ViewBag.Projet = TempData["Projet"];
            ViewBag.Action = TempData["Action"];
            ViewBag.Dr = TempData["Dr"];

           
            //ViewBag.ObjectifId = TempData["ObjectifId"];
            //ViewData["ProfilId"] = new SelectList(context.Profils, "Id", "Name");
            //ViewData["StructureId"] = new SelectList(context.Structures, "Id", "Name");
            if (id == null || context.Stocks == null)
            {
                return NotFound();
            }

            var prog = await context.Stocks.FindAsync(id);
            if (prog == null)
            {
                return NotFound();
            }
            ViewBag.ObjectifId=prog.ObjectifId;
            return View(prog);
        }

        //// POST: Indicateurs/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,ObjectifId,Nombre,Superficie,Valeur,Date")] Stock stock)
        {
            if (Id != stock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(stock);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(stock.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListStock));
            }
            return View(stock);
        }

        private bool StockExists(int id)
        {
            return (context.Stocks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || context.Stocks == null)
            {
                return NotFound();
            }

            var stock = await context.Stocks
                .Include(i => i.Objectif)
                .ThenInclude(i => i.ActionProj)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (context.Stocks == null)
            {
                return Problem("Entity set 'projetCOMContext.Indicateurs'  is null.");
            }
            var stock = await context.Stocks.FindAsync(id);
            var obj =context.Objectifs.Where(x => x.Id == stock.ObjectifId).FirstOrDefault();

            if (stock != null)
            {
                context.Stocks.Remove(stock);
            }
            if (obj != null)
            {
                context.Objectifs.Remove(obj);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(ListStock));
        }

        private bool IndicateurExists(int id)
        {
            return (context.Stocks?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        [HttpPost]
        public IActionResult ExportStockData()
        {
            var builder = WebApplication.CreateBuilder();
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Query to retrieve stock data and related tables
                string query = @"
        SELECT S.[Nombre], S.[Superficie], S.[Valeur], S.[date], E.[Annee] AS ExerciceName, DR.[Name] AS DRName, PR.[Name] AS ProgrammeName, P.[Name] AS ProjetName, AP.[Name] AS ActionProjName
        FROM Stocks AS S
        INNER JOIN Objectifs AS O ON S.objectifId = O.Id
        INNER JOIN Exercices AS E ON O.ExerciceId = E.Id
        INNER JOIN DRs AS DR ON O.DRId = DR.Id
        INNER JOIN ActionProjs AS AP ON O.ActionProjId = AP.Id
        INNER JOIN Projets AS P ON AP.ProjetId = P.Id
        INNER JOIN Programmes AS PR ON P.ProgrammeId = PR.Id";

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // Create a new Excel package
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    // Create the worksheet
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Stock Data");

                    // Write column headers
                    worksheet.Cells[1, 1].Value = "Nombre";
                    worksheet.Cells[1, 2].Value = "Superficie";
                    worksheet.Cells[1, 3].Value = "Valeur";
                    worksheet.Cells[1, 4].Value = "Date";
                    worksheet.Cells[1, 5].Value = "Exercice";
                    worksheet.Cells[1, 6].Value = "DR";
                    worksheet.Cells[1, 7].Value = "Programme";
                    worksheet.Cells[1, 8].Value = "Projet";
                    worksheet.Cells[1, 9].Value = "Action Project";

                    // Write data rows
                    for (int row = 0; row < dataTable.Rows.Count; row++)
                    {
                        worksheet.Cells[row + 2, 1].Value = dataTable.Rows[row]["Nombre"];
                        worksheet.Cells[row + 2, 2].Value = dataTable.Rows[row]["Superficie"];
                        worksheet.Cells[row + 2, 3].Value = dataTable.Rows[row]["Valeur"];
                        worksheet.Cells[row + 2, 4].Value = dataTable.Rows[row]["date"];
                        worksheet.Cells[row + 2, 5].Value = dataTable.Rows[row]["ExerciceName"];
                        worksheet.Cells[row + 2, 6].Value = dataTable.Rows[row]["DRName"];
                        worksheet.Cells[row + 2, 7].Value = dataTable.Rows[row]["ProgrammeName"];
                        worksheet.Cells[row + 2, 8].Value = dataTable.Rows[row]["ProjetName"];
                        worksheet.Cells[row + 2, 9].Value = dataTable.Rows[row]["ActionProjName"];
                    }

                    // Generate a unique file name
                    string fileName = "StockData_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
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

                        return Json(result);
                    }
                    else
                    {
                        // File download failed
                        var result = new
                        {
                            success = false,
                            message = "File download failed"
                        };

                        return Json(result);
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
}
