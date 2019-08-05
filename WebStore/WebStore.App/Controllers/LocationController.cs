using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebStore.App.Models;
using WebStore.BLL.Interfaces;

namespace WebStore.App.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationRepository _locationRepo;
        public LocationController(ILocationRepository locationRepo)
        {
            _locationRepo = locationRepo ?? throw new ArgumentNullException(nameof(locationRepo));
        }

        // GET: Location
        public ActionResult Index([FromQuery] string search = "")
        {
            IEnumerable<LocationViewModel> viewModel = _locationRepo.GetLocations()
                .Where(l => l.Name.Contains(search))
                .Select(l => new LocationViewModel(l));
            return View(viewModel);
        }

        [HttpGet]
        // GET: Location/Details/5
        public ActionResult Details(int id)
        {
            BLL.Location location;
            try
            {
                location = _locationRepo.GetLocationById(id);
            } catch (KeyNotFoundException)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(new LocationViewModel(location));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        // GET: Location/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocationViewModel viewModel)
        {
            try
            {
                // TODO: Add insert logic here
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }
                var newLocation = viewModel.AsLocation;
                _locationRepo.AddLocation(newLocation);
                _locationRepo.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Location/Edit/5
        public ActionResult Edit(int id)
        {
            BLL.Location location;
            try
            {
                location = _locationRepo.GetLocationById(id);
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(new LocationViewModel(location));
        }

        // POST: Location/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, LocationViewModel viewModel)
        {
            try
            {
                // TODO: Add update logic here
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }
                var newLocation = viewModel.AsLocation;
                _locationRepo.EditLocation(id, newLocation);
                _locationRepo.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(viewModel);
            }
        }

        // GET: Location/Delete/5
        public ActionResult Delete(int id)
        {
            BLL.Location location;
            try
            {
                location = _locationRepo.GetLocationById(id);
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(new LocationViewModel(location));
        }

        // POST: Location/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, LocationViewModel viewModel)
        {
            try
            {
                // TODO: Add delete logic here
                _locationRepo.DeleteLocation(id);
                _locationRepo.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}