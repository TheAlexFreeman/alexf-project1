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
        private readonly ICustomerRepository _customerRepo;
        private readonly IOrderRepository _orderRepo;
        public LocationController(ILocationRepository locationRepo, ICustomerRepository customerRepo, IOrderRepository orderRepo)
        {
            _locationRepo = locationRepo ?? throw new ArgumentNullException(nameof(locationRepo));
            _customerRepo = customerRepo ?? throw new ArgumentNullException(nameof(customerRepo));
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));


        }

        // GET: Location
        public ActionResult Select([FromQuery] string search = "")
        {
            IEnumerable<LocationViewModel> viewModel = _locationRepo.GetLocations(search)
                .Select(l => new LocationViewModel(l));
            return View(viewModel);
        }

        [HttpGet]
        // GET: Location/Details/5
        public ActionResult Inventory(int id)
        {
            BLL.Location location;
            try
            {
                location = _locationRepo.GetLocationById(id);
            } catch (KeyNotFoundException)
            {
                return RedirectToAction(nameof(Select));
            }
            return View(new LocationViewModel(location));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        // GET: Location/Create
        public ActionResult Create()
        {
            return View(new LocationViewModel());
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
                var newLocation = viewModel.AsLocation();
                _locationRepo.AddLocation(newLocation);
                _locationRepo.Save();
                return RedirectToAction(nameof(Select));
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
                return RedirectToAction(nameof(Select));
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
                var newLocation = viewModel.AsLocation();
                _locationRepo.EditLocation(id, newLocation);
                _locationRepo.Save();
                return RedirectToAction(nameof(Select));
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
                return RedirectToAction(nameof(Select));
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
                return RedirectToAction(nameof(Select));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult OrderHistory(int id)
        {
            try
            {
                var orders = _orderRepo.GetOrderHistoryByLocation(id);
                return View(orders.Where(o => o.Id > 19).Select(o => new OrderViewModel(o)));
            }
            catch
            {
                return View(nameof(Select), new { id });
            }
        }
    }
}