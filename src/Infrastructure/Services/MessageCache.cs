using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Services;

public sealed class MessageCache : IMessageCache
{
	private readonly IMapper _mapper;
	private readonly IMainDbContext _context;
	private readonly MemoryCache _cache;
	private SemaphoreSlim Semaphore = new(1);
	public MessageCache(IServiceScopeFactory scopeFactory)
	{
		var scope = scopeFactory.CreateScope();
		_mapper = scope.ServiceProvider.GetService<IMapper>()!;
		_context = scope.ServiceProvider.GetService<IMainDbContext>()!;
		_cache = new(new MemoryDistributedCacheOptions());
	}
	public async Task<MessageModel?> GetById(Guid messageId, CancellationToken ct)
	{
		if (_cache.TryGetValue<MessageModel>(messageId, out MessageModel cacheItem))
			return cacheItem;
		
		await Semaphore.WaitAsync(ct);
		try {
			var dbMsg = await _context.Messages.FirstOrDefaultAsync(x => x.Id == messageId, ct);
			if (dbMsg == null) return null;
			var model = _mapper.Map<MessageModel>(dbMsg);
			_cache.Set(messageId, model, new MemoryCacheEntryOptions {
				SlidingExpiration = TimeSpan.FromMinutes(10),
				Size = 0
			});
			return model;
		}
		finally { 
			Semaphore.Release();
		}
	}

	public void Set(MessageModel model)
	{
		_cache.Set(model.Id, model, new MemoryCacheEntryOptions
		{
			SlidingExpiration = TimeSpan.FromMinutes(10),
			Size = 0
		});
	}
}
