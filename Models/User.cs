using Google.Cloud.Firestore;

namespace linux_desktop.Models;

[FirestoreData]
public class User
{
    public string? Uid { get; set; }
    public string? IdToken { get; set; }
    
    [FirestoreProperty("email")]
    public string? Email { get; set; }
    [FirestoreProperty("FirstnameLastname")]
    public string? Name { get; set; }
    [FirestoreProperty("admin")]
    public bool? WorkPosition { get; set; }
    [FirestoreProperty("transcriptionCount")]
    public int? TranscriptionCount { get; set; }
    [FirestoreProperty("lastTranscription")]
    public string? LastTranscription { get; set; }
    
}