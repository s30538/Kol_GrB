using System.Data.Common;
using Microsoft.Data.SqlClient;
using WebApplication1.Exceptions;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Services;

public class DbService : IDbService
{
    private readonly string _connectionString;

    public DbService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default") ?? string.Empty;
    }


    public async Task<GetVisits> getVisits(int customerId)
    {
        var query =
            @"SELECT clinet_id, first_name ,last_name, date_ofbirth, r.rental_id, rental_date, return_date, s.name, ri.price_at_rental, m.title
            FROM Client c
            JOIN Visit v ON v.client_id = client_id
            JOIN Mechanics m ON m.mechanic_id = v.mechanic_id
            JOIN Visit_Service vs ON vs.visit_id = v.visit_id
            JOIN Service s ON s.service_id = vs.rental_id
            JOIN Movie mo ON mo.movie_id = ri.movie_id
            WHERE c.client_id = @client_id;";

        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        await connection.OpenAsync();

        command.Parameters.AddWithValue("@customerId", customerId);
        var reader = await command.ExecuteReaderAsync();

        GetVisits? visits = null;
        while (await reader.ReadAsync())
        {
            if (visits is null)
            {
                visits = new GetVisits
                {
                    FirstName_Client = reader.GetString(0),
                    LastName_Client = reader.GetString(1),
                    DateOfBirth_Client = reader.GetDateTime(2),

                    Id_Mechanic = reader.GetInt32(3),
                    Licence_Mechanic = reader.GetString(4),
                    Visit = new List<Visit>()
                };
                int rentalId = reader.GetInt32(5);

                var visit = visits.Visit.FirstOrDefault(e => e.Name.Equals(customerId));
                if (visit is null)
                {
                    visit = new Visit()
                    {
                        Name = reader.GetString(6),
                        serviceFee = reader.GetFloat(7)

                    };
                    visits.Visit.Add(visit);
                }
            }


        }

        return visits;
    }

    public async Task AddNewVisit(int customerId, GetVisits getVisits)
    {
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();
        
        DbTransaction transaction = await connection.BeginTransactionAsync();
        command.Transaction = transaction as SqlTransaction;
        
        var customerIdRes = await command.ExecuteScalarAsync();
        if(customerIdRes is null)
            throw new NotFoundException($"Customer with ID - {customerId} - not found.");
            
        command.Parameters.Clear();
    }
}