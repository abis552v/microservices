using System;
using System.Threading.Tasks;
using Catalog.Contracts;
using Common.Service;
using Inventory.Service.Entities;
using MassTransit;

namespace Inventory.Service.Consumers
{
    //consuming message coming from contracts
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IRepository<CatalogItem> repository;

        public CatalogItemCreatedConsumer(IRepository<CatalogItem> repo)
        {
            this.repository = repo;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            var message = context.Message;

            var item = await repository.GetAsync(message.ItemId);
            if (item != null)
            {
                return;
            }

            item = new CatalogItem
            {
                Id = message.ItemId,
                Name = message.Name,
                Description = message.Description
            };

            await repository.CreateAsync(item);



        }
    }
}