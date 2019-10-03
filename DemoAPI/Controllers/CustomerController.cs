using DemoAPI.Helpers;
using DemoAPI.Models;
using DemoAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] PagingParams pagingParams)
        {
            var paged = await _customerService.GetAllPagingAsync(pagingParams);
            return Ok(new
            {
                paged.Items,
                paged.CurrentPage,
                paged.PageSize,
                paged.TotalCount,
                paged.TotalPages
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetById(int id)
        {
            var customer = await _customerService.GetAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Create(Customer customer)
        {
            var result = await _customerService.AddAsync(customer);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            await _customerService.UpdateAsync(customer);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerService.GetAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            await _customerService.DeleteAsync(customer);

            return NoContent();
        }
    }
}