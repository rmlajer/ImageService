using Microsoft.AspNetCore.Mvc;
using ImageService.Models;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Npgsql;
using Dapper;
using Microsoft.AspNetCore.Cors;

namespace GetImageService.Controllers;



[ApiController]
[Route("[controller]")]

public class ImagesController : ControllerBase
{

    private readonly DbConnection dbConnection = new DbConnection();
    private readonly ILogger<ImagesController> _logger;

    public ImagesController(ILogger<ImagesController> logger, IConfiguration configuration)
    {
        _logger = logger;
    }

   

    [EnableCors]
    [HttpGet("{id:int}")]
    public ImageDTO GetImageById(int id)
    {
        Console.WriteLine("Get Image by ID");
        var SQL = $"SELECT * FROM public.images WHERE id={id}";
        ImageDTO returnImageDTO = new();

        using (var connection = new NpgsqlConnection(dbConnection.connectionString))
        {
            
            returnImageDTO = connection.Query<ImageDTO>(SQL).First();

        }
        return returnImageDTO;

    }
    
    [EnableCors]
    [HttpPost()]
    public int PostImage(byte[] imageBytea)
    {


        Console.WriteLine("Post Image");
        Console.WriteLine("Byte Array: " + imageBytea);


        var sql = $"INSERT INTO public.Images" +
            $"(imageBytea)" +
            $"VALUES ('{imageBytea}') RETURNING id";


        Console.WriteLine("sql: " + sql);

        using (var connection = new NpgsqlConnection(dbConnection.connectionString))
        {
            try
            {
                return connection.Execute(sql);
               
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't add the Image to the list: " + e.Message);
                throw new InvalidDataException();

            }
        }
    }
}


