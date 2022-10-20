﻿namespace BackendMecanicaElEnano.Dto
{
    public class VehiculoDto
    {
            public Guid VehiculoId { get; set; }

            public string Patente { get; set; } = null!;

            public string Modelo { get; set; } = null!;

            public string NumeroChasis { get; set; } = null!;

            public string Cliente { get; set; } = null!;

            public string Telefono { get; set; } = null!;

            public string Direccion { get; set; } = null!;

            public string Mail { get; set; } = null!;

            public string Cuit { get; set; } = null!;
        
    }
}
