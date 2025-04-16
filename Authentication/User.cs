public class User
{
    public string username { get; set; }
    public string hashedPassword { get; set; }

    // Empty constructor needed for JSON deserialization
    public User() { }
    
    // Optional: Constructor for easy object creation
    public User(string username, string hashedPassword)
    {
        this.username = username;
        this.hashedPassword = hashedPassword;
    }

}

//This class is used when you load/save user data to/from users.json
//The empty constructor is needed so the JsonSerializer can create User objects from the file