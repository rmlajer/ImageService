namespace ImageService.Models;
using System.ComponentModel.DataAnnotations;
public record ImageDTO
{

    public ImageDTO()
    {

    }
    public ImageDTO(byte [] imageBytea)
    {

        this.ImageBytea = imageBytea;

    }

    public ImageDTO(int id, byte [] imageBytea)
    {

        this.Id = id;
        this.ImageBytea = imageBytea;

    }
  


    public int Id { get; set; }

    [Required]
    public byte[] ImageBytea { get; set; }
 
}