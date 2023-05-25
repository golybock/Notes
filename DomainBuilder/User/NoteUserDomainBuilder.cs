using Database.User;
using Domain.Note;
using Domain.User;

namespace DomainBuilder.User;

public static class NoteUserDomainBuilder
{
    public static NoteUserDomain Create(NoteUserDatabase noteUserDatabase)
    {
        return new NoteUserDomain()
        {
            Email = noteUserDatabase.Email,
            Name = noteUserDatabase.Name
        };
    }
    
    public static NoteUserDomain Create(NoteUserDatabase noteUserDatabase, List<NoteDomain> noteDomains)
    {
        return new NoteUserDomain()
        {
            Email = noteUserDatabase.Email,
            Name = noteUserDatabase.Name,
            Notes = noteDomains
        };
    }
}