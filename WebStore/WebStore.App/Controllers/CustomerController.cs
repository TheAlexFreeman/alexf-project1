using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebStore.App.Models;
using WebStore.BLL;
using WebStore.BLL.Interfaces;

namespace WebStore.App.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly ILocationRepository _locationRepo;
        private readonly IOrderRepository _orderRepo;
        public CustomerController(ICustomerRepository customerRepo, ILocationRepository locationRepo, IOrderRepository orderRepo)
        {
            _customerRepo = customerRepo ?? throw new ArgumentNullException("Customer repository cannot be null");
            _locationRepo = locationRepo ?? throw new ArgumentNullException(nameof(locationRepo));
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
        }

        [HttpGet]
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            Customer customer;
            try
            {
                customer = _customerRepo.GetCustomerById(id);

            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction(nameof(List));
            }
            return View(new CustomerViewModel(customer));
        }
        [HttpGet]
        // GET: Customer/Enter
        public ActionResult Enter()
        {
            return View(new CustomerViewModel());
        }

        // POST: Customer/Enter
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enter(CustomerViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }
                // TODO: Add insert logic here
                Customer customer;
                try
                {
                    customer = _customerRepo.GetCustomerByName(viewModel.FirstName, viewModel.LastName);
                }
                catch (KeyNotFoundException)
                {
                    customer = viewModel.AsCustomer;
                    _customerRepo.AddCustomer(customer);
                    _customerRepo.Save();
                    customer = _customerRepo.GetCustomerByName(customer.FirstName, customer.LastName);

                }
                return RedirectToAction(nameof(Details), new { id = customer.Id });
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult List(string search = "")
        {
            try
            {
                var customers = _customerRepo.SearchCustomersByName(search);
                return View(customers.Select(c => new CustomerViewModel(c)));
            }
            catch
            {
                // Error view model (How do I work this???)
                return View(nameof(Enter));
            }
        }

        [HttpGet]
        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var customer = _customerRepo.GetCustomerById(id);
                return View(new CustomerViewModel(customer));
            }
            catch
            {
                return View(nameof(List));
            }
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CustomerViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }
                // TODO: Add update logic here
                viewModel.Id = id;
                _customerRepo.UpdateCustomer(viewModel.AsCustomer);
                _customerRepo.Save();
                return RedirectToAction(nameof(Details), new { Id = id });
            }
            catch
            {
                return RedirectToAction(nameof(List));
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var customer = _customerRepo.GetCustomerById(id);
                return View(new CustomerViewModel(customer));
            }
            catch
            {
                // Use ErrorViewModel
                return RedirectToAction(nameof(List));
            }
        }

        // POST: Customer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CustomerViewModel viewModel)
        {
            try
            {
                // TODO: Add delete logic here
                _customerRepo.DeleteCustomer(id);
                return RedirectToAction(nameof(List));
            }
            catch
            {
                return View(viewModel);
            }
        }

        [HttpGet]
        public ActionResult History(int id)
        {
            try
            {
                var orders = _customerRepo.GetOrderHistory(id);
                return View(orders.Select(o => new OrderViewModel(o)));
            }
            catch
            {
                // Use ErrorViewModel
                return RedirectToAction(nameof(List));
            }
        }

        //[HttpGet]
        //public ActionResult SelectLocation()
        //{
        //    try
        //    {
        //        //
        //    }
        //}


        [HttpGet]
        public ActionResult Order(int id, string storeName)
        {
            try
            {
                var seller = new LocationViewModel(_locationRepo.GetLocationByName(storeName));
                var buyer = new CustomerViewModel(_customerRepo.GetCustomerById(id));
                var order = new Order(buyer.AsCustomer, seller.AsLocation);
                _orderRepo.AddOrder(order);
                _orderRepo.Save();
                return View(new OrderViewModel(order));
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction(nameof(LocationController.Select));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Order(OrderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var order = viewModel.AsOrder;
            _orderRepo.UpdateOrder(order);
            _orderRepo.Save();
            return RedirectToAction(nameof(OrderHistory), new { id = viewModel.Buyer.Id });
        }

        [HttpGet]
        public ActionResult OrderHistory(int id)
        {
            try
            {
                var orders = _orderRepo.GetOrderHistoryByCustomer(id);
                return View(orders.Select(o => new OrderViewModel(o)));
            }
            catch
            {
                return View(nameof(Details), new { id });
            }
        }
    }
}