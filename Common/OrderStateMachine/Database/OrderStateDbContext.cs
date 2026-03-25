using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using OrderStateMachine.Database.Mapping;

namespace OrderStateMachine.Database
{
    public class OrderStateDbContext : SagaDbContext
    {

        public OrderStateDbContext(DbContextOptions<OrderStateDbContext> options) : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get
            {
                yield return new OrderStateMap();
            }
        }
    }
}
