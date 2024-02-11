using HoneyRaesAPI.Models;
using HoneyRaesAPI.Models.DTOs;
List<Customer> customers = new List<Customer> 
{ 
    new Customer()
    {
        Id = 1,
        Name = "Val Freeman",
        Address = "128 React Router Dom Dr."
    },
    new Customer()
    {
        Id = 2,
        Name = "Steve Brownlee",
        Address = "12345 Shark Attack Way"
    },
    new Customer()
    {
        Id = 3,
        Name = "Jeffrey Lebowski",
        Address = "12 Golfer Ct"
    }
};
List<Employee> employees = new List<Employee> 
{ 
    new Employee()
    {
        Id = 1,
        Name = "Jack Handy",
        Specialty = "Changin tires"
    },
    new Employee()
    {
        Id = 2,
        Name = "Elvin Johnson",
        Specialty = "navigation systems"
    }
};
List<ServiceTicket> serviceTickets = new List<ServiceTicket> 
{ 
    new ServiceTicket()
    {
        Id = 1,
        CustomerId = 1,
        EmployeeId = 1,
        Description = "Dropped in toilet",
        Emergency = true,
        DateCompleted = new DateTime(2023, 12, 30)

    },
    new ServiceTicket()
    {
        Id = 2,
        CustomerId = 2,
        EmployeeId = 1,
        Description = "cheese whiz in charging port",
        Emergency = false,
        DateCompleted = new DateTime(2024, 1, 2)
    },
    new ServiceTicket()
    {
        Id = 3,
        CustomerId = 3,
        EmployeeId = 2,
        Description = "cracked screen",
        Emergency = false
    },
    new ServiceTicket()
    {
        Id = 4,
        CustomerId = 1,
        Description = "toilet again",
        Emergency = false
    },
    new ServiceTicket()
    {
        Id = 5,
        CustomerId = 2,
        Description = "more cheese whiz, now in headphone jack",
        Emergency = true
    }
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/employees", () => 
{
    return employees.Select(e => new EmployeeDTO
    {
        Id = e.Id,
        Name = e.Name,
        Specialty = e.Specialty
    });
}
);

app.MapGet("/employees/{id}", (int id) =>
{
    Employee employee = employees.FirstOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }
    List<ServiceTicket> tickets = serviceTickets.Where(st => st.EmployeeId == id).ToList();
    return Results.Ok(new EmployeeDTO
    {
        Id = employee.Id,
        Name = employee.Name,
        Specialty = employee.Specialty,
        ServiceTickets = tickets.Select(t => new ServiceTicketDTO
        {
            Id = t.Id,
            CustomerId = t.CustomerId,
            EmployeeId = t.EmployeeId,
            Description = t.Description,
            Emergency = t.Emergency,
            DateCompleted = t.DateCompleted
        }).ToList()
    });
});

app.MapGet("/customers", () => 
{
    return customers.Select(c => new CustomerDTO
    {
        Id = c.Id,
        Name = c.Name,
        Address = c.Address
    });
}
);

app.MapGet("/customers/{id}", (int id) =>
{
    Customer customer = customers.FirstOrDefault(c => c.Id == id);
    if (customer == null)
    {
        return Results.NotFound();
    }
    List<ServiceTicket> tickets = serviceTickets.Where(st => st.CustomerId == id).ToList();
    return Results.Ok(new CustomerDTO
    {
        Id = customer.Id,
        Name = customer.Name,
        Address = customer.Address,
        ServiceTickets = tickets.Select(t => new ServiceTicketDTO
        {
            Id = t.Id,
            CustomerId = t.CustomerId,
            EmployeeId = t.EmployeeId,
            Description = t.Description,
            Emergency = t.Emergency,
            DateCompleted = t.DateCompleted
        }).ToList()
    });
}
);

app.MapGet("/servicetickets", () => 
{
    return serviceTickets.Select(t => new ServiceTicketDTO
    {
        Id = t.Id,
        CustomerId = t.CustomerId,
        EmployeeId = t.EmployeeId,
        Description = t.Description,
        Emergency = t.Emergency,
        DateCompleted = t.DateCompleted
    });
}
);

app.MapGet("/servicetickets/{id}", (int id) =>
{
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);
    
    if (serviceTicket == null)
    {
        return Results.NotFound();
    }
    
    Employee employee = employees.FirstOrDefault(e => e.Id == serviceTicket.EmployeeId);
    Customer customer = customers.FirstOrDefault(c => c.Id == serviceTicket.CustomerId);
    
    return Results.Ok(new ServiceTicketDTO
    {
        Id = serviceTicket.Id,
        CustomerId = serviceTicket.CustomerId,
        Customer = customer == null ? null : new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        },
        EmployeeId = serviceTicket.EmployeeId,
        Employee = employee == null ? null : new EmployeeDTO
        {
            Id = employee.Id,
            Name = employee.Name,
            Specialty = employee.Specialty
        },
        Description = serviceTicket.Description,
        Emergency = serviceTicket.Emergency,
        DateCompleted = serviceTicket.DateCompleted
    });
});

app.MapPost("/servicetickets", (ServiceTicket serviceTicket) =>
{

    // Get the customer data to check that the customerid for the service ticket is valid
    Customer customer = customers.FirstOrDefault(c => c.Id == serviceTicket.CustomerId);

    // if the client did not provide a valid customer id, this is a bad request
    if (customer == null)
    {
        return Results.BadRequest();
    }

    // creates a new id (SQL will do this for us like JSON Server did!)
    serviceTicket.Id = serviceTickets.Max(st => st.Id) + 1;
    serviceTickets.Add(serviceTicket);

    // Created returns a 201 status code with a link in the headers to where the new resource can be accessed
    return Results.Created($"/servicetickets/{serviceTicket.Id}", new ServiceTicketDTO
    {
        Id = serviceTicket.Id,
        CustomerId = serviceTicket.CustomerId,
        Customer = new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        },
        Description = serviceTicket.Description,
        Emergency = serviceTicket.Emergency
    });

});

app.MapDelete("/servicetickets/{id}", (int id) => 
{
     ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);
    
    if (serviceTicket == null)
    {
        return Results.NotFound();
    }
    serviceTickets.Remove(serviceTicket);
    return Results.NoContent();
});

app.Run();
