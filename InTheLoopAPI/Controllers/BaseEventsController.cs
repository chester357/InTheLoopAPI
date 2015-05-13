using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using InTheLoopAPI.Models;

namespace InTheLoopAPI.Controllers
{
    public class BaseEventsController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/BaseEvents
        public IQueryable<BaseEvent> GetBaseEvents()
        {
            return db.BaseEvents;
        }

        // GET: api/BaseEvents/5
        [ResponseType(typeof(BaseEvent))]
        public async Task<IHttpActionResult> GetBaseEvent(int id)
        {
            BaseEvent baseEvent = await db.BaseEvents.FindAsync(id);
            if (baseEvent == null)
            {
                return NotFound();
            }

            return Ok(baseEvent);
        }

        // PUT: api/BaseEvents/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBaseEvent(int id, BaseEvent baseEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != baseEvent.Id)
            {
                return BadRequest();
            }

            db.Entry(baseEvent).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BaseEventExists(id))
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

        // POST: api/BaseEvents
        [ResponseType(typeof(BaseEvent))]
        public async Task<IHttpActionResult> PostBaseEvent(BaseEvent baseEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BaseEvents.Add(baseEvent);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = baseEvent.Id }, baseEvent);
        }

        // DELETE: api/BaseEvents/5
        [ResponseType(typeof(BaseEvent))]
        public async Task<IHttpActionResult> DeleteBaseEvent(int id)
        {
            BaseEvent baseEvent = await db.BaseEvents.FindAsync(id);
            if (baseEvent == null)
            {
                return NotFound();
            }

            db.BaseEvents.Remove(baseEvent);
            await db.SaveChangesAsync();

            return Ok(baseEvent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BaseEventExists(int id)
        {
            return db.BaseEvents.Count(e => e.Id == id) > 0;
        }
    }
}