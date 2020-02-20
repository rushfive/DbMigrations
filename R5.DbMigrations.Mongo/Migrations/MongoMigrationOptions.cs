namespace R5.DbMigrations.Mongo.Migrations
{
	public class MongoMigrationOptions
	{
		public MongoTransactionOption TransactionOption { get; set; } = MongoTransactionOption.UseAndRetryWithoutTransactionOnFail;

		public bool ShouldRetryWithoutTransaction => TransactionOption == MongoTransactionOption.UseAndRetryWithoutTransactionOnFail;

		public bool UseTransaction => TransactionOption == MongoTransactionOption.Use
			|| TransactionOption == MongoTransactionOption.UseAndRetryWithoutTransactionOnFail;
	}

	public enum MongoTransactionOption
	{
		None,
		Use,
		UseAndRetryWithoutTransactionOnFail
	}
}
