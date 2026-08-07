using Google.Cloud.Firestore;

namespace linux_desktop.Models;

[FirestoreData]
public class User
{
    public string? Uid { get; set; }
    
    [FirestoreProperty("email")]
    public string? Email { get; set; }
    [FirestoreProperty("FirstnameLastname")]
    public string? Name { get; set; }
    [FirestoreProperty("admin")]
    public bool? WorkPosition { get; set; }
    
}