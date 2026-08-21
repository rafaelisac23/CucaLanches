using System.Data;
using CucaLanches.Application.Orders.Interfaces;
using CucaLanches.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MySqlConnector;

namespace CucaLanches.Infrastructure.Orders;

public class OrderRepository:IOrderRepository
{
    
    private readonly AppDbContext _dbContext;

    public OrderRepository(AppDbContext  dbContext)
    {
        _dbContext = dbContext;
    }
    
    private async Task<int> GetNextOrderNumberAsync()
    {
        var today = DateTime.UtcNow.Date;
        var connection = _dbContext.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        // Associa à transação ativa do EF Core, se houver
        var currentTransaction = _dbContext.Database.CurrentTransaction?.GetDbTransaction();

        await using var command = connection.CreateCommand();
        command.Transaction = currentTransaction;
    
        // O LAST_INSERT_ID() faz o MySQL guardar e retornar o valor gerado atomicamente
        command.CommandText = @"
        INSERT INTO OrderSequences (Date, LastNumber)
        VALUES (@date, LAST_INSERT_ID(1))
        ON DUPLICATE KEY UPDATE LastNumber = LAST_INSERT_ID(LastNumber + 1);

        SELECT LAST_INSERT_ID();";

        var dateParam = command.CreateParameter();
        dateParam.ParameterName = "@date";
        dateParam.Value = today;
        command.Parameters.Add(dateParam);

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }
    
    public async Task CreateAsync(Order order)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // Pega o próximo número com garantia atômica
            order.OrderNumber = await GetNextOrderNumberAsync();

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Order?> GetByIdAsync(int orderId)
    {
        return await _dbContext.Orders.AsNoTracking()
            .Include(o=>o.Address)
            .Include(o=>o.Client)
            .Include(o=>o.Address.Neighborhood)
            .Include(o=>o.Items)
            .ThenInclude(o=>o.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }
}