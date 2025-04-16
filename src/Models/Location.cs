namespace CarRental.src.Models;
public class Location
{
    public Guid Id { get;set; }
    // Location codes: Palma Airport - PAP, Palma City Center - PCC, Alcudia - ALC, Manacor - MAN
    static readonly HashSet<string> Codes = ["PAP", "PCC", "ALC", "MAN"];
    public string LocationCode { get;set; }
    public string Name { get;set; }
    public string Address { get;set; }

    public Location(
        string locationCode,
        string name,
        string address
    )
    {
        Id = Guid.NewGuid();
        LocationCode = locationCode;
        Name = name;
        Address = address;
    }

    public static string NormalizeCode(string locationCode){
        if (String.IsNullOrEmpty(locationCode)) {
            throw new ArgumentException("Location code can't be null or empty.");
        }

        string normalizedCode = locationCode.Trim().ToUpper();
        if (!Codes.Contains(normalizedCode)){
            throw new ArgumentException($"Invalid location code: {normalizedCode}");
        };

        return normalizedCode;
    }
}