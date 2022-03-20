using System;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using Microsoft.Xrm.Sdk.Query;

namespace TaskClass
{
    public class TaskClass : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory serviceFactory =
                    (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                Entity rent = (Entity)context.InputParameters["Target"];

                try
                {
                    Guid customer;

                    if (rent.Attributes.Contains("mr_customer") && rent.Attributes.Contains("statuscode"))
                    {
                        customer = rent.GetAttributeValue<EntityReference>("mr_customer").Id;
                        QueryExpression query = new QueryExpression
                        {
                            EntityName = "mr_rent",
                            ColumnSet = new ColumnSet(new string[] { "mr_customer", "statuscode" }),
                            Criteria =
                            {
                                FilterOperator = LogicalOperator.And,
                                Conditions =
                                {
                                    new ConditionExpression ("statuscode", ConditionOperator.Equal, 315890001),
                                    new ConditionExpression ("mr_customer", ConditionOperator.Equal, customer)
                                }
                            }
                        };
                        EntityCollection collection = service.RetrieveMultiple(query);
                        if (collection.Entities.Count > 10)
                        {
                            throw new InvalidPluginExecutionException("More than 10 rents in status Renting per one owner");
                        }

                    }
                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in MyPlug-in.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("MyPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}

