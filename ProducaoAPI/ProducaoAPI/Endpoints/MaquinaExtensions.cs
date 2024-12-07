//using Microsoft.EntityFrameworkCore;
//using ProducaoAPI.Data;
//using ProducaoAPI.Models;
//using ProducaoAPI.Requests;

//namespace ProducaoAPI.Endpoints
//{
//    public static class MaquinaExtensions
//    {
//        public static void MaquinaRoutes(this WebApplication app)
//        {
//            app.MapGet("/Maquinas", async (ProducaoContext context) =>
//            {
//                var maquinas = await context.Maquinas.ToListAsync();
//                return Results.Ok(maquinas);
//            });

//            app.MapGet("/Maquinas/{id}", async (ProducaoContext context, int id) =>
//            {
//                var maquina = await context.Maquinas.FirstOrDefaultAsync(m => m.Id == id);
                
//                if(maquina == null) return Results.NotFound();

//                return Results.Ok(maquina);
//            });

//            app.MapPost("/Maquinas", async (MaquinaRequest req, ProducaoContext context) =>
//            {
//                var maquina = new Maquina(req.nome, req.marca);
//                await context.AddAsync(maquina);
//                await context.SaveChangesAsync();
//            });

//            app.MapPut("/Maquinas/{id}", async (MaquinaRequest req, ProducaoContext context, int id) =>
//            {
//                var maquina = await context.Maquinas.FirstOrDefaultAsync(m => m.Id == id);

//                if (maquina == null) return Results.NotFound();

//                maquina.Nome = req.nome;
//                maquina.Marca = req.marca;
//                await context.SaveChangesAsync();
//                return Results.NoContent();

//            });

//            app.MapDelete("/Maquinas/{id}", async (ProducaoContext context, int id) =>
//            {
//                var maquina = context.Maquinas.FirstOrDefault(m => m.Id == id);

//                if (maquina == null) return Results.NotFound();

//                context.Maquinas.Remove(maquina);
//                await context.SaveChangesAsync();
//                return Results.NoContent();
//            });
//        }
//    }
//}
