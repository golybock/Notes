using Blank.User;
using Database.User;

namespace DatabaseBuilder.User;

public static class NoteUserDatabaseBuilder
{
    public static NoteUserDatabase Create(NoteUserBlank noteUserBlank)
    {
        return new NoteUserDatabase()
        {
            Name = noteUserBlank.Name,
            Email = noteUserBlank.Email
        };
    } 
    
    public static NoteUserDatabase Create(NoteUserBlank noteUserBlank, string hashedPassword)
    {
        return new NoteUserDatabase()
        {
            Name = noteUserBlank.Name,
            Email = noteUserBlank.Email,
            PasswordHash = hashedPassword
        };
    } 
    
    public static NoteUserDatabase Create(int id, NoteUserBlank noteUserBlank)
    {
        return new NoteUserDatabase()
        {
            Id = id,
            Name = noteUserBlank.Name,
            Email = noteUserBlank.Email
        };
    } 
    
    public static NoteUserDatabase Create(int id, NoteUserBlank noteUserBlank, string hashedPassword)
    {
        return new NoteUserDatabase()
        {
            Id = id,
            Name = noteUserBlank.Name,
            Email = noteUserBlank.Email,
            PasswordHash = hashedPassword
        };
    } 
}