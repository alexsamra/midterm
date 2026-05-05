using dal;
using model;
using frontend.Model;

// Initialize DAL and services. Connection string mirrors api appsettings.
var connectionString = "Server=host.docker.internal;Port=3333;Database=midterm;User=root;Password=a;";

var userDal = new UserDal(connectionString);
var userService = new UserService(userDal);
var client = new BankClient(userService);

TerminalApp.Run(client);

