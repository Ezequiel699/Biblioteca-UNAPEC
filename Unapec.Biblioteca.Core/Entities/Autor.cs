namespace Unapec.Biblioteca.Core.Entities;

public class Autor {
    public int Id

{
    get;
    set;
}

public required string Nombre {
    get;
    set;
}

public string? PaisOrigen {
    get;
    set;
}

// FK a Idioma
public int IdiomaNativoId {
    get;
    set;
}

public Idioma? IdiomaNativo {
    get;
    set;
}

public bool Estado {
    get;
    set;
}

= true;
public DateTime CreadoEn {
    get;
    set;
}

= DateTime.UtcNow;
public DateTime? ActualizadoEn {
    get;
    set;
}
}
