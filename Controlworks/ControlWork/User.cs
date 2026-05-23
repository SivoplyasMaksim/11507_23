public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int? Age { get; set; }
    public DateTime CreatedAt { get; set; }

    public override string ToString() =>
        $"User(Id={Id}, Name={Name}, Email={Email}, Age={Age}, CreatedAt={CreatedAt})";
}
