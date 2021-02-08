using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ShowBisaoAPI.Models;
using System.Web.Http.Cors;

namespace ShowBisaoAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PerguntasController : ApiController
    {
        private ShowBisaoEntities db = new ShowBisaoEntities();

        // GET: api/Perguntas
        public List<PerguntaVM> GetPerguntas()
        {
            IQueryable<Pergunta> perguntas =  db.Perguntas;
            List<PerguntaVM> listPerguntas = new List<PerguntaVM>();
            List<RespostaVM> listRespostas = new List<RespostaVM>();

            foreach (var item in perguntas)
            {
                PerguntaVM pergunta = new PerguntaVM()
                {
                    IdPergunta = item.IdPergunta,
                    Rodada = item.Rodada,
                    Pergunta = item.Pergunta1,
                    Premio = item.Premio                   

                };

                pergunta.Respostas = getRespostas(item.Respostas, pergunta.IdPergunta, item.Rodada);
                listPerguntas.Add(pergunta);
            }          

            return listPerguntas;
        }

        private List<RespostaVM> getRespostas(ICollection<Resposta> respostas, int idPergunta, int idRodada)
        {
            List<RespostaVM> listaRespostas = new List<RespostaVM>();
            var colRespostas = respostas.Where(r => r.IdPergunta == idPergunta && r.Rodada == idRodada);

            foreach (var item in colRespostas)
            {
                RespostaVM resp = new RespostaVM()
                {
                    IdAlternativa = item.IdAlternativa,
                    Alternativa = item.Alternativa,
                    EhCorreta = item.EhCorreta,
                    IdPergunta = item.IdPergunta,
                    Rodada = item.Rodada
                };
                listaRespostas.Add(resp);
            }

            return listaRespostas;
        }

        // GET: api/Perguntas/5
        [ResponseType(typeof(Pergunta))]
        public IHttpActionResult GetPergunta(int id, [FromUri]int idRodada)
        {
            Pergunta pergunta = db.Perguntas.Find(id, idRodada);
            List<RespostaVM> listRespostas = new List<RespostaVM>();

            if (pergunta == null)
            {
                return NotFound();
            }

            PerguntaVM perguntavm = new PerguntaVM()
            {
                IdPergunta = pergunta.IdPergunta,
                Rodada = pergunta.Rodada,
                Pergunta = pergunta.Pergunta1,
                Premio = pergunta.Premio
            };

            perguntavm.Respostas = getRespostas(pergunta.Respostas, id, idRodada);
            
            return Ok(perguntavm);
        }

        // PUT: api/Perguntas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPergunta(int id, Pergunta pergunta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pergunta.IdPergunta)
            {
                return BadRequest();
            }

            db.Entry(pergunta).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerguntaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Perguntas
        [ResponseType(typeof(Pergunta))]
        public IHttpActionResult PostPergunta(Pergunta pergunta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Perguntas.Add(pergunta);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PerguntaExists(pergunta.IdPergunta))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pergunta.IdPergunta }, pergunta);
        }

        // DELETE: api/Perguntas/5
        [ResponseType(typeof(Pergunta))]
        public IHttpActionResult DeletePergunta(int id)
        {
            Pergunta pergunta = db.Perguntas.Find(id);
            if (pergunta == null)
            {
                return NotFound();
            }

            db.Perguntas.Remove(pergunta);
            db.SaveChanges();

            return Ok(pergunta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PerguntaExists(int id)
        {
            return db.Perguntas.Count(e => e.IdPergunta == id) > 0;
        }
    }
}