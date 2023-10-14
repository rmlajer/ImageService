using Microsoft.AspNetCore.Mvc;
using ImageService.Models;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Npgsql;
using Dapper;
using Microsoft.AspNetCore.Cors;
using System.Text;

namespace ImageService.Controllers;



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
    public int PostImage(string imageString)
    {

        int imageId;
        Console.WriteLine("Post Image");
        Console.WriteLine("Byte Array: " + imageString);
        byte[] imageBytes = Encoding.ASCII.GetBytes(imageString);


        /*
        var sql = $"INSERT INTO public.Images" +
            $"(imageBytea)" +
            $"VALUES ({imageBytes}) RETURNING id";



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
        }*/

        using (var conn = new NpgsqlConnection(dbConnection.connectionString))
        {
            string sQL = "INSERT INTO public.images (imageBytea) VALUES (@Image) RETURNING id";
            using (var command = new NpgsqlCommand(sQL, conn))
            {
                NpgsqlParameter param = command.CreateParameter();
                param.ParameterName = "@Image";
                param.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bytea;
                param.Value = imageBytes;
                command.Parameters.Add(param);

                conn.Open();
                imageId = (int)command.ExecuteScalar();
            }
        }

        return imageId;
    }
}


