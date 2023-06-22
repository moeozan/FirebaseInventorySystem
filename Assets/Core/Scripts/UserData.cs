using Firebase.Firestore;

[FirestoreData]
public class UserData
{
    [FirestoreProperty] public string Email { get; set; }
    [FirestoreProperty] public string Password { get; set; }
    [FirestoreProperty] public Inventory UserInventory { get; set; } = new Inventory();

    public UserData() { }

    public UserData(string email, string password)
    {
        Email = email;
        Password = password;
        UserInventory = new Inventory();
    }
}
