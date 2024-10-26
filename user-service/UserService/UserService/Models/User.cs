namespace UserService.Models;

public class User(int id, string name, int credit)
{
    public int Id { get; set; } = id;

    public string Name { get; set; } = name;
    
    public int Credit { get; set; } = credit;
}