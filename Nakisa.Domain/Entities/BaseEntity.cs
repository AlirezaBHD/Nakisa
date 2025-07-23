using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nakisa.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Display(Name = "شناسه")]
    public int Id { get; set; }
    
    [Display(Name = "زمان ایجاد")]
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    
    [Display(Name = "زمان آخرین تغییر")]
    public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;
}