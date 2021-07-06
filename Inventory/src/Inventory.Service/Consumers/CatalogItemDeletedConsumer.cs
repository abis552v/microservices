using System;
using System.Threading.Tasks;
using Catalog.Contracts;
using Common.Service;
using Inventory.Service.Entities;
using MassTransit;

namespace Inventory.Service.Consumers
{
    //consuming message coming from contracts
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<CatalogItem> repository;

        public CatalogItemDeletedConsumer(IRepository<CatalogItem> repo)
        {
            this.repository = repo;
        }

        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;

            var item = await repository.GetAsync(message.ItemId);
            if (item == null)
            {
                return;
            }
            await repository.RemoveAsync(message.ItemId);
        }
    }
}