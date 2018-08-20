using System;

namespace CommandLineBank
{
	class Transaction 
    {
		/// <summary>
		/// Description of the transaction
		/// </summary>
		public string Description { get; set; } //could describe where the transaction is coming from, but we will just use it for "deposit" and "withdraw"

		/// <summary>
		/// Date/time that the transaction occurred
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Amount used in the transaction; positive for a deposit, negative for a withdrawal
		/// </summary>
		public decimal Amount { get; set; }

		/// <summary>
		/// Balance of the account after the transaction has been applied
		/// </summary>
		public decimal Balance { get; set; }

		/// <summary>
		/// Create a transaction at the current instant
		/// </summary>
		/// <param name="amount">amount used in the transaction; positive for a deposit, negative for withdrawal</param>
		/// <param name="balance">balance of the account after the transaction has been applied</param>
		/// <param name="description">description of the transaction</param>
		public Transaction(decimal amount, decimal balance, string description)
		{
			Amount = amount;
			Balance = balance;
			Description = description;
			Date = DateTime.Now; //should probably handle everything in UTC if this were a real bank
		}
    }
}
