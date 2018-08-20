using System;
using System.Collections.Generic;
using System.Text;

namespace CommandLineBank
{
    class ConsoleView
    {
		private Bank _bank;

		private enum Action
		{
			Deposit,
			Withdraw
		}

		/// <summary>
		/// Start banking
		/// </summary>
		public void Start()
		{
			_bank = new Bank("Command Line Bank");
			DisplayStartup();
		}

		/// <summary>
		/// Display the bank startup screen
		/// </summary>
		private void DisplayStartup()
		{
			DisplayStartup(false);
		}
		/// <summary>
		/// Display the bank startup screen
		/// </summary>
		/// <param name="previouslyInvalid">whether the user previously encountered an error on this screen</param>
		private void DisplayStartup(bool previouslyInvalid)
		{
			Console.Clear();
			if (previouslyInvalid)
			{
				Console.WriteLine("Invalid entry. Please try again.");
			}
			else
			{
				Console.WriteLine("Welcome to the " + _bank.Name + "!");
			}
			Console.WriteLine("Select an option below to get started.");
			Console.WriteLine();
			Console.WriteLine("  1.  Create an account");
			Console.WriteLine("  2.  Login");
			Console.WriteLine("  3.  Exit");
			ConsoleKeyInfo cki = Console.ReadKey();

			switch (cki.KeyChar)
			{
				case '1':
					DisplayCreateAccount();
					break;
				case '2':
					DisplayLogin();
					break;
				case '3':
					Console.WriteLine("Thank you for visiting!");
					//exiting, don't call another display function
					break;
				default:
					//invalid option, start over
					DisplayStartup(true);
					break;
			}
		}

		/// <summary>
		/// Display the account creation screen
		/// </summary>
		private void DisplayCreateAccount()
		{
			DisplayCreateAccount(false);
		}
		/// <summary>
		/// Display the account creation screen
		/// </summary>
		/// <param name="previouslyInvalid">whether the user previously encountered an error on this screen</param>
		private void DisplayCreateAccount(bool previouslyInvalid)
		{
			string line = "";
			string username;
			string password;
			string firstname;
			string lastname;

			Console.Clear();
			if (previouslyInvalid)
			{
				Console.WriteLine("Could not create an account. Please try again.");
			}

			else
			{
				Console.WriteLine("Follow the prompts to create an account.");
			}

			//prompt for username
			while (string.IsNullOrEmpty(line))
			{
				Console.Write("Username: ");
				line = Console.ReadLine().Trim().ToLower();

				if (string.IsNullOrEmpty(line))
				{
					DisplayStartup();
					return;
				}

				else if (_bank.DoesUserExist(line))
				{
					Console.WriteLine("This username is already taken.");
					line = "";
				}

				else if (!_bank.IsUsernameValid(line))
				{
					Console.WriteLine("Usernames must begin with a letter and contain only letters and numbers");
					line = "";
				}
			}
			username = line;
			line = "";

			//prompt for password
			while (string.IsNullOrEmpty(line))
			{
				Console.Write("Password: ");
				line = ReadPassword();

				//if no password is entered, restart the screen so the user has a chance to quit
				if (string.IsNullOrEmpty(line))
				{
					DisplayCreateAccount(true);
					return;
				}

				//validate password; could add more stuff here like multiple cases, numbers, letters, and special characters
				if (line.Length < 8)
				{
					Console.WriteLine("Your password must be at least 8 characters.");
					line = "";
					continue;
				}

				Console.WriteLine();
				Console.Write("Confirm password: ");
				if (line != ReadPassword())
				{
					Console.WriteLine("Your passwords do not match.");
					line = "";
				}
			}
			password = line;
			line = "";

			//prompt for names
			firstname = PromptForName("first");
			lastname = PromptForName("last");

			if (!_bank.CreateAccount(username, password, firstname, lastname))
			{
				DisplayCreateAccount(true);
				return;
			}

			DisplayLogin();
		}

		/// <summary>
		/// Display the login screen
		/// </summary>
		private void DisplayLogin()
		{
			DisplayLogin(false);
		}
		/// <summary>
		/// Display the login screen
		/// </summary>
		/// <param name="previouslyInvalid">whether the user previously encountered an error on this screen</param>
		private void DisplayLogin(bool previouslyInvalid)
		{
			string username;
			string password;

			Console.Clear();
			if (previouslyInvalid)
			{
				Console.WriteLine("Incorrect username or password. Please try again.");
			}

			else
			{
				Console.WriteLine("Enter your username and password to log in.");
			}

			//prompt for username
			Console.Write("Username: ");
			username = Console.ReadLine().Trim().ToLower();

			if (string.IsNullOrEmpty(username))
			{
				DisplayStartup();
				return;
			}

			//prompt for password
			Console.Write("Enter your password: ");
			password = ReadPassword();

			//if no password is entered, restart the screen so the user has a chance to quit
			if (string.IsNullOrEmpty(password))
			{
				DisplayLogin(true);
				return;
			}

			User user = _bank.Login(username, password);
			if (user == null)
			{
				//login failed for some reason
				DisplayLogin(true);
				return;
			}
			DisplayAccount(user);

		}

		/// <summary>
		/// Display the user's account
		/// </summary>
		/// <param name="user">logged in user</param>
		private void DisplayAccount(User user)
		{
			DisplayAccount(user, false);
		}
		/// <summary>
		/// Display the user's account
		/// </summary>
		/// <param name="user">logged in user</param>
		/// <param name="previouslyInvalid">whether the user previously encountered an error on this screen</param>
		private void DisplayAccount(User user, bool previouslyInvalid)
		{
			Console.Clear();
			if (previouslyInvalid)
			{
				Console.WriteLine("Invalid entry. Please try again.");
			}
			else
			{
				Console.WriteLine("Hello " + user.FirstName + " " + user.LastName + ".");
			}
			Console.WriteLine("Your account balance is $" + user.Balance + "."); //would probably be better to format this string to properly display dollars
			Console.WriteLine("Select an option below.");
			Console.WriteLine();
			Console.WriteLine("  1.  Deposit");
			Console.WriteLine("  2.  Withdraw");
			Console.WriteLine("  3.  View transactions");
			Console.WriteLine("  4.  Log out");
			ConsoleKeyInfo cki = Console.ReadKey();

			switch (cki.KeyChar)
			{
				case '1':
					DisplayDeposit(user);
					break;
				case '2':
					DisplayWithdraw(user);
					break;
				case '3':
					DisplayTransactions(user);
					break;
				case '4':
					DisplayStartup();
					break;
				default:
					//invalid option, start over
					DisplayAccount(user, true);
					break;
			}
		}

		/// <summary>
		/// Display the prompt for making a withdrawal
		/// </summary>
		/// <param name="user">logged in user</param>
		private void DisplayWithdraw(User user)
		{
			DisplayAction(user, Action.Withdraw);
		}	
		/// <summary>
		/// Display the prompt for making a deposit
		/// </summary>
		/// <param name="user">logged in user</param>
		private void DisplayDeposit(User user)
		{
			DisplayAction(user, Action.Deposit);
		}
		/// <summary>
		/// Display the prompt for withdrawing or depositing
		/// </summary>
		/// <param name="user">logged in user</param>
		/// <param name="action">action to take</param>
		private void DisplayAction(User user, Action action)
		{
			string line = "";
			string error = "";
			string actionStr = action == Action.Deposit ? "deposit" : "withdraw";
			bool success = false;
			Console.WriteLine();

			while (string.IsNullOrEmpty(line))
			{
				Console.WriteLine("Enter an amount to " + actionStr + ": ");
				line = Console.ReadLine();

				//take the appropriate action
				switch (action)
				{
					case Action.Deposit:
						success = _bank.Deposit(user, line, out error);
						break;
					case Action.Withdraw:
						success = _bank.Withdraw(user, line, out error);
						break;
					default:
						success = false;
						break;
				}


				//depositing failed
				if (success == false)
				{
					if (!string.IsNullOrEmpty(error))
					{
						Console.WriteLine(error);
						line = "";
					}	
					else //failed for unknown reason
					{
						DisplayAccount(user, true);
						return;
					}
				}
			}
			Console.WriteLine("Your new balance is $" + user.Balance + ". Press any key to continue.");
			Console.ReadKey();
			DisplayAccount(user);
		}

		/// <summary>
		/// Display a list of transactions the user has taken
		/// </summary>
		/// <param name="user">logged in user</param>
		private void DisplayTransactions(User user)
		{
			Console.Clear();

			if (user.TransactionHistory.Count == 0)
			{
				Console.WriteLine("You have not made any transactions.");
			}

			else
			{
				//could format this better, but UI isn't very important for this project
				Console.WriteLine("Date, Description, Amount, Balance");
				foreach (Transaction tx in user.TransactionHistory)
				{
					//could use string builder here, but with joining 7 strings it's about as efficient to do this
					Console.WriteLine(tx.Date.ToString("MM/dd/yyyy") + ", " + tx.Description + ", " + tx.Amount + ", " + tx.Balance);
				}
			}

			Console.WriteLine();
			Console.WriteLine("Press any key to continue.");
			Console.ReadKey();
			DisplayAccount(user);
		}

		/// <summary>
		/// Read a password from the console
		/// </summary>
		/// <returns>password</returns>
		private string ReadPassword()
		{
			//since this is a banking application used by many types of users, we are going to echo back "*" for each character typed
			//even though most console applications don't echo anything to prevent the password length from being found
			string password;
			StringBuilder builder = new StringBuilder();
			ConsoleKeyInfo cki;

			//read each key from the user
			do
			{
				cki = Console.ReadKey(true);
				if (cki.Key != ConsoleKey.Backspace && cki.Key != ConsoleKey.Enter && cki.Key != ConsoleKey.Escape)
				{
					builder.Append(cki.KeyChar);
					Console.Write("*"); //print out asterix for each character added
				}
				else if (cki.Key == ConsoleKey.Backspace && builder.Length > 0)
				{
					builder.Remove(builder.Length - 1, 1);
					Console.Write("\b \b"); //backspace removes last asterix and last character in builder
				}
			}
			while (cki.Key != ConsoleKey.Enter);

			password = builder.ToString();
			Console.WriteLine();

			return password;
		}

		/// <summary>
		/// Prompts the user for a name
		/// </summary>
		/// <param name="type">descriptor of the name, such as "first", "middle", or "last"</param>
		/// <returns>name</returns>
		private string PromptForName(string type)
		{
			string line = "";
			type = type.ToLower();

			//prompt for username
			while (string.IsNullOrEmpty(line))
			{
				Console.Write("Enter your " + type + " name: ");
				line = Console.ReadLine().Trim();

				//we don't want to discriminate, all characters are allowed in your name
				//if actually storing in a database would need to strip injection characters eventually
			}
			return line;
		}
    }
}
