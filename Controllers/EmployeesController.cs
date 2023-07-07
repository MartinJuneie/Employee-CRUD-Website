using CRUD.Data;
using CRUD.Models;
using CRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly CRUDDBContext cRUDDBContext;

        public EmployeesController(CRUDDBContext cRUDDBContext)
        {
            this.cRUDDBContext = cRUDDBContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await cRUDDBContext.Employees.ToListAsync();
            return View(employees);

        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                DateOfBirth = addEmployeeRequest.DateOfBirth,
                Department = addEmployeeRequest.Department
            };
            await cRUDDBContext.Employees.AddAsync(employee);
            await cRUDDBContext.SaveChangesAsync();
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await cRUDDBContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = Guid.NewGuid(),
                    Name = employee.Name,
                    Email = employee.Email,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department
                };
                return await Task.Run(() => View("View", viewModel));
            }

            return RedirectToAction("Index"); 
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await cRUDDBContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

                await cRUDDBContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await cRUDDBContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                cRUDDBContext.Employees.Remove(employee);
                await cRUDDBContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
