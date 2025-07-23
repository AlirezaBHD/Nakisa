using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Nakisa.Domain.Enums;

namespace Nakisa.Domain.Entities;

[DisplayName("دسته بندی")]
public class Category : BaseEntity
{
    [MaxLength(50)]
    [Display(Name = "نام")]
    public required string Name { get; set; }
    
    [Display(Name = "ترتیب")]
    public int Order { get; set; }

    [Display(Name = "نوع دسته بندی")] 
    public CategoryType CategoryType { get; set; } = CategoryType.Genre;
    public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}