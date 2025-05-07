
using WebApplication1.Models;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Services;

public interface IDbService
{
    Task<GetVisits> getVisits(int customerId);
    Task AddNewVisit(int customerId, GetVisits getVisits);
}