using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IProductRepository _productRepo;
        public CustomerController(ICustomerRepository customerRepo, ILocationRepository locationRepo, IOrderRepository orderRepo, IProductRepository productRepo)
        {
            _customerRepo = customerRepo ?? throw new ArgumentNullException("Customer repository cannot be null");
            _locationRepo = locationRepo ?? throw new ArgumentNullException(nameof(locationRepo));
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
            _productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
        }

        [HttpGet]
        // GET: Customer
        public ActionResult Index()
        {
            var viewModel = new CustomerViewModel();
            foreach(Customer customer in _customerRepo.GetCustomers(""))
            {
                viewModel.CustomerOptions.Add(new SelectListItem
                {
                    Value = customer.Id.ToString(),
                    Text = customer.FullName,
                });
            }
            return View(viewModel);
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
                var customers = _customerRepo.GetCustomers(search);
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
                var seller = _locationRepo.GetLocationByName(storeName);
                var buyer = _customerRepo.GetCustomerById(id);
                var order = new Order(buyer, seller, new Dictionary<Product, int>(seller.Products.Select(p => new KeyValuePair<Product, int>(p, 0))));
                _orderRepo.AddOrder(order);
                _orderRepo.Save();
                order = _orderRepo.GetLatestOrder(id, seller.Id);
                return View(new OrderViewModel(order));
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction(nameof(LocationController.Select));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Order(int id, int storeId, IFormCollection formCollection)
        {
            var values = new Dictionary<string, string>();
            
            //if (!ModelState.IsValid)
            //{
            //    return View(viewModel);
            //}
            var order = _orderRepo.GetLatestOrder(id, storeId);
            foreach (Product product in order.ProductSet)
            {
                if (formCollection.ContainsKey(product.Name) && formCollection[product.Name] != "")
                {
                    order.AddProduct(product, int.Parse(formCollection[product.Name]));
                }
            }
            //new Dictionary<Product, int>(viewModel.Prices.Select(kvp =>
            //         new KeyValuePair<Product, int>(new Product(kvp.Key, kvp.Value), viewModel.Quantity(kvp.Key)))));
            order.Close();
            _orderRepo.UpdateOrder(order);
            _orderRepo.Save();
            return RedirectToAction(nameof(OrderHistory), new { id = id });
        }

        [HttpGet]
        public ActionResult OrderHistory(int id)
        {
            try
            {
                var orders = _orderRepo.GetOrderHistoryByCustomer(id);
                return View(orders.Where(o => o.Id > 19).Select(o => new OrderViewModel(o)));
            }
            catch
            {
                return View(nameof(Details), new { id });
            }
        }
    }
}