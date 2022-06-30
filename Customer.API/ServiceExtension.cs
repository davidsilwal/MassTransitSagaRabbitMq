using MassTransit;
using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Customer.API.Features;

namespace Customer.API
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(o =>
            {
                o.SetKebabCaseEndpointNameFormatter();

                //     o.AddSaga<CustomerSaga>().InMemoryRepository();

                o.AddSagaStateMachine<CreateCustomerStateMachine, CreateCustomerState>().InMemoryRepository();

                o.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });


            return services;
        }
    }
}