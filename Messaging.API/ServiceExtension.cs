using MassTransit;


namespace Messaging.API
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddBus(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddMassTransit(o =>
            {
                o.SetKebabCaseEndpointNameFormatter();
              
                o.AddConsumers(typeof(CustomerCreatedMessagingConsumer).Assembly);

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
