namespace Coach.Application.Abstractions.Identity;

public interface IUser
{
    string? Email { get; set; }

    string? UserName { get; set; }

    Guid Id { get; set; }

    Guid AthleteId { get; set; }
}
