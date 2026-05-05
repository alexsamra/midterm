using Microsoft.Extensions.Configuration;
using dal;
using model;
using frontend.Model;

// Load configuration from appsettings.json
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

var connectionString = config.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json");

// Initialize dependency injection
IUserRepository userRepository = new UserDal(connectionString);
IAccountService accountService = new UserService(userRepository);
var bankClient = new BankClient(accountService);

// Run terminal application
TerminalApp.Run(bankClient);

