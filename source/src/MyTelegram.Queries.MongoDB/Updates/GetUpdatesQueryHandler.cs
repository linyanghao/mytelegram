﻿namespace MyTelegram.QueryHandlers.MongoDB.Updates;

public class GetUpdatesQueryHandler(IQueryOnlyReadModelStore<UpdatesReadModel> store) : IQueryHandler<GetUpdatesQuery, IReadOnlyCollection<IUpdatesReadModel>>
{
    public async Task<IReadOnlyCollection<IUpdatesReadModel>> ExecuteQueryAsync(GetUpdatesQuery query,
        CancellationToken cancellationToken)
    {
        Expression<Func<UpdatesReadModel, bool>> predicate = p => (p.OwnerPeerId == query.PeerId);
        predicate = 
            predicate
                //.WhereIf(query.Date > 0, p => p.Date > query.Date)
            .WhereIf(query.MinPts > 0, p => p.Pts > query.MinPts);

        return await store.FindAsync(predicate,
            0,
            query.Limit,
            cancellationToken: cancellationToken);
    }
}