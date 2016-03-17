using Microsoft.AspNet.WebHooks;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CustomWebHooksSender.WebHooks
{
    /// <summary>
    /// Use an <see cref="IWebHookFilterProvider"/> implementation to describe the events that users can
    /// subscribe to. A wildcard filter is always registered meaning that users can register for
    /// "all events". It is possible to have 0, 1, or more <see cref="IWebHookFilterProvider"/>
    /// implementations.
    /// </summary>
    public class CustomFilterProvider : IWebHookFilterProvider
    {
        private readonly Collection<WebHookFilter> _filters = new Collection<WebHookFilter>
        {
            new WebHookFilter {Name = "CriarCliente", Description = "Quando um novo cliete e criado"},
            new WebHookFilter {Name = "EditarCliente", Description = "Quando um cliente é editado"},
            new WebHookFilter {Name = "ExcluirCliente", Description = "Quando um cliente é removido"}
        };

        public Task<Collection<WebHookFilter>> GetFiltersAsync()
        {
            return Task.FromResult(this._filters);
        }
    }
}