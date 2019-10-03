using DemoAPI.Helpers;
using DemoAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAPI.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<PagedList<Customer>> GetAllPagingAsync(PagingParams pagingParams);
        Task<Customer> GetAsync(int id);
        Task<Customer> AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Customer customer);
    }

    public class CustomerService : ICustomerService
    {
        private readonly CustomerContext _db;

        public CustomerService(CustomerContext context)
        {
            _db = context;
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();
            return customer;
        }

        public async Task DeleteAsync(Customer customer)
        {
            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var customers = await _db.Customers.ToListAsync();
            return customers;
        }

        public async Task<PagedList<Customer>> GetAllPagingAsync(PagingParams pagingParams)
        {
            var query = from c in _db.Customers
                        select c;

            if (!string.IsNullOrEmpty(pagingParams.Keyword))
            {
                query = query.Where(x => x.Name.ToLower().Contains(pagingParams.Keyword) ||
                                            x.Email.ToLower().Contains(pagingParams.Keyword) ||
                                            x.BirthDay.Contains(pagingParams.Keyword) ||
                                            x.PhoneNumber.Contains(pagingParams.Keyword) ||
                                            x.Address.Contains(pagingParams.Keyword));
            }

            if (pagingParams.FilterGender.HasValue)
            {
                query = query.Where(x => x.Gender == pagingParams.FilterGender);
            }

            return await PagedList<Customer>.CreateAsync(query, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<Customer> GetAsync(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            return customer;
        }

        public async Task UpdateAsync(Customer customer)
        {
            _db.Entry(customer).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
    }
}
