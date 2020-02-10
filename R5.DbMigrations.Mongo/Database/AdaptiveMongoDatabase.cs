using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using MongoDB.Bson;
using MongoDB.Driver;
using static LanguageExt.Prelude;

namespace R5.DbMigrations.Mongo.Database
{
	// create a new one per migration
	// so we dont have to worry and manage session state
	public class AdaptiveMongoDbContext : IMongoDatabase
	{
		public IMongoClient Client => throw new NotImplementedException();
		public DatabaseNamespace DatabaseNamespace => throw new NotImplementedException();
		public MongoDatabaseSettings Settings => throw new NotImplementedException();

		private readonly IMongoDatabase _database;
		private readonly Option<IClientSessionHandle> _transactionSession;

		private Option<IClientSessionHandle> _inStartedTransactionState => _transactionSession
			.Some(s => s.IsInTransaction ? _transactionSession : Option<IClientSessionHandle>.None)
			.None(() => Option<IClientSessionHandle>.None);

		internal static readonly TransactionOptions _transactionOptions
			= new TransactionOptions(ReadConcern.Snapshot, writeConcern: WriteConcern.WMajority);

		private AdaptiveMongoDbContext(
			IMongoDatabase database,
			Option<IClientSessionHandle> transactionSession)
		{
			_database = database;
			_transactionSession = transactionSession;

			//transactionSession.IfSome(ts => ts.StartTransaction(_transactionOptions));
		}

		public void StartTransaction()
			=> _transactionSession.IfSome(
					ts => ts.StartTransaction(_transactionOptions));

		public Task CommitTransactionAsync()
			=> _inStartedTransactionState
				.Some(s => s.CommitTransactionAsync())
				.None(() => Task.CompletedTask);

		public Task AbortTransactionAsync()
			=> _inStartedTransactionState
				.Some(s => s.AbortTransactionAsync())
				.None(() => Task.CompletedTask);

		public static AdaptiveMongoDbContext WithTransaction(IMongoDatabase database)
		{
			if (database == null) throw new ArgumentNullException(nameof(database));
			var session = database.Client.StartSession();
			return new AdaptiveMongoDbContext(database, Optional(session));
		}

		public static AdaptiveMongoDbContext WithoutTransaction(IMongoDatabase database)
		{
			if (database == null) throw new ArgumentNullException(nameof(database));
			return new AdaptiveMongoDbContext(database, Option<IClientSessionHandle>.None);
		}

		public static AdaptiveMongoDbContext WithTransaction(Func<IMongoDatabase> getDatabase)
		{
			if (getDatabase == null) throw new ArgumentNullException(nameof(getDatabase));
			var database = getDatabase();
			var session = database.Client.StartSession();
			return new AdaptiveMongoDbContext(database, Optional(session));
		}

		public static AdaptiveMongoDbContext WithoutTransaction(Func<IMongoDatabase> getDatabase)
		{
			if (getDatabase == null) throw new ArgumentNullException(nameof(getDatabase));
			return new AdaptiveMongoDbContext(getDatabase(), Option<IClientSessionHandle>.None);
		}

		public static async Task<AdaptiveMongoDbContext> WithTransactionAsync(Func<Task<IMongoDatabase>> getDatabase)
		{
			if (getDatabase == null) throw new ArgumentNullException(nameof(getDatabase));
			var database = await getDatabase();
			var session = await database.Client.StartSessionAsync();
			return new AdaptiveMongoDbContext(database, Optional(session));
		}

		public static async Task<AdaptiveMongoDbContext> WithoutTransactionAsync(Func<Task<IMongoDatabase>> getDatabase)
		{
			if (getDatabase == null) throw new ArgumentNullException(nameof(getDatabase));
			var database = await getDatabase();
			return new AdaptiveMongoDbContext(database, Option<IClientSessionHandle>.None);
		}

		public IAsyncCursor<TResult> Aggregate<TResult>(PipelineDefinition<NoPipelineInput, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<TResult> Aggregate<TResult>(IClientSessionHandle session, PipelineDefinition<NoPipelineInput, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(PipelineDefinition<NoPipelineInput, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(IClientSessionHandle session, PipelineDefinition<NoPipelineInput, TResult> pipeline, AggregateOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void CreateCollection(string name, CreateCollectionOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void CreateCollection(IClientSessionHandle session, string name, CreateCollectionOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task CreateCollectionAsync(string name, CreateCollectionOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task CreateCollectionAsync(IClientSessionHandle session, string name, CreateCollectionOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void CreateView<TDocument, TResult>(string viewName, string viewOn, PipelineDefinition<TDocument, TResult> pipeline, CreateViewOptions<TDocument> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void CreateView<TDocument, TResult>(IClientSessionHandle session, string viewName, string viewOn, PipelineDefinition<TDocument, TResult> pipeline, CreateViewOptions<TDocument> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task CreateViewAsync<TDocument, TResult>(string viewName, string viewOn, PipelineDefinition<TDocument, TResult> pipeline, CreateViewOptions<TDocument> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task CreateViewAsync<TDocument, TResult>(IClientSessionHandle session, string viewName, string viewOn, PipelineDefinition<TDocument, TResult> pipeline, CreateViewOptions<TDocument> options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void DropCollection(string name, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void DropCollection(IClientSessionHandle session, string name, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task DropCollectionAsync(string name, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task DropCollectionAsync(IClientSessionHandle session, string name, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IMongoCollection<TDocument> GetCollection<TDocument>(string name, MongoCollectionSettings settings = null)
		{
			var collection = _database.GetCollection<TDocument>(name, settings);

			return _transactionSession
				.Some(s => AdaptiveMongoCollection<TDocument>.WithTransaction(collection, s))
				.None(() => AdaptiveMongoCollection<TDocument>.WithoutTransaction(collection));
		}

		public IAsyncCursor<string> ListCollectionNames(ListCollectionNamesOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<string> ListCollectionNames(IClientSessionHandle session, ListCollectionNamesOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<string>> ListCollectionNamesAsync(ListCollectionNamesOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<string>> ListCollectionNamesAsync(IClientSessionHandle session, ListCollectionNamesOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<BsonDocument> ListCollections(ListCollectionsOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IAsyncCursor<BsonDocument> ListCollections(IClientSessionHandle session, ListCollectionsOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<BsonDocument>> ListCollectionsAsync(ListCollectionsOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IAsyncCursor<BsonDocument>> ListCollectionsAsync(IClientSessionHandle session, ListCollectionsOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void RenameCollection(string oldName, string newName, RenameCollectionOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public void RenameCollection(IClientSessionHandle session, string oldName, string newName, RenameCollectionOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task RenameCollectionAsync(string oldName, string newName, RenameCollectionOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task RenameCollectionAsync(IClientSessionHandle session, string oldName, string newName, RenameCollectionOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public TResult RunCommand<TResult>(Command<TResult> command, ReadPreference readPreference = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public TResult RunCommand<TResult>(IClientSessionHandle session, Command<TResult> command, ReadPreference readPreference = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TResult> RunCommandAsync<TResult>(Command<TResult> command, ReadPreference readPreference = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TResult> RunCommandAsync<TResult>(IClientSessionHandle session, Command<TResult> command, ReadPreference readPreference = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IChangeStreamCursor<TResult> Watch<TResult>(PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IChangeStreamCursor<TResult> Watch<TResult>(IClientSessionHandle session, PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IChangeStreamCursor<TResult>> WatchAsync<TResult>(PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<IChangeStreamCursor<TResult>> WatchAsync<TResult>(IClientSessionHandle session, PipelineDefinition<ChangeStreamDocument<BsonDocument>, TResult> pipeline, ChangeStreamOptions options = null, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public IMongoDatabase WithReadConcern(ReadConcern readConcern)
		{
			throw new NotImplementedException();
		}

		public IMongoDatabase WithReadPreference(ReadPreference readPreference)
		{
			throw new NotImplementedException();
		}

		public IMongoDatabase WithWriteConcern(WriteConcern writeConcern)
		{
			throw new NotImplementedException();
		}
	}
}
