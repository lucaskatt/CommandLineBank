using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CommandLineBank
{
	class Bank
	{
		/// <summary>
		/// Name of the bank
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Dictionary mapping usernames to Users
		/// </summary>
		private Dictionary<string, User> _users;

		public Bank(string name)
		{
			Name = name;
			_users = new Dictionary<string, User>();
		}

		public bool DoesUserExist(string username)
		{
			return _users.ContainsKey(username);
		}

		public bool IsUsernameValid(string username)
		{
			//do some basic username validation; might need more work to handle non-english characters safely
			//checks if the string is not null, starts with a letter, and contains only letters and numbers
			return (!string.IsNullOrEmpty(username) && char.IsLetter(username[0]) && username.ToCharArray().All(x => char.IsLetterOrDigit(x)));
		}

		public bool CreateAccount(string username, string password, string firstname, string lastname)
		{
			//do additional validation in case something has happened in the time it took the user to finish creating an account
			if (!DoesUserExist(username) && IsUsernameValid(username))
			{
				User user = new User(username, HashPassword(password), firstname, lastname);
				_users.Add(username, user);
				return true;
			}

			return false;
		}

		public User Login(string username, string password)
		{
			User user;

			if (_users.ContainsKey(username))
			{
				user = _users[username];
			}
			else
			{
				return null; //no user with this username exists
			}

			string hashPass = HashPassword(password);
			if (user.Password == hashPass)
			{
				return user;
			}

			return null;
		}

		public bool Deposit(User user, string amount, out string error)
		{
			decimal result;
			error = "";

			if (!decimal.TryParse(amount, out result))
			{
				error = "This is not a valid value.";
				return false;
			}

			if (result <= 0)
			{
				error = "The value must be greater than 0.";
				return false;
			}

			//to simplify things, we are just not going to allow balances over the MaxValue
			if (user.Balance > decimal.MaxValue - result)
			{
				error = "There is not enough room in your account. Please create a new one.";
				return false;
			}

			user.Balance += result;
			user.TransactionHistory.Add(new Transaction(result, user.Balance, "Deposit"));

			return true;
		}

		public bool Withdraw(User user, string amount, out string error)
		{
			decimal result;
			error = "";

			if (!decimal.TryParse(amount, out result))
			{
				error = "This is not a valid value.";
				return false;
			}

			if (result <= 0)
			{
				error = "The value must be greater than 0.";
				return false;
			}

			if (user.Balance - result < 0)
			{
				error = "You do not have enough money in your account.";
				return false;
			}

			user.Balance -= result;
			user.TransactionHistory.Add(new Transaction(-result, user.Balance, "Withdraw"));

			return true;
		}

		private string HashPassword(string password)
		{
			//just using a simple hash without salt as an example
			StringBuilder builder = new StringBuilder();

			using (SHA256 sha = SHA256.Create())
			{
				byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
				foreach (byte b in bytes)
				{
					builder.Append(b.ToString("x2"));
				}
			}
			return builder.ToString();
		}
    }
}
