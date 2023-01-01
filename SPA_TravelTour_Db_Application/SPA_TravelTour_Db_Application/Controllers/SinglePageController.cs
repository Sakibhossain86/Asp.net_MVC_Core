using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPA_TravelTour_Db_Application.Models;
using SPA_TravelTour_Db_Application.ViewModels;

namespace SPA_TravelTour_Db_Application.Controllers
{
    public class SinglePageController : Controller
    {
        TravelTourDbContext db;
        IWebHostEnvironment webHost;
        public SinglePageController(TravelTourDbContext db, IWebHostEnvironment webHost)
        {
            this.db = db;
            this.webHost = webHost;
        }
        public async Task<IActionResult> Index()
        {
            var id = 0;
            if (db.TravelAgents.Any())
            {
                id = db.TravelAgents.ToList()[0].TravelAgentId;
            }

            DataViewModel data = new DataViewModel();
            data.SelectedAgentId = id;
            data.TravelAgents = await db.TravelAgents.ToListAsync();
            data.TourPackages = await db.TourPackages.ToListAsync();
            data.PackageFeatures = await db.PackageFeatures.ToListAsync();
            data.Tourists = await db.Tourists.ToListAsync();
            data.AgentTourPackages = await db.AgentTourPackages.Where(oi => oi.TravelAgentId == id).ToListAsync();


            return View(data);
        }
        #region child actions
        public async Task<IActionResult> GetSelectedAgentPackages(int id)
        {

            var agentTourPackages = await db.AgentTourPackages.Include(x => x.TourPackage).Where(oi => oi.TravelAgentId == id).ToListAsync();
            return PartialView("_AgentTourPackageTable", agentTourPackages);
        }
        //Travel Agent Start// Master Details View//
        public async Task<IActionResult> CreateTravelAgent()
        {
            ViewData["TourPackage"] = await db.TourPackages.ToListAsync();
            return PartialView("_CreateTravelAgent");
        }
        [HttpPost]
        public async Task<IActionResult> CreateTravelAgent(TravelAgent o, int[] TourPackageId)
        {
            if (ModelState.IsValid)
            {
                for (var i = 0; i < TourPackageId.Length; i++)
                {
                    o.AgentTourPackages.Add(new AgentTourPackage { TourPackageId = TourPackageId[i]});
                }
                await db.TravelAgents.AddAsync(o);

                await db.SaveChangesAsync();


                var ord = await GetTravelAgent(o.TravelAgentId);
                return Json(ord);
            }
            return BadRequest();
        }
        public async Task<IActionResult> EditTravelAgent(int id)
        {
            //ViewData["Products"] = await db.Products.ToListAsync();
            //ViewData["Customers"] = await db.Customers.ToListAsync();
            var data = await db.TravelAgents
                .Include(x => x.AgentTourPackages)
                .FirstOrDefaultAsync(x => x.TravelAgentId == id);
            return PartialView("_EditTravelAgent", data);

        }
        [HttpPost]
        public async Task<IActionResult> EditTravelAgent(TravelAgent t)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(t);
            }

            return BadRequest();
        }
        public async Task<IActionResult> DeleteTravelAgent(int id)
        {
            var o = new TravelAgent { TravelAgentId = id };
            db.Entry(o).State = EntityState.Deleted;
            await db.SaveChangesAsync();
            return Json(new { success = true, message = "Data deleted" });
        }
        private async Task<TravelAgent?> GetTravelAgent(int id)
        {
            var o = await db.TravelAgents.FirstOrDefaultAsync(x => x.TravelAgentId == id);
            return o;
        }
        
        public async Task<IActionResult> CreateAgentPackage()
        {
            ViewData["TourPackage"] = await db.TourPackages.ToListAsync();
            return PartialView("_CreateAgentPackage");
        }
        public async Task<IActionResult> CreateAgentTourPackages(int id)
        {
            ViewData["TravelAgentId"] = id;
            ViewData["TourPackage"] = await db.TourPackages.ToListAsync();
            return PartialView("_CreateAgentTourPackages");
        }
        [HttpPost]
        public async Task<IActionResult> CreateAgentTourPackages(AgentTourPackage ap)
        {
            if (ModelState.IsValid)
            {
                await db.AgentTourPackages.AddAsync(ap);
                await db.SaveChangesAsync();
                var o = await GetTravelAgent(ap.TravelAgentId, ap.TourPackageId);
                return Json(o);
            }
            ViewData["TourPackage"] = await db.TourPackages.ToListAsync();
            return BadRequest();
        }
        public async Task<IActionResult> EditAgentTourPackages(int tid, int pid)
        {
            ViewData["TourPackage"] = await db.TourPackages.ToListAsync();
            var ap = await db.AgentTourPackages.FirstAsync(x => x.TravelAgentId == tid && x.TourPackageId == pid);
            return PartialView("_EditAgentTourPackages", ap);
        }
        [HttpPost]
        public async Task<IActionResult> EditAgentTourPackages(AgentTourPackage ap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ap).State = EntityState.Modified;
                await db.SaveChangesAsync();
                var o = await GetTravelAgent(ap.TravelAgentId, ap.TourPackageId);
                return Json(o);
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAgentTourPackages([FromQuery] int tid, [FromQuery] int pid)
        {

            var o = new AgentTourPackage { TourPackageId = pid, TravelAgentId = tid };
            db.Entry(o).State = EntityState.Deleted;

            await db.SaveChangesAsync();

            return Json(new { success = true, message = "Data deleted" });

        }
        private async Task<AgentTourPackage> GetTravelAgent(int tid, int pid)
        {
            var oi = await db.AgentTourPackages
                .Include(o => o.TravelAgent)
                .Include(o => o.TourPackage)
                .FirstAsync(x => x.TravelAgentId == tid && x.TourPackageId == pid);
            return oi;
        }
        //end master details
        //TourPackage Start
        public async Task<IActionResult> GetTourPackages()
        {
            return PartialView("_TourPackageTable", await db.TourPackages.ToListAsync());
        }
        public IActionResult CreateTourPackage()
        {
            return PartialView("_CreateTourPackage");
        }
        [HttpPost]
        public async Task<IActionResult> CreateTourPackage(TourPackage t)
        {
            if (ModelState.IsValid)
            {
                await db.TourPackages.AddAsync(t);
                await db.SaveChangesAsync();
                return Json(t);
            }
            return BadRequest("Unexpected Error");
        }
        public async Task<IActionResult> EditTourPackage(int id)
        {
            var data = await db.TourPackages.FirstOrDefaultAsync(c => c.TourPackageId == id);
            return PartialView("_EditTourPackage", data);
        }
        [HttpPost]
        public async Task<IActionResult> EditTourPackage(TourPackage t)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(t);
            }
            return BadRequest("Unexpected error");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTourPackage(int id)
        {
            if (!await db.PackageFeatures.AnyAsync(o => o.TourPackageId == id))
            {
                var o = new TourPackage { TourPackageId = id };
                db.Entry(o).State = EntityState.Deleted;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
                return Json(new { success = true, message = "Data deleted" });
            }
            return Json(new { success = false, message = "Cannot delete, item has related child." });
        }
        //TourPackage End
        //PackageFreature Start
        public async Task<IActionResult> GetPackageFeature()
        {
            return PartialView("_PackageFeatureTable", await db.PackageFeatures.ToListAsync());
        }
        public async Task<IActionResult> CreatePackageFeature()
        {
            ViewData["TourPackage"] = await db.TourPackages.ToListAsync(); 
            return PartialView("_CreatePackageFeature");
        }
        [HttpPost]
        public async Task<IActionResult> CreatePackageFeature(PackageFeature packageFeature)
        {
            if (ModelState.IsValid)
            {
                await db.PackageFeatures.AddAsync(packageFeature);
                await db.SaveChangesAsync();
                var r = GetTourPackage2(packageFeature.PackageFeatureId);
                return Json(packageFeature);
            }
            return BadRequest("Falied to insert product");
        }
        public async Task<IActionResult> EditPackageFeature(int id)
        {
            ViewData["TourPackage"] = await db.TourPackages.ToListAsync();
            var data = await db.PackageFeatures.FirstOrDefaultAsync(c => c.PackageFeatureId == id);
            return PartialView("_EditPackageFeature", data);
        }
        [HttpPost]
        public async Task<IActionResult> EditPackageFeature(PackageFeature t)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t).State = EntityState.Modified;
                await db.SaveChangesAsync();
                var r = GetTourPackage2(t.PackageFeatureId);
                return Json(t);
            }
            return BadRequest("Unexpected error");
        }
        private PackageFeature? GetTourPackage2(int id)
        {
            return db.PackageFeatures.Include(x => x.TourPackage).FirstOrDefault(x => x.PackageFeatureId == id);
        }
        [HttpPost]
        public async Task<IActionResult> DeletePackageFeature(int id)
        {
            PackageFeature t = new PackageFeature { PackageFeatureId = id };
            db.Entry(t).State = EntityState.Deleted;
            try
            {
                    await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            return Json(new { success = true, message = "Data deleted" });
        }
        //PackageFreature End
        //Tourist Start
        public IActionResult CreateTourist()
        {
            ViewData["TourPackage"] = new SelectList(db.Set<TourPackage>(), "TourPackageId", "PackageName");
            return PartialView("_CreateTourist");
        }
        [HttpPost]
        public async Task<IActionResult> CreateTourist(TouristInputModel t)
        {
            if (ModelState.IsValid)
            {
                var tourist = new Tourist { TouristName = t.TouristName, BookingDate  = t.BookingDate,TouristOccupation=t.TouristOccupation, TourPackageId=t.TourPackageId };
                string fileName = Guid.NewGuid() + Path.GetExtension(t.TouristPicture.FileName);
                string savePath = Path.Combine(this.webHost.WebRootPath, "Pictures", fileName);
                var fs = new FileStream(savePath, FileMode.Create);
                t.TouristPicture.CopyTo(fs);
                fs.Close();
                tourist.TouristPicture = fileName;
                await db.Tourists.AddAsync(tourist);
                await db.SaveChangesAsync();
                var x = GetTourPackage(tourist.TouristId);
                return Json(tourist);
            }
            ViewData["TourPackage"] = new SelectList(db.Set<TourPackage>(), "TourPackageId", "PackageName");
            return BadRequest("Falied to insert product");
        }
        public async Task<IActionResult> EditTourist(int id)
        {
            ViewData["TourPackage"] = new SelectList(db.Set<TourPackage>(), "TourPackageId", "PackageName");
            var data = await db.Tourists.FirstAsync(x => x.TouristId == id);
            ViewData["CurrentPic"] = data.TouristPicture;  
            return PartialView("_EditTourist", new TouristEditModel { TouristId = data.TouristId, TouristName = data.TouristName, BookingDate = data.BookingDate, TouristOccupation = data.TouristOccupation, TourPackageId = data.TourPackageId});
        }
        [HttpPost]
        public async Task<IActionResult> EditTourist(TouristEditModel t)
        {
            if (ModelState.IsValid)
            {
                var tourist = await db.Tourists.FirstAsync(x => x.TouristId == t.TouristId);
                tourist.TouristName = t.TouristName;
                tourist.BookingDate = t.BookingDate;
                tourist.TouristOccupation = t.TouristOccupation;
                tourist.TourPackageId = t.TourPackageId;
                if (t.TouristPicture != null)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(t.TouristPicture.FileName);
                    string savePath = Path.Combine(this.webHost.WebRootPath, "Pictures", fileName);
                    var fs = new FileStream(savePath, FileMode.Create);
                    t.TouristPicture.CopyTo(fs);
                    fs.Close();
                    tourist.TouristPicture = fileName;
                }
                await db.SaveChangesAsync();
                var x = GetTourPackage(tourist.TouristId);
                return Json(tourist);


            }
            ViewData["TourPackage"] = new SelectList(db.Set<TourPackage>(), "TourPackageId", "PackageName");
            return BadRequest();
        }
        private Tourist? GetTourPackage(int id)
        {
            return db.Tourists.Include(x=>x.TourPackage).FirstOrDefault(x=>x.TouristId==id);
        }
        public async Task<IActionResult> DeleteTourist(int id)
        {
            Tourist t = new Tourist { TouristId = id };
            db.Entry(t).State = EntityState.Deleted;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            return Json(new { success = true, message = "Data deleted" });
        }
        //Tourist End
        #endregion
    }
}