using System.ComponentModel.DataAnnotations;

namespace DOMAIN.Base;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeleteAt { get; set; }
}