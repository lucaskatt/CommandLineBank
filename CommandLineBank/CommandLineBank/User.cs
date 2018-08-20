using System;
using System.Collections.Generic;
using System.Text;

namespace CommandLineBank
{
    class User
    {
		/// <summary>
		/// First name of the user
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Last name of the user
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Balance of the user's account
		/// </summary>
		public decimal Balance { get; set; } //using decimal for more precision in financial calculations

		/// <summary>
		/// Unique username
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Hashed password
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// List of past transactions in the order they occurred
		/// </summary>
		public List<Transaction> TransactionHistory { get; set; } //transactions should always be added in order, and using a list allows us to search a subset based on a date range if we need to

		/// <summary>
		/// Creates a new user with a balance of 0
		/// </summary>
		/// <param name="username">Login username</param>
		/// <param name="password">Login password</param>
		/// <param name="firstname">First name</param>
		/// <param name="lastname">Last name</param>
		public User(string username, string password, string firstname, string lastname)
		{
			Username = username;
			Password = password;
			FirstName = firstname;
			LastName = lastname;
			Balance = 0;
			TransactionHistory = new List<Transaction>();
		}
	}
}
