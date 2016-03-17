using CustomWebHooksSender.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CustomWebHooksSender.Controllers
{
    [RoutePrefix("api/Cliente")]
    public class ClientesController : ApiController
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ClientesController()
        {
            _applicationDbContext = new ApplicationDbContext();
        }

        [Route("CriarCliente")]
        [HttpPost]
        public Task<HttpResponseMessage> CriarCliente(ClienteModel model)
        {
            HttpResponseMessage response;
            try
            {
                _applicationDbContext.ClienteModel.Add(model);
                _applicationDbContext.SaveChanges();
                response = Request.CreateResponse(HttpStatusCode.OK, _applicationDbContext.Entry(model).Entity);
                Notificar("CriarCliente", model);
            }
            catch (Exception)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("ConsultaClientes")]
        [HttpGet]
        [Authorize]
        public Task<HttpResponseMessage> PesquisarCliente()
        {
            HttpResponseMessage response;
            try
            {
                var cliente = _applicationDbContext.ClienteModel.ToList();
                response = Request.CreateResponse(HttpStatusCode.OK, cliente);
            }
            catch (Exception)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("EditarCliente")]
        [HttpPut]
        public Task<HttpResponseMessage> EditarCliente(ClienteModel model)
        {
            HttpResponseMessage response;

            try
            {
                _applicationDbContext.Entry(model).State = EntityState.Modified;
                _applicationDbContext.SaveChanges();
                response = Request.CreateResponse(HttpStatusCode.OK, _applicationDbContext.Entry(model).Entity);
                Notificar("EditarCliente", _applicationDbContext.Entry(model).Entity);
            }
            catch (Exception)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        [Route("ExcluirCliente")]
        [HttpDelete]
        public Task<HttpResponseMessage> ExcluirCliente([FromBody]JObject jObject)
        {
            var id = jObject["Id"].ToObject<int>();
            HttpResponseMessage response;
            try
            {
                var cliente = _applicationDbContext.ClienteModel.FirstOrDefault(x => x.Id.Equals(id));
                _applicationDbContext.ClienteModel.Remove(cliente);
                _applicationDbContext.SaveChanges();
                response = Request.CreateResponse(HttpStatusCode.OK);
                Notificar("ExcluirCliente", cliente);
            }
            catch (Exception)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        private async void Notificar(string evento, object objeto)
        {
            await this.NotifyAsync(evento, objeto);
        }
    }
}