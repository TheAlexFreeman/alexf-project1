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
        public CustomerController(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo ?? throw new ArgumentNullException("Customer repository cannot be null");
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

            } catch (KeyNotFoundException)
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
                } catch (KeyNotFoundException)
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
            } catch
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
            } catch
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
    }
}